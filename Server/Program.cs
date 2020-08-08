using System.Net.Sockets;
using System.Threading.Tasks;
using ChatServer;
using AutoPackage;

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
            
            Task task = new Task( () => Client(server) );
            task.Start();
            
            server.CloseListenSocket();
        }

        static void Client(TCPServer server)
        {
            Socket listenSocket = server.GetSocket();
            TCPClientSocket clientSocket = new TCPClientSocket(listenSocket);
            clientSocket.Info += WriteLine;
            
            while (true)
            {
                string message = clientSocket.GetMessage();

                string temp = Package.Pack("0", "SERVICE", "Ваше сообщение получено");
                clientSocket.SendMessage(temp);

                var msg = Package.Unpack(message);

                if (msg.Item2 == "STOP")
                {
                    temp = Package.Pack("0", "SERVICE", "Соединение разорвано");
                    clientSocket.SendMessage(temp);
                    break;
                }
            }
            clientSocket.CloseSocketClient();
        }
    }
}