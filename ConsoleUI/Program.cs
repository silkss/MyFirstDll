using CsTelegramBot;

namespace ConsoleUI
{ 
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new TelegramImpl();
            await client.BotUser();   
            if (client.Me != null)
            {
                Console.WriteLine(client.Me.Id);
                if (client.StartRecieve())
                {
                    while (true) { };
                }
            }
            else
            {
                Console.WriteLine("Something goes wrong");
            }
        }
    }
}