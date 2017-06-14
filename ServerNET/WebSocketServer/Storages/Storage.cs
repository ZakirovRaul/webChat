using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer.Storages
{
    class Storage
    {
        private static UsersStorage usersStorage;
        private static MessagesStorage messagesStorage;

        public static UsersStorage UsersStorage
        {
            get
            {
                if (usersStorage == null)
                {
                    usersStorage = new UsersStorage();
                }
                return usersStorage;
            }
        }

        public static MessagesStorage MessagesStorage
        {
            get
            {
                if (messagesStorage == null)
                {
                    messagesStorage = new MessagesStorage();
                }
                return messagesStorage;
            }
        }
    }
}
