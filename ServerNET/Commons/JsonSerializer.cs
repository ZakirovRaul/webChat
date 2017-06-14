using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class JsonSerializer
    {
        public static string Serialize(object data, Type type)
        {
            var knownTypes = new List<Type>(){ typeof(KeyValuePair<string,string>[]), typeof(string[])};
            var jsonSerializer = new DataContractJsonSerializer(type, knownTypes);
            using (var stream = new MemoryStream())
            {
                jsonSerializer.WriteObject(stream, data);
                var buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0 , buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
