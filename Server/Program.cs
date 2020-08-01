using ChatServer;

using static System.Console;

namespace Server
{
    class Program
    {
        static void Main()
        {
            TCPServer server = new TCPServer();
            server.Info += WriteLine;

            server.Start();

            while (true)
            {
                server.ClientConnecting();
                
                while (true)
                {
                    string message = server.GetMessage();

                    server.SendMessage("Ваше сообщение получено");

                    if (message == "стоп")
                    {
                        server.SendMessage("Соединение разорвано");
                        break;
                    }
                }
                server.CloseSocketClient();
                break;
            }
            server.CloseListenSocket();
        }
    }
}