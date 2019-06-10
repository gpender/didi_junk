using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DidiWebSocketTest.Models
{
    public class TestProtocol : IProtocol
    {
        public event EventHandler<MessageBase> OnMessage;
        public event EventHandler<string> OnInfo;
        public event EventHandler<string> OnError;
        ITransport transport;
        string ipAddress;
        public string Protocol { get { return "test"; } }
        public string Url { get { return $"ws://{ipAddress}/chat"; } }
        public TestProtocol(ITransport transport, string ipAddress)
        {
            this.transport = transport;
            this.transport.OnMessage += Transport_OnMessage;
            this.transport.OnError += Transport_OnError;
            this.transport.OnInfo += Transport_OnInfo;
            this.ipAddress = ipAddress;
        }
        private void Transport_OnInfo(object sender, string info)
        {
            OnInfo?.Invoke(this, info);
        }
        private void Transport_OnError(object sender, string err)
        {
            OnError?.Invoke(sender, err);
        }
        private void Transport_OnMessage(object sender, byte[] msg)
        {
            if(msg.Length > 100)
            {
                OnMessage?.Invoke(sender, new ImageMessage(msg));
            }
            else
            {
                OnMessage?.Invoke(sender, new HelloMessage(msg));
            }
        }
        public void SendMessage(MessageBase message)
        {
            transport.Connect(this);
            transport.SendMessage(message);
        }
        public void Connect()
        {
            transport.Connect(this);
        }

        public void Close()
        {
            transport.Close();
        }
    }
}
