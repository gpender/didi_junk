using DidiWebSocketTest.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DidiWebSocketTest.Models.MessagesHttp
{
    public class ParameterAttributesMessage : HttpMessageBase
    {
        public ParameterAttributesMessage(string ipAddress, int tag) : base()
        {
            RequestUrl = $@"http://{ipAddress}/menus.json";
        }
        public ParameterAttributesMessage(byte[] responseBytes)
        {
            string jsonMenu = ASCIIEncoding.UTF8.GetString(responseBytes);
        }
    }
}
