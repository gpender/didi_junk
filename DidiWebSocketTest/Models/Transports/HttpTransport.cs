using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace DidiWebSocketTest.Models
{
    public class HttpTransport : ITransport
    {
        //public event EventHandler<string> OnInfo;
        //public event EventHandler<byte[]> OnMessage;
        //public event EventHandler<string> OnError;
        //WebSocket ws = null;

        //public HttpTransport()
        //{

        //}

        //public void Close()
        //{
        //    if (ws != null) ws.Close();
        //}

        //public void Connect(IProtocol protocol)
        //{
        //    if (ws != null && ws.ReadyState == WebSocketState.Open) return;
        //    ws = new WebSocket(protocol.Url, protocol.Protocol);
        //    ws.OnOpen += Ws_OnOpen;
        //    ws.OnClose += Ws_OnClose;
        //    ws.OnMessage += Ws_OnMessage;
        //    ws.OnError += Ws_OnError;
        //    ws.Connect();
        //}
        //private void Ws_OnError(object sender, ErrorEventArgs e)
        //{
        //    OnError?.Invoke(sender, e.Message);
        //}

        //private void Ws_OnMessage(object sender, MessageEventArgs e)
        //{
        //    if (e.IsBinary)
        //    {
        //        OnMessage?.Invoke(sender, e.RawData);
        //    }
        //    else
        //    {
        //        OnInfo?.Invoke(sender, e.Data);
        //    }
        //}

        //private void Ws_OnClose(object sender, CloseEventArgs e)
        //{
        //    OnInfo?.Invoke(sender, "WebSocket Closed");
        //    ws.OnOpen -= Ws_OnOpen;
        //    ws.OnClose -= Ws_OnClose;
        //    ws.OnMessage -= Ws_OnMessage;
        //    ws.OnError -= Ws_OnError;
        //}

        //private void Ws_OnOpen(object sender, EventArgs e)
        //{
        //    OnInfo?.Invoke(sender, "WebSocket Opened");
        //}
        //public void SendMessage(string message)
        //{
        //    if (ws != null) ws.Send(message);
        //}
        //public void SendMessage(MessageBase message)
        //{
        //    if (ws != null) ws.Send(message.MessageBytes);
        //}
        public event EventHandler<byte[]> OnMessage;
        public event EventHandler<string> OnInfo;
        public event EventHandler<string> OnError;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Connect(IProtocol protocol)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(MessageBase message)
        {
            throw new NotImplementedException();
        }
    }
}
