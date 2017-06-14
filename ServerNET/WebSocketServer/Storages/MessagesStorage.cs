using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Commons;

namespace WebSocketServer.Storages
{
    public class MessagesStorage : IStorage<KeyValuePair<string,string>>
    {
        private List<KeyValuePair<string, string>> messages = new List<KeyValuePair<string, string>>();


        public void Add(KeyValuePair<string, string> item)
        {
            messages.Add(item);
        }

        public object GetAll()
        {
            //if (messages.Count > 10)
            //{
            //    return JsonSerializer.Serialize(messages.Skip(10).ToArray(), typeof(KeyValuePair<string, string>[]));
            //}
            //else
            //{
            //    return JsonSerializer.Serialize(messages.ToArray(), typeof(KeyValuePair<string, string>[]));
            //}
            if (messages.Count > 10)
            {
                return messages.Skip(10).ToArray();
            }
            else
            {
                return messages.ToArray();
            }
        }

        public void Remove(KeyValuePair<string, string> item)
        {
            if (messages.Contains(item))
            {
                messages.Remove(item);
            }
        }
    }
}
