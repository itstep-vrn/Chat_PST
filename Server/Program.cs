using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using static System.Console;

namespace Server
{
    internal static class Program
    {
        private static readonly IPAddress ip_address = IPAddress.Parse("127.0.0.1");
        private const int port = 8888;
        private static TcpListener server;

        private static void Main()
        {
            try
            {
                server = new TcpListener(ip_address, port);
                server.Start();

                WriteLine("Ожидаем подключения");

                while (true)
                {
                    var tcpClient = server.AcceptTcpClient();
                    var client = new ClientObj(tcpClient);

                    var clientThread = new Task(client.Process);
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