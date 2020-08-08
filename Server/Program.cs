using System.Net.Sockets;
using System.Threading.Tasks;
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
                Task task = new Task( () => Client(server) );
                task.Start();
            }
            //server.CloseListenSocket();
        }

        static void Client(TCPServer server)
        {
            Socket listenSocket = server.GetSocket();
            TCPClientSocket clientSocket = new TCPClientSocket(listenSocket);
            clientSocket.Info += WriteLine;
            
            while (true)
            {
                string message = clientSocket.GetMessage();

                clientSocket.SendMessage("Ваше сообщение получено");

                if (message == "стоп")
                {
                    clientSocket.SendMessage("Соединение разорвано");
                    break;
                }
            }
            clientSocket.CloseSocketClient();
        }
    }
}