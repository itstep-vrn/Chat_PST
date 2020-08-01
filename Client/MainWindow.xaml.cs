using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 750);
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        public MainWindow()
        {
            InitializeComponent();
            socket.Connect(ip);
        }

        private void Send_OnClick(object sender, RoutedEventArgs e)
        {
            string message = Message.Text;
            byte[] sendData = Encoding.Unicode.GetBytes(message);
            socket.Send(sendData);
            
            byte[] data = new byte[256];
            StringBuilder respond = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = socket.Receive(data);
                respond.Append(Encoding.Unicode.GetString(data), 0, bytes);
            } while (socket.Available > 0);

            Status.Text = String.Empty;
            Status.Text = respond.ToString();
        }
    }
}