using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Lib;

using static System.Console;

namespace Server
{
    public class ClientObj
    {
        private readonly TcpClient client;

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
                var options = new JsonSerializerOptions {WriteIndented = false};

                while (true)
                {
                    var message = Message.Get(stream);

                    WriteLine($"Сообщение от клиента - {message}");

                    var msgIn = JsonSerializer.Deserialize<Msg>(message.ToString(), options);
                    WriteLine($"{msgIn.Type} : {msgIn.Message}");

                    message.Clear();

                    var bufferIn = new byte[256];
                    // Отправка сообщения клиенту
                    var msgOut = new Msg {Type = "RESPOND", Message = "Сообщение получено"};
                    var messageOut = JsonSerializer.Serialize(msgOut, options);
                    WriteLine(messageOut);
                    bufferIn = Encoding.Unicode.GetBytes(messageOut);
                    stream.Write(bufferIn, 0, bufferIn.Length);
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
}