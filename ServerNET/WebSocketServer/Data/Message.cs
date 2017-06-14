using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Commons;
using WebSocketServer.Interfaces;

namespace WebSocketServer.Data
{
    [DataContract]
    class Message : IMessage
    {
        public Message(string type, object data)
        {
            Type = type;
            CreationDate = DateTime.Now;
            Data = data;
        }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; private set; }

        [DataMember(Name = "data")]
        public object Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, typeof (Message));
        }
    }
}
