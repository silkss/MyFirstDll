using System;
using System.Net.Sockets;
using System.Text;

namespace MyTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(System.Net.IPAddress.Any, 1234);

            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream ns = client.GetStream();
                
                var hello = Encoding.Default.GetBytes("hello world\n");

                ns.Write(hello, 0, hello.Length);
                while (client.Connected)
                {
                    byte[] msg = new byte[1024];
                    try
                    {
                        int count = ns.Read(msg, 0, msg.Length);
                        if (count != 0)
                        {
                            Console.WriteLine(Encoding.Default.GetString(msg, 0, count));
                            Console.WriteLine(new String('-', 10));
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine($"Client disconnected {e.Message}");
                        break;
                    }
                }
            }
        }
    }
}
