using DidiWebSocketTest.Interfaces;
using System.Linq;
using System.Text;

namespace DidiWebSocketTest.Models.Messages
{
    public abstract class HttpMessageBase : MessageBase, IHttpMessage
    {
        public HttpMessageBase() : base()
        {}

        public string RequestUrl { get; protected set; }

        public string Response { get; protected set; }

        public void SetResponseBytes(byte[] response)
        {
            Response = Encoding.ASCII.GetString(response);
        }
    }
}
