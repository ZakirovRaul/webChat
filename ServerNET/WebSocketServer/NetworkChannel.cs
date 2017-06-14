using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketServer.Data;
using WebSocketServer.Interfaces;
using WebSocketServer.Parsers;
using WebSocketServer.Storages;

namespace WebSocketServer
{
    class NetworkChannel : IChannel, IDisposable
    {
        private SocketWrapper socketWrapper;
        private ChannelsNotifier channelsNotifier;
        private HandShaker handShaker;
        public NetworkChannel(Socket socket, ChannelsNotifier channelsNotifier)
        {
            socketWrapper = new SocketWrapper(socket);
            handShaker = new HandShaker(socketWrapper);
            this.channelsNotifier = channelsNotifier;
            channelsNotifier.AddChannel(this);
        }

        public void ProcessMessage()
        {
            string userName;
            if (handShaker.Shake(out userName))
            {
                Task.Factory.StartNew(() =>
                {
                    while (socketWrapper.Connected)
                    {
                        socketWrapper.Write(FrameHelper.Ping());
                        Thread.Sleep(25000);
                    }
                });
                Notify(MessageFactory.CreateHistory());
                Notify(MessageFactory.CreateMessage(string.Format("{0} has connected", userName)));

                Storage.UsersStorage.Add(userName);
                channelsNotifier.NotifyAll(MessageFactory.CreateUsersList());

                while (true)
                {
                    var message = socketWrapper.Read();
                    if (RequestParser.CloseChannel(message))
                    {
                        socketWrapper.Close();
                        Storage.UsersStorage.Remove(userName);
                        channelsNotifier.RemoveChannel(this);
                        channelsNotifier.NotifyAll(MessageFactory.CreateUsersList());
                        channelsNotifier.NotifyAll(MessageFactory.CreateMessage(string.Format("{0} has disconnected", userName)));
                    }
                    else
                    {
                        if (!RequestParser.IsPongMessage(message))
                        {
                            var messageStr = FrameHelper.DecodeMessage(message);
                            Storage.MessagesStorage.Add(new KeyValuePair<string, string>(userName, messageStr));
                            channelsNotifier.Notify(this, MessageFactory.CreateMessage(string.Format("{0}:{1}", userName, messageStr)));
                        }
                    }
                }
            }
        }

        public void Notify(IMessage message)
        {
            socketWrapper.Write(message); 
        }

        public void Dispose()
        {
            channelsNotifier.RemoveChannel(this);
        }
    }
}
