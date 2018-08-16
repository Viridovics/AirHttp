using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AirHttp.Configuration;
using AirHttp.Protocols;

namespace AirHttp.SystemXml.Configuration
{
    public class SystemXmlAirContentProcessor : IAirContentProcessor
    {
        public string ContentType => ContentTypes.Xml;

        public T DeserializeObject<T>(string serializedObject)
        {
            var serializer = new XmlSerializer(typeof(T));
            using(var serializaedObjectStream = GenerateStreamFromString(serializedObject))
            {
                return (T)serializer.Deserialize(serializaedObjectStream);
            }
        }

        public string SerializeObject<T>(T pocoObject)
        {
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using(var ms = new MemoryStream())
            {
                var writer = XmlWriter.Create(ms, settings);

                var cs = new XmlSerializer(typeof(T));

                cs.Serialize(writer, pocoObject);

                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                using(StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
