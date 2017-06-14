using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer.Interfaces
{
    interface IMessage
    {
        string Type { get; set; }
        DateTime CreationDate { get; }
        object Data { get; set; }

    }
}
