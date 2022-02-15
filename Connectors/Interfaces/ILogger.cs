namespace Connectors.Interfaces;

public interface ILogger
{
    void AddLog(LogType type, string msg);
}

public enum LogType
{
    Warm,
    Info,
    Crit,
    Error,
    Debug
}