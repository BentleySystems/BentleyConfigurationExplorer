using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkspaceCFG;

internal static class Program
{
    private static readonly JsonSerializerSettings Settings =
        (JsonSerializerSettings)typeof(WorkspaceExport)
            .GetField("WorkspaceJsonSettings", BindingFlags.NonPublic | BindingFlags.Static)
            .GetValue(null);

    private static int Main(string[] args)
    {
        string directory = Path.Combine(Path.GetTempPath(), "BCE-persistence-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "workspace.bcf");
            bool silent = true;
            CFGConfiguration original = CreateWorkspace();
            WorkspaceExport.SaveWorkspace(ref path, ref original, ref silent);
            Assert(File.Exists(path), "Workspace save failed.");
            AssertGraph(Load(path));
            Console.WriteLine("PASS: save/load preserves model types, values, cycles and shared references");

            string legacy = JsonConvert.SerializeObject(CreateWorkspace(), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });
            File.WriteAllText(path, legacy);
            AssertGraph(Load(path));
            Console.WriteLine("PASS: existing JSON workspace format remains readable");

            string deniedType = typeof(ConstructionProbe).AssemblyQualifiedName;
            var probe = new JObject { ["$type"] = deniedType };
            foreach (string member in new[] { "CFGFiles", "CFGEvents", "ChildFiles" })
            {
                JObject document = Wrap(member, probe);
                AssertRejected(document.ToString(), path, "not permitted");
            }
            Assert(ConstructionProbe.Created == 0, "Unauthorized constructor was executed.");
            Console.WriteLine("PASS: unauthorized constructors blocked at every ArrayList entry point");

            foreach (string typeName in new[]
            {
                "System.Windows.Data.ObjectDataProvider, PresentationFramework",
                "System.Diagnostics.Process, System.Diagnostics.Process",
                "WorkspaceCFG.MainForm, ConfigurationExplorer",
                "WorkspaceCFG.CFGFile, UntrustedAssembly",
                "System.Collections.Generic.List`1[[System.Diagnostics.Process, System.Diagnostics.Process]], System.Private.CoreLib"
            })
            {
                AssertRejected(new JObject { ["$type"] = typeName }.ToString(), path, "not permitted");
            }
            Console.WriteLine("PASS: gadgets, non-model application types, assembly spoofing and generic types rejected");

            var deeplyNested = new JObject();
            JObject current = deeplyNested;
            for (int depth = 0; depth < 140; depth++)
            {
                var next = new JObject();
                current["nested"] = next;
                current = next;
            }
            AssertRejected(deeplyNested.ToString(Formatting.None), path, "MaxDepth");
            Console.WriteLine("PASS: excessive JSON nesting rejected");

            if (args.Length > 0)
            {
                AssertRejected(File.ReadAllText(args[0]), path, "not permitted");
                Console.WriteLine("PASS: supplied .bcf payload rejected by serializer and real file loader");
            }
            AssertSafeReload(directory);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("PASS: partially deserialized models can be finalized safely");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
        finally
        {
            FileWatcher.ClearWatches();
            Directory.Delete(directory, true);
        }
    }

    private static void AssertSafeReload(string directory)
    {
        string productDirectory = Path.Combine(directory, "untrusted-product");
        Directory.CreateDirectory(productDirectory);
        string productDll = Path.Combine(productDirectory, "Bentley.PowerPlatform.FeatureAspects.dll");
        File.Copy(typeof(Program).Assembly.Location, productDll);
        string marker = Path.Combine(directory, "unexpected-code-execution.txt");
        string previousMarker = Environment.GetEnvironmentVariable("BCE_RELOAD_MARKER");
        CultureInfo previousCulture = CultureInfo.CurrentCulture;
        Environment.SetEnvironmentVariable("BCE_RELOAD_MARKER", marker);
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        try
        {
            var original = new CFGConfiguration { DisableEventRecording = true };
            original.ApplicationData.Name = "MicroStation Regression";
            original.ApplicationData.EXEPath = Path.Combine(productDirectory, "MicroStation.exe");
            original.ApplicationData.Version = "10.0.0.0";
            original.ApplicationData.StartupCfgPath = Path.Combine(directory, "startup.cfg");
            original.ApplicationData.ConfigurationRoot = directory;
            File.WriteAllText(original.ApplicationData.StartupCfgPath, "RELOAD_TEST = safe");
            string path = Path.Combine(directory, "reload.bcf");
            bool silent = true;
            WorkspaceExport.SaveWorkspace(ref path, ref original, ref silent);

            CFGConfiguration workspace = Load(path);
            workspace.ProcessConfiguration();
            Assert(workspace.GetVariable("_USTN_PRODUCT_CONFIGVARNAME")?.Value == "RESOURCE_PRODUCT",
                "Reload did not read product string resources.");
            Assert(workspace.GetVariable("_USTN_PRODUCT_NAME")?.Value == "Resource Product",
                "Product name resource lost.");
            Assert(workspace.GetVariable("_USTN_PRODUCT_FILETYPENAME")?.Value == "",
                "Non-string resource was not skipped.");
            Assert(workspace.GetVariable("_USTN_PRODUCT_VERSION_MAJOR")?.Value == "2" &&
                workspace.GetVariable("_USTN_PRODUCT_VERSION_BUILD")?.Value == "0",
                "Short product version was not handled safely.");
            Assert(!AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                !assembly.IsDynamic && string.Equals(assembly.Location, productDll, StringComparison.OrdinalIgnoreCase)),
                "Reload loaded the untrusted product assembly.");
            Assert(!File.Exists(marker), "Reload executed an untrusted static initializer or getter.");
            Console.WriteLine("PASS: .bcf reload reads only string metadata without loading or executing the product DLL");

            Type productType = typeof(Bentley.PowerPlatform.Resx);
            Assert((string)workspace.GetPropertyInfoValue(ref productType, "ProductNames_ConfigVarName") == "RESOURCE_PRODUCT",
                "Public metadata accessor did not use resource data.");
            workspace.SetProductInfo(ref productType);
            Assert(!File.Exists(marker), "Public metadata API executed a product getter.");
            Console.WriteLine("PASS: public product metadata APIs do not invoke getters");

            File.WriteAllText(productDll, "not a managed PE image");
            workspace = Load(path);
            workspace.ProcessConfiguration();
            Assert(workspace.GetVariable("_USTN_PRODUCT_NAME") == null,
                "Corrupt product image supplied metadata.");
            File.Delete(productDll);
            workspace = Load(path);
            workspace.ProcessConfiguration();
            Assert(workspace.GetVariable("_USTN_PRODUCT_NAME") == null && !File.Exists(marker),
                "Missing product image was not handled safely.");
            Console.WriteLine("PASS: corrupt and missing product DLLs are skipped safely");
        }
        finally
        {
            Environment.SetEnvironmentVariable("BCE_RELOAD_MARKER", previousMarker);
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    private static CFGConfiguration CreateWorkspace()
    {
        var workspace = new CFGConfiguration { Description = "Regression workspace" };
        var parent = new CFGFile(ref workspace) { Name = "parent.cfg" };
        var child = new CFGFile(ref workspace) { Name = "child.cfg", ParentFile = parent };
        parent.ChildFiles.Add(child);
        workspace.CFGFiles.Add(parent);
        workspace.CFGFiles.Add(child);
        var line = new CFGLine { ParentFile = child, Text = "TEST = value", LineNumber = 1 };
        line.Tokens.Add(new CFGToken { Value = "TEST" });
        child.Lines.Add(line);
        var variable = new CFGVariable(workspace) { Name = "TEST", Value = "value", FinalExpansion = "value" };
        variable.Args[0] = "argument";
        variable.Values[0] = "value";
        workspace.Variables.Add(variable.Name, variable);
        workspace.CFGEvents.Add(new CFGEvent { ParentFile = child, ParentLine = line, Variable = variable });
        return workspace;
    }

    private static void AssertGraph(CFGConfiguration workspace)
    {
        Assert(workspace != null && workspace.Description == "Regression workspace", "Workspace did not load.");
        var parent = (CFGFile)workspace.CFGFiles[0];
        var child = (CFGFile)workspace.CFGFiles[1];
        var cfgEvent = (CFGEvent)workspace.CFGEvents[0];
        Assert(ReferenceEquals(parent.ChildFiles[0], child), "Shared child reference lost.");
        Assert(ReferenceEquals(child.ParentFile, parent), "Parent file cycle lost.");
        Assert(ReferenceEquals(child.ParentConfiguration, workspace), "Workspace cycle lost.");
        Assert(ReferenceEquals(child.Lines[0].ParentFile, child), "Line cycle lost.");
        Assert(ReferenceEquals(cfgEvent.ParentFile, child), "Event file reference lost.");
        Assert(ReferenceEquals(cfgEvent.ParentLine, child.Lines[0]), "Event line reference lost.");
        CFGVariable variable = workspace.Variables["TEST"];
        Assert(ReferenceEquals(cfgEvent.Variable, variable), "Event variable reference lost.");
        Assert(ReferenceEquals(variable.ParentConfiguration, workspace), "Variable cycle lost.");
        Assert(variable.Value == "value" && variable.Args[0] == "argument" && variable.Values[0] == "value",
            "Variable values or arrays lost.");
    }

    private static JObject Wrap(string member, JObject item)
    {
        var collection = new JObject { ["$values"] = new JArray(item.DeepClone()) };
        if (member != "ChildFiles")
            return new JObject { [member] = collection };

        return new JObject
        {
            ["CFGFiles"] = new JObject
            {
                ["$values"] = new JArray(new JObject
                {
                    ["$type"] = typeof(CFGFile).AssemblyQualifiedName,
                    ["ChildFiles"] = collection
                })
            }
        };
    }

    private static void AssertRejected(string json, string path, string expectedError)
    {
        bool rejected = false;
        try
        {
            JsonConvert.DeserializeObject<CFGConfiguration>(json, Settings);
        }
        catch (JsonException exception)
        {
            rejected = exception.ToString().Contains(expectedError, StringComparison.OrdinalIgnoreCase);
        }
        Assert(rejected, "Serializer did not reject the input for the expected reason: " + expectedError);
        File.WriteAllText(path, json);
        CFGConfiguration existing = CreateWorkspace();
        CFGConfiguration loaded = existing;
        bool silent = true;
        WorkspaceExport.LoadConfiguration(ref path, ref loaded, ref silent);
        Assert(ReferenceEquals(existing, loaded), "Rejected input replaced the current workspace.");
    }

    private static CFGConfiguration Load(string path)
    {
        CFGConfiguration workspace = null;
        bool silent = true;
        WorkspaceExport.LoadConfiguration(ref path, ref workspace, ref silent);
        return workspace;
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    public sealed class ConstructionProbe
    {
        public static int Created;

        public ConstructionProbe()
        {
            Created++;
        }
    }
}

namespace Bentley.PowerPlatform
{
    public static class Resx
    {
        static Resx()
        {
            string marker = Environment.GetEnvironmentVariable("BCE_RELOAD_MARKER");
            if (!string.IsNullOrEmpty(marker))
                File.WriteAllText(marker, "Unexpected execution of product code");
        }

        public static string ProductNames_ConfigVarName => "EXECUTED_GETTER";
        public static string ProductVersion => "99.99.99.99";
    }
}