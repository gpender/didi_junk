using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace DidiWebSocketTest.Models
{
    public class HttpTransport : ITransport
    {
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
        public void SendRequestResponseMessage(IMessage requestMessage)
        {
            IHttpMessage httpMessage = requestMessage as IHttpMessage;
            if(httpMessage != null)
            {
                try
                {
                    HttpWebRequest request = CreateGetRequest(new Uri(httpMessage.RequestUrl));
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream dataStream = response.GetResponseStream();
                        BinaryReader breader = new BinaryReader(dataStream);
                        httpMessage.SetResponseBytes(ReadAllBytes(breader));
                    }
                }
                catch(Exception e)
                {
                    OnError(this, e.Message);
                }
            }
        }

        public void SendMessage(IMessage message)
        {
            throw new NotImplementedException();
        }

        HttpWebRequest CreateGetRequest(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "GET";
            //request.KeepAlive = false;
            request.Timeout = 2000;
            request.Proxy = null;
            return request;
        }
        public byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }
    }
}
