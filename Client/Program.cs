using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Lib;

using static System.Console;

namespace Client
{
    internal static class Program
    {
        private const string ipaddress = "127.0.0.1";
        private const int port = 8888;

        private static void Main()
        {
            TcpClient client = null;
            var options = new JsonSerializerOptions { WriteIndented = false};

            try
            {
                client = new TcpClient(ipaddress, port);
                var stream = client.GetStream();
                
                while (true)
                {
                    Write("Введите сообщение: ");
                    var messageOut = ReadLine();
                    var msgOut = new Msg {Type = "TEXT", Message = messageOut};
                    var message = JsonSerializer.Serialize(msgOut, options);
                    
                    WriteLine(message);

                    var bufferOut = Encoding.Unicode.GetBytes(message);
                    stream.Write(bufferOut, 0, bufferOut.Length);

                    message.Remove(0);
                    
                    var messageIn = Message.Get(stream);
                    var msgIn = JsonSerializer.Deserialize<Msg>(messageIn.ToString(), options);
                    WriteLine($"Ответ сервера - {msgIn.Type} : {msgIn.Message}");
                    messageIn.Clear();
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
