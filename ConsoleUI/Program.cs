
namespace ConsoleUI
{ 
    public class Program
    {
        private static TelegramImpl client = new TelegramImpl(new Services.ReceiversRepository());
        public static async Task Main(string[] args)
        {
            await client.BotUser();   
            if (client.Me != null)
            {
                Console.WriteLine(client.Me.Id);
                if (client.StartRecieve())
                {
                    while ( await commands(Console.ReadLine())) { };
                }
            }
            else
            {
                Console.WriteLine("Something goes wrong");
            }
        }

        private static async Task<bool> commands(string? command) => command switch
        {
            "disconnect" => false,
            "message" => await sendmessage(),
            _ => true
        };

        private static async Task<bool> sendmessage()
        {
            Console.WriteLine("Enter message");
            var message = Console.ReadLine();
            if (message != null)
            {
                await client.SendMessage(message);
            }
            return true;
        }
    }
}