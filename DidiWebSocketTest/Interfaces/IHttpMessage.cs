namespace DidiWebSocketTest.Interfaces
{
    public interface IHttpMessage : IMessage
    {
        string RequestUrl { get; }
        string Response { get; }
        void SetResponseBytes(byte[] response);
    }
}
