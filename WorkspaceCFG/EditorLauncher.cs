using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace WorkspaceCFG
{
    internal static class EditorLauncher
    {
        internal static bool TryOpen(string filename, int line)
        {
            foreach (ProcessStartInfo startInfo in GetStartInfos(filename, line))
            {
                if (!File.Exists(startInfo.FileName))
                    continue;

                try
                {
                    using Process process = Process.Start(startInfo);
                    if (process != null)
                        return true;
                }
                catch (Win32Exception exception)
                {
                    Debug.WriteLine($"  [ERROR] EditorLauncher: {exception.Message}");
                }
                catch (InvalidOperationException exception)
                {
                    Debug.WriteLine($"  [ERROR] EditorLauncher: {exception.Message}");
                }
            }
            return false;
        }

        internal static IEnumerable<ProcessStartInfo> GetStartInfos(string filename, int line)
        {
            string fullPath = Path.GetFullPath(filename);
            string[] installationRoots =
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs")
            };

            foreach (string editor in new[] { "Notepad++", "Microsoft VS Code" })
            {
                foreach (string root in installationRoots)
                {
                    if (!Path.IsPathFullyQualified(root))
                        continue;

                    string executable = editor == "Notepad++" ? "notepad++.exe" : "Code.exe";
                    var startInfo = CreateStartInfo(Path.Combine(root, editor, executable));
                    if (editor == "Notepad++")
                    {
                        if (line > 0)
                            startInfo.ArgumentList.Add("-n" + line.ToString(CultureInfo.InvariantCulture));
                        startInfo.ArgumentList.Add(fullPath);
                    }
                    else
                    {
                        startInfo.ArgumentList.Add("--goto");
                        startInfo.ArgumentList.Add(fullPath + ":" + Math.Max(1, line).ToString(CultureInfo.InvariantCulture));
                    }
                    yield return startInfo;
                }
            }

            if (Path.IsPathFullyQualified(Environment.SystemDirectory))
            {
                var startInfo = CreateStartInfo(Path.Combine(Environment.SystemDirectory, "notepad.exe"));
                startInfo.ArgumentList.Add(fullPath);
                yield return startInfo;
            }
        }

        private static ProcessStartInfo CreateStartInfo(string executable)
        {
            return new ProcessStartInfo
            {
                FileName = executable,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(executable)
            };
        }
    }
}