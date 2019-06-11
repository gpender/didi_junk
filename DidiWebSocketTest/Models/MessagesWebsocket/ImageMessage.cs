namespace DidiWebSocketTest.Models.Messages
{
    public class ImageMessage : WebSocketMessageBase
    {
        public ImageMessage()
        {
        }
        public ImageMessage(byte[] frame)
        {
            MessageBytes = frame;
        }
    }
}
