using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.Interfaces;

namespace WebSocketServer
{
    class SocketWrapper
    {
        private Socket socket;
        public SocketWrapper(Socket socket)
        {
            this.socket = socket;
        }

        public bool Connected
        {
            get { return socket.Connected; }
        }

        public byte[] Read()
        {
            var data = new List<byte>();
            using (var netStream = new NetworkStream(socket))
            {
                ReadBytes(netStream, data);
                return data.ToArray();

            }
        }

        private void ReadBytes(NetworkStream stream, List<byte> data)
        {
            var buffer = new byte[1];
            stream.Read(buffer, 0, buffer.Length);
            data.AddRange(buffer);
            if (stream.DataAvailable)
            {
                ReadBytes(stream, data);
            }
        }

        public string ReadMessage()
        {
            var message = Read();
            return FrameHelper.DecodeMessage(message);
        }

        public void Write(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            socket.Send(buffer);
        }

        public void Write(byte[] data)
        {
            socket.Send(data);
        }

        public void Close()
        {
            socket.Close();
        }

        public void Write(IMessage message)
        {
            var jsonMessage = FrameHelper.EncodeMessageToSend(message.ToString());
            socket.Send(jsonMessage);
        }
    }
}
