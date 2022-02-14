using System;
using TCPGotm.enums;

namespace TCPGotm;

internal static class MessageParser
{
    /// <summary>
    /// Returns tuple where 1st element is underlying symbol
    /// 2nd element current price of underlying symbol
    /// 3rd signal type <see cref="SignalType"/>
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static (string, decimal, SignalType) Parse(string message)
    {
        var messages = message.Split(';');

        return (messages[0], 
            decimal.Parse(messages[1].Replace('.', ',')), 
            parsesignaltype(messages[2]));
    }

    private static SignalType parsesignaltype(string msg) => msg switch
    {
        "OPEN" => SignalType.OPEN,
        "CLOSE" => SignalType.CLOSE,
        "INIT" => SignalType.INIT,
        _ => throw new ArgumentException("Wrong message: ", nameof(msg))
    };
}
