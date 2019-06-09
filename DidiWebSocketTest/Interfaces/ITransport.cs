using DidiWebSocketTest.Models.Messages;
using System;

namespace DidiWebSocketTest.Interfaces
{
    public interface ITransport
    {
        event EventHandler<byte[]> OnMessage;
        event EventHandler<string> OnInfo;
        event EventHandler<string> OnError;
        void Connect(IProtocol protocol);
        void Close();
        void SendMessage(MessageBase message);
    }
}
