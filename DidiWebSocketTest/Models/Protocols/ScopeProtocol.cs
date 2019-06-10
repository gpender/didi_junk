using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using System;

namespace DidiWebSocketTest.Models
{
    public class ScopeProtocol : IProtocol
    {
        public event EventHandler<MessageBase> OnMessage;
        public event EventHandler<string> OnInfo;
        public event EventHandler<string> OnError;
        ITransport transport;
        string ipAddress;
        public string Protocol { get { return "scope"; } }
        public string Url { get { return $"ws://{ipAddress}/chat"; } }
        public ScopeProtocol(ITransport transport, string ipAddress)
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
        private void Transport_OnMessage(object sender, byte[] frame)
        {
            MessageBase msg = null;
            if (frame[2] == 0x00)
            {
                msg = new ScopeParametersMessage(frame);
            }
            if (frame[2] == 0x01)
            {
                msg = new ScopeConfigMessage(frame);
            }
            if (frame[2] == 0x02)
            {
                msg = new ScopeBufferMessage(frame);
            }
            OnMessage?.Invoke(sender, msg);
        }
        public void SendMessage(MessageBase message)
        {
            transport.Connect(this);
            //transport.SendMessage(message);
        }
        public void GetScopeParameters()
        {
            transport.Connect(this);
            transport.SendMessage(new ScopeParametersMessage());
        }
        public void GetScopeConfigParameters()
        {
            transport.Connect(this);
            transport.SendMessage(new ScopeConfigMessage());
        }
        public void GetScopeBuffer()
        {
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
