using DidiWebSocketTest.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DidiWebSocketTest.Interfaces
{
    public interface IProtocol
    {
        string Protocol { get; }
        string Url { get; }

        event EventHandler<MessageBase> OnMessage;
        void SendMessage(MessageBase message);
        void Connect();
        void Close();
    }
}
