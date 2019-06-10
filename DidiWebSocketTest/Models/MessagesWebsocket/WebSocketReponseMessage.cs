using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DidiWebSocketTest.Models.Messages
{
    public class MessageBroker
    {
        // Response. Receive the response byte[] frame and instantiate the correct message type
        // Request. Select a message and populate it.
        public void GetMessage(byte[] frame)
        {

        }
    }

    public class StartScopeRequestMessage : MessageBase
    {
        public StartScopeRequestMessage(byte[] frame)
        {
            header[2] = 0x04; // 
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ResponseDataStructure
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public String message;
        }
    }
}
