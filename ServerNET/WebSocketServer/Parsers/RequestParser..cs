using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer.Parsers
{
    class RequestParser
    {
        private const int CloseConnection = 0x08;
        private const int Pong = 0xA;

        public static bool CloseChannel(byte[] message)
        {
            return (message[0] & 127) == CloseConnection;
        }

        public static bool IsPongMessage(byte[] message)
        {
            return (message[0] & 127) == Pong;
        }
    }
}
