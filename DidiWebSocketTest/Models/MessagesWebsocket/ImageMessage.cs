namespace DidiWebSocketTest.Models.Messages
{
    public class ImageMessage : MessageBase
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
