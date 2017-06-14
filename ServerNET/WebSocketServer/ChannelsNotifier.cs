using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.Interfaces;

namespace WebSocketServer
{
    class ChannelsNotifier
    {
        private List<IChannel> items;

        public ChannelsNotifier()
        {
            items = new List<IChannel>();
        }

        public void AddChannel(IChannel channel)
        {
            items.Add(channel);
        }

        public void RemoveChannel(IChannel channel)
        {
            items.Remove(channel);
        }

        public void Notify(IChannel channel, IMessage message)
        {
            foreach(IChannel item in items)
            {
                if (item != channel)
                {
                    item.Notify(message);
                }
            }
        }

        public void NotifyAll(IMessage message)
        {
            foreach (IChannel item in items)
            {
                item.Notify(message);
            }
        }


    }
}
