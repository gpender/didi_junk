using DidiWebSocketTest.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DidiWebSocketTest.Models.MessagesHttp
{
    public class MenusMessage : HttpMessageBase
    {
        public MenusMessage(string ipAddress) : base()
        {
            RequestUrl = $@"http://{ipAddress}/menus.json";
        }
        public MenusMessage(byte[] responseBytes)
        {
            string jsonMenu = ASCIIEncoding.UTF8.GetString(responseBytes);
        }
    }
}
