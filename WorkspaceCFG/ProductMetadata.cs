using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Resources;

namespace WorkspaceCFG
{
    internal static class ProductMetadata
    {
        internal static Dictionary<string, string> Read(string path)
        {
            var values = new Dictionary<string, string>(StringComparer.Ordinal);
            using var stream = File.OpenRead(path);
            using var image = new PEReader(stream);
            if (!image.HasMetadata || image.PEHeaders.CorHeader.ResourcesDirectory.Size == 0)
                return values;

            MetadataReader metadata = image.GetMetadataReader();
            var directory = image.PEHeaders.CorHeader.ResourcesDirectory;
            var resources = image.GetSectionData(directory.RelativeVirtualAddress)
                .GetContent(0, directory.Size).ToArray();
            foreach (ManifestResourceHandle handle in metadata.ManifestResources)
            {
                ManifestResource resource = metadata.GetManifestResource(handle);
                if (!resource.Implementation.IsNil ||
                    !metadata.GetString(resource.Name).EndsWith(".resources", StringComparison.Ordinal))
                    continue;

                using var resourceStream = new MemoryStream(resources, false);
                resourceStream.Position = checked((int)resource.Offset);
                using var reader = new BinaryReader(resourceStream);
                int length = reader.ReadInt32();
                if (length < 0 || length > resourceStream.Length - resourceStream.Position)
                    throw new BadImageFormatException("Invalid embedded resource length.");

                using var data = new MemoryStream(reader.ReadBytes(length), false);
                using var resourceReader = new ResourceReader(data);
                IDictionaryEnumerator entries = resourceReader.GetEnumerator();
                while (entries.MoveNext())
                {
                    string name = (string)entries.Key;
                    if (!name.StartsWith("ProductNames_", StringComparison.Ordinal) &&
                        name != "ProductVersion" && name != "CopyrightYears")
                        continue;

                    resourceReader.GetResourceData(name, out string type, out byte[] bytes);
                    if (type != "ResourceTypeCode.String")
                        continue;

                    using var valueStream = new MemoryStream(bytes, false);
                    using var valueReader = new BinaryReader(valueStream);
                    values[name] = valueReader.ReadString();
                }
            }
            return values;
        }
    }
}