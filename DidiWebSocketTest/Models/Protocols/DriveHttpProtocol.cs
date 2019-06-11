using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using DidiWebSocketTest.Models.MessagesHttp;
using System;

namespace DidiWebSocketTest.Models
{
    public class DriveHttpProtocol : IProtocol
    {
        public event EventHandler<IMessage> OnMessage;
        public event EventHandler<string> OnInfo;
        public event EventHandler<string> OnError;
        ITransport transport;
        string ipAddress;
        public string Protocol { get { return "parameter"; } }
        public string Url { get { return $"http://{ipAddress}/"; } }
        public DriveHttpProtocol(ITransport transport, string ipAddress)
        {
            this.transport = transport;
            this.transport.OnMessage += Transport_OnMessage;
            this.transport.OnError += Transport_OnInfo;
            this.transport.OnInfo += Transport_OnInfo;
            this.ipAddress = ipAddress;
        }

        private void Transport_OnInfo(object sender, string info)
        {
            OnInfo?.Invoke(this, info);
        }
        private void Transport_OnMessage(object sender, byte[] frame)
        {
        }
        public void SendMessage(IMessage message)
        {
            transport.SendMessage(message);
        }
        public void GetMenus()
        {
            MenusMessage message = new MenusMessage(ipAddress);
            transport.SendRequestResponseMessage(message);
        }
        public void GetParameterAttributes(int tag)
        {
            ParameterAttributesMessage message = new ParameterAttributesMessage(ipAddress, tag);
            transport.SendRequestResponseMessage(message);
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
