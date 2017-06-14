using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebSocketServer
{
    internal class HandShaker
    {
        private SocketWrapper socketWrapper;
        public HandShaker(SocketWrapper socketWrapper)
        {
            this.socketWrapper = socketWrapper;
        }

        public bool Shake(out string name)
        {
            string request = Encoding.UTF8.GetString(socketWrapper.Read());
            name = Regex.Match(request, @"^GET\s/\?name=(?<name>.+)\sHTTP").Groups["name"].ToString();

            string webSocketKey;
            if (WantToShakeAHand(request, out webSocketKey))
            {
                string response = CreateHandShakeResponse(webSocketKey);
                socketWrapper.Write(response);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool WantToShakeAHand(string request, out string webSocketKey)
        {
            var headerKeys = request.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var key = headerKeys.FirstOrDefault(x => x.Contains("Sec-WebSocket-Key:"));
            if (!string.IsNullOrEmpty(key))
            {
                webSocketKey = key.Replace("Sec-WebSocket-Key: ", string.Empty);
                return true;
            }
            else
            {
                webSocketKey = null;
                return false;
            }
        }

        public string CreateHandShakeResponse(string webSocketKey)
        {
            webSocketKey = webSocketKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var responseKey =
                Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(webSocketKey)));

            string response =
                string.Format(
                    "HTTP/1.1 101 Switching Protocols\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: {0}\r\nSec-WebSocket-Protocol: chat\r\n\r\n",
                    responseKey);
            return response;
        }

    }
}
