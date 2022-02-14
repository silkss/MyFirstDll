using Connectors.IB;
using Connectors.Interfaces;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPGotm.enums;

namespace TCPGotm;

internal class TcpServer
{
    private readonly TcpListener _Server;
    private readonly IConnector _Connector;
    private readonly ILogger _Logger;
    private readonly Trader _Trader;
         
    private bool working = false;

    public TcpServer(IConnector connector, ILogger logger)
    {
        _Logger = logger;
        _Connector = connector;
        _Server = new TcpListener(System.Net.IPAddress.Any, 1488);
    }

    private void _start()
    {
        working = true;
        _Server.Start();
    }

    public void Start()
    {
        _start();

        while (working)
        {
            TcpClient client = _Server.AcceptTcpClient();
            NetworkStream ns = client.GetStream();

            /* Пример отправки сообщения в сетевой поток */
            var hello = Encoding.Default.GetBytes("hello world\n");
            ns.Write(hello, 0, hello.Length);

            _Logger.AddLog(LogType.Info, "Someone connected");

            _ = Task.Run(() =>
            {
                Trader trader = new Trader(_Connector);
                while (client.Connected)
                {
                    byte[] msg = new byte[1024];
                    try
                    {
                        int count = ns.Read(msg, 0, msg.Length);
                        if (count != 0)
                        {
                            var message = Encoding.Default.GetString(msg, 0, count);
                            _Logger.AddLog(LogType.Info, message);

                            (var underlying, var price, var type) = MessageParser.Parse(message);
                            trader.Trade(underlying, price, type);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        _Logger.AddLog(LogType.Warm, $"Client disconnected {e.Message}");
                        break;
                    }
                }
            });

        }
    }
}

