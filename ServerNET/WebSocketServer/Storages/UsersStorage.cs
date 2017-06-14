using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Commons;

namespace WebSocketServer.Storages
{
    class UsersStorage : IStorage<string>
    {
        private List<string> users = new List<string>();

        public void Add(string user)
        {
            users.Add(user);
        }

        public object GetAll()
        {
            //return JsonSerializer.Serialize(users.ToArray(), typeof(string[]));
            return users.ToArray();
        }

        public void Remove(string user)
        {
            if (users.Contains(user))
            {
                users.Remove(user);
            }
        }


    }
}
