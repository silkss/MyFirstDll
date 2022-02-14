using Connectors.Interfaces;
using System;

namespace TCPGotm;

internal class ConsoleLogger : ILogger
{
    public void AddLog(LogType type, string msg)
    {
        Console.WriteLine($"[{DateTime.Now}]\t{type}. {msg}");
    }
}

