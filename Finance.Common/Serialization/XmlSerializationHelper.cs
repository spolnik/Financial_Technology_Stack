using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Finance.Common.Serialization
{
    public static class XmlSerializationHelper<TSerializationType>
    {
        /// <summary>
        /// Serializes an object using <see cref="XmlSerializer"/> to string inside the <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static string SerializeToString(TSerializationType objectToSerialize)
        {
            var serializer = new XmlSerializer(typeof(TSerializationType));
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, objectToSerialize);
                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Serializes an object using <see cref="XmlSerializer"/> to file using the <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <param name="filePath"></param>
        public static void SerializeToFile(TSerializationType objectToSerialize, string filePath)
        {
            var serializer = new XmlSerializer(typeof(TSerializationType));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, objectToSerialize);
            }
        }

        /// <summary>
        /// Using the <see cref="TextReader"/>, reads the file and deserializes an object using <see cref="XmlSerializer"/>.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TSerializationType DeserializeFromFile(string filePath)
        {
            var deserializer = new XmlSerializer(typeof(TSerializationType));
            TSerializationType obj;
            using (var reader = new StreamReader(filePath))
            {
                obj = (TSerializationType)deserializer.Deserialize(reader);
            }
            return obj;
        }

        /// <summary>
        /// This method deserializes the <see cref="string"/> <paramref name="fileContent"/> to the actual <typeparamref name="TSerializationType"/> object.
        /// </summary>
        /// <param name="fileContent">Xml representation of the object to deserialize</param>
        /// <returns></returns>
        public static TSerializationType DeserializeFromString(string fileContent)
        {
            var deserializer = new XmlSerializer(typeof(TSerializationType));
            TSerializationType obj;
            using (var reader = new StringReader(fileContent))
            {
                obj = (TSerializationType)deserializer.Deserialize(reader);
            }
            return obj;
        }

        public static void SerializeToFile(TSerializationType data, FileInfo fileInfo)
        {
            var serializer = new XmlSerializer(typeof(TSerializationType));
            using (var fileStream = fileInfo.OpenWrite())
            {
                serializer.Serialize(fileStream, data);
            }
        }

        public static TSerializationType DeserializeFromFile(FileInfo fileInfo)
        {
            var deserializer = new XmlSerializer(typeof(TSerializationType));
            using (var fileStream = fileInfo.OpenRead())
            {
                return (TSerializationType)deserializer.Deserialize(fileStream);
            }
        }

        public static byte[] SerializeToBytes(TSerializationType data)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, data);
                var buffer = memoryStream.GetBuffer();
                return buffer;
            }
        }

        public static TSerializationType DeserializeFromBytes(byte[] buffer)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(buffer))
                return (TSerializationType)binaryFormatter.Deserialize(memoryStream);
        }
    }
}