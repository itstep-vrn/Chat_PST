using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public delegate void Message(string message);
    public class TCPServer
    {
        public event Message Info;
        
        private readonly IPEndPoint ip;
        private readonly Socket listenSocket;

        public TCPServer()
        {
            ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 750);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public TCPServer(string ip_address, int port)
        {
            ip = new IPEndPoint(IPAddress.Parse(ip_address), port);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            listenSocket.Bind(ip);
            listenSocket.Listen(10);
            Info?.Invoke("Сервер запущен. Ожидает подключения...");
        }

        public Socket GetSocket()
        {
            return listenSocket;
        }
        
        public void CloseListenSocket()
        {
            listenSocket.Shutdown(SocketShutdown.Both);
            listenSocket.Close();
        }
    }

    public class TCPClientSocket
    {
        public event Message Info;
        private readonly Socket client;

        private int bytes = 0;
        private byte[] data = new byte[256];
        private StringBuilder tempMessage = new StringBuilder();

        public TCPClientSocket(Socket connect)
        {
            client = connect.Accept();
        }
        
        public string GetMessage()
        {
            tempMessage.Clear();
            do
            {
                bytes = client.Receive(data);
                tempMessage.Append(Encoding.Unicode.GetString(data), 0, bytes);
            } while (client.Available > 0);
            
            Info?.Invoke(DateTime.Now.ToShortTimeString() + ":" + tempMessage);
            
            return tempMessage.ToString();
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data);
            Info?.Invoke("Сообщение отправлено");
        }
        
        public void CloseSocketClient()
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}