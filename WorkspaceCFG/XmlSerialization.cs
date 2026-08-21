// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Xml.Serialization;

namespace WorkspaceCFG
{

    /// <summary>
/// XML serialization for generic instances.
/// </summary>
    public static class XmlSerialization
    {
        /// <summary>
    /// Instantiate an object of arbitrary type from an XML file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullpath">Fullpath to XML file to deserialize.</param>
    /// <returns>An object of type T.</returns>
        public static T DeserializeFromFile<T>(string fullpath)
        {
            try
            {
                using (var fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(fileStream);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
    /// Serialize an object to XML file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">Object to serialize.</param>
    /// <param name="fullpath">Fullpath of XML file to serialize to.</param>
    /// <returns>XML string.</returns>
        public static string SerializeToFile<T>(T obj, string fullpath)
        {
            try
            {
                using (var fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(fileStream, obj);
                    return fileStream.ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
    /// Serialize an object to Stream.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">Object to serialize.</param>
    /// <param name="stream">Stream to serialize to.</param>
        public static void SerializeToStream<T>(T obj, Stream stream)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, obj);
            }
            catch
            {
                throw;
            }
        }
    }
}