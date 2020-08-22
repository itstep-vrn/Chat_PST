using System;
using System.Net.Sockets;
using System.Text;

using static System.Console;

namespace Client
{
    class Program
    {
        private static string ipaddress = "127.0.0.1";
        private static int port = 8888;

        static void Main()
        {
            TcpClient client = null;

            try
            {
                client = new TcpClient(ipaddress, port);
                NetworkStream stream = client.GetStream();
                string messageOut;
                byte[] bufferOut;
                byte[] bufferIn = new byte[64];
                int bytes;
                StringBuilder messageIn = new StringBuilder();

                while (true)
                {
                    Write("Введите сообщение: ");
                    messageOut = ReadLine();

                    bufferOut = Encoding.Unicode.GetBytes(messageOut);
                    stream.Write(bufferOut, 0, bufferOut.Length);

                    bytes = 0;
                    do
                    {
                        bytes = stream.Read(bufferIn, 0, bufferIn.Length);
                        messageIn.Append(Encoding.Unicode.GetString(bufferIn, 0, bytes));
                    } while (stream.DataAvailable);
                    WriteLine($"Ответ сервера - {messageIn}");
                }
            }
            catch (Exception e)
            {
                WriteLine(e);
            }
            finally
            {
                client?.Close();
            }
        }
    }
}
