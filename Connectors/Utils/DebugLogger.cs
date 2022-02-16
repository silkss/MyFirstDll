using Connectors.Interfaces;
using System.Diagnostics;

namespace Connectors.Utils;

internal class DebugLogger : ILogger
{
    public void AddLog(LogType type, string msg)
    {
        Debug.WriteLine($"{DateTime.Now}\t{type}\n{msg}");
    }
}
