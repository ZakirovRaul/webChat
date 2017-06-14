using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.Interfaces;
using WebSocketServer.Storages;

namespace WebSocketServer.Data
{
    class MessageFactory
    {
        public static IMessage CreateMessage(string data)
        {
            return new Message(MessageType.MESSAGE, data);
        }

        public static IMessage CreateUsersList()
        {
            return new Message(MessageType.USERLIST, Storage.UsersStorage.GetAll());
        }

        public static IMessage CreateHistory()
        {
            return new Message(MessageType.HISTORY, Storage.MessagesStorage.GetAll());
        }
    }
}
