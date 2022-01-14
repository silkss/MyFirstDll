namespace MyTCPServer
{
    internal class Commands
    {
        public static bool ProcessCommand(string? command) => command switch
        {
            null => true,
            "quit" => false,
            _ => true
        };
    }
}
