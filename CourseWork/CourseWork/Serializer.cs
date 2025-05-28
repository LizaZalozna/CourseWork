using System.IO;
using System.Xml.Serialization;

namespace CourseWork
{
    public static class Serializer
    {
        public static void SaveToXml<T>(T obj, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using FileStream stream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(stream, obj);
        }

        public static T LoadFromXml<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using FileStream stream = new FileStream(filePath, FileMode.Open);
            return (T)serializer.Deserialize(stream);
        }
    }
}