using Newtonsoft.Json;
using System.IO;

namespace JToolbox.Misc.Serializers
{
    public class SerializerJson : ISerializer
    {
        public void ToFile<T>(T obj, string filePath)
        {
            var serialized = ToString(obj);
            File.WriteAllText(filePath, serialized);
        }

        public T FromFile<T>(string filePath)
        {
            var serialized = File.ReadAllText(filePath);
            return FromString<T>(serialized);
        }

        public string ToString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T FromString<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}