using System.Linq;

namespace DidiWebSocketTest.Models.Messages
{
    // Header bytes
    // 0 - Data Offset (header size: normally = 4)
    // 1 - Version (=1)
    // 2 - Message Type
    // 3 - Reserved

    // Response Message Types
    // 0 - GET SCOPE PARAMS
    // 1 - GET CONFIG PARAMS
    // 2 - SEND FULL BUFFER
    // 3 - SEND FULL BUFFER
    public abstract class WebSocketMessageBase : MessageBase
    {
        // Header bytes
        // 0 - Data Offset (header size: normally = 4)
        // 1 - Version (=1)
        // 2 - Message Type
        // 3 - Reserved
        const int headerLength = 0x04;
        const int version = 0x01;
        protected byte[] header = new byte[4] { headerLength, version, 0x00, 0x00 };
        public string Message { get; protected set; }
        public MessageType MessageType
        {
            get { return (MessageType)header[2]; }
        }
        public WebSocketMessageBase() : base()
        {
        }
        public WebSocketMessageBase(byte[] frame) : base()
        {
            header = frame.Take(headerLength).ToArray();
        }
    }
}
