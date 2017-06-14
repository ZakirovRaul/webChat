namespace WebSocketServer.Interfaces
{
    interface IChannel
    {
        void Notify(IMessage message);
    }
}
