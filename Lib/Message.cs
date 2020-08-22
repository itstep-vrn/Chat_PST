using System.Net.Sockets;
using System.Text;

namespace Lib
{
    public static class Message
    {
        public static StringBuilder Get(NetworkStream stream)
        {
            var bufferOut = new byte[256];
            var message = new StringBuilder();
            // Получение сообщения от клиента
            do
            {
                var bytes = stream.Read(bufferOut, 0, bufferOut.Length);
                message.Append(Encoding.Unicode.GetString(bufferOut, 0, bytes));
            } while (stream.DataAvailable);

            return message;
        }
    }
}