using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using System;
using WebSocketSharp;

namespace DidiWebSocketTest.Models
{
    public class WebSocketTransport : ITransport
    {
        public event EventHandler<string> OnInfo;
        public event EventHandler<byte[]> OnMessage;
        public event EventHandler<string> OnError;
        WebSocket ws = null;

        public WebSocketTransport() { }
        public void Close()
        {
            if (ws != null) ws.Close();
        }
        public void Connect(IProtocol protocol)
        {
            if (ws != null && ws.ReadyState == WebSocketState.Open) return;
            ws = new WebSocket(protocol.Url, protocol.Protocol);
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.OnMessage += Ws_OnMessage;
            ws.OnError += Ws_OnError;
            ws.Connect();
        }
        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            OnError?.Invoke(sender, e.Message);
        }
        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                OnMessage?.Invoke(sender, e.RawData);
            }
            else
            {
                OnInfo?.Invoke(sender, e.Data);
            }
        }
        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            OnInfo?.Invoke(sender, "WebSocket Closed");
            ws.OnOpen -= Ws_OnOpen;
            ws.OnClose -= Ws_OnClose;
            ws.OnMessage -= Ws_OnMessage;
            ws.OnError -= Ws_OnError;
            ws = null;
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            OnInfo?.Invoke(sender, "WebSocket Opened");
        }
        public void SendMessage(MessageBase message)
        {
            if (ws != null && ws.ReadyState == WebSocketState.Open)
            {
                ws.Send(message.MessageBytes);
            }
            else
            {
                OnInfo?.Invoke(this, "Cannot send message, WebSocket not Open");
            }
        }
    }
}
