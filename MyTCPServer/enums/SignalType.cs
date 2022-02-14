namespace TCPGotm.enums;
/// <summary>
/// INIT -- prepe all to send orders
/// OPEN -- open new Straddle or close old straddle and Open new. 
/// CLOSE -- close current straddle.
/// </summary>
internal enum SignalType
{
    INIT,
    OPEN,
    CLOSE
}
