using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization
{
    public static class JsonSerialization
    {
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static void SaveToFile<T>(T obj, string filePath)
        {
            var json = Serialize(obj);
            File.WriteAllText(filePath, json);
        }

        public static T? LoadFromFile<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return Deserialize<T>(json);
        }
    }

}
