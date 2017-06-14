using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer
{
    class Server : IDisposable
    {
        private Socket serverSocket;
        private SocketWrapper socketWrapper;
        public Server(int port)
        {
            serverSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Start()
        {
            try
            {
                var channelsNotifier = new ChannelsNotifier();
                serverSocket.Listen(128);
                while (true)
                {
                    Socket acceptedSocket = serverSocket.Accept();
                    Task.Factory.StartNew(() =>
                    {

                        var channel = new NetworkChannel(acceptedSocket, channelsNotifier);
                        channel.ProcessMessage();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:\r\n{0}", ex.Message);
            }
        }

        

        public void Dispose()
        {
            serverSocket.Close();
        }
    }
}
