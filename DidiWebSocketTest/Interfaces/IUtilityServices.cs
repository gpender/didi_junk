using System.Windows.Media.Imaging;

namespace DidiWebSocketTest.Interfaces
{
    public interface IUtilityServices
    {
        BitmapImage ByteArrayToImage(byte[] byteArrayIn);
        string ByteArrayToString(byte[] ba);
    }
}
