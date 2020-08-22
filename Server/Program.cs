using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Server
{
    class ClientObj
    {
        private TcpClient client;

        public ClientObj(TcpClient client)
        {
            this.client = client;
        }

        public void Process()
        {
            NetworkStream stream = null;

            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[64];
                StringBuilder message = new StringBuilder();
                int bytes;

                while (true)
                {
                    bytes = 0;
                    // Получение сообщения от клиента
                    do
                    {
                        bytes = stream.Read(buffer, 0, buffer.Length);
                        message.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                    } while (stream.DataAvailable);

                    WriteLine(message.ToString());

                    // Отправка сообщения клиенту
                    buffer = Encoding.Unicode.GetBytes("Сообщение получено");
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                WriteLine(e);
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }
    }

    class Program
    {
        private static IPAddress ip_address = IPAddress.Parse("127.0.0.1");
        private static int port = 8888;
        private static TcpListener server;
        
        static void Main()
        {
            try
            {
                server = new TcpListener(ip_address, port);
                server.Start();

                WriteLine("Ожидаем подключения");

                while (true)
                {
                    TcpClient tcpClient = server.AcceptTcpClient();
                    ClientObj client = new ClientObj(tcpClient);

                    Task clientThread = new Task(client.Process);
                    clientThread.Start();
                }
                
            }
            catch (Exception e)
            {
                WriteLine(e);
            }
            finally
            {
                server?.Stop();
            }
        }
    }
}
