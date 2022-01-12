using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CsTelegramBot
{
    public class TelegramImpl
    {

        public const string TOKEN = "2031108528:AAE9qpNIRCbpF23IufRJTO-0D2h7IyIf0gg";
        public User? Me { get; set; }

        private CancellationTokenSource cts = new CancellationTokenSource();

        private TelegramBotClient client = new TelegramBotClient(TOKEN);

        public async Task<User> BotUser()
        {
            if (Me == null)
            {
                Me = await client.GetMeAsync();
                return Me;
            }
            return Me;
        }

        public bool StartRecieve()
        {
            if (Me == null) return false;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };

            client.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);
            return true;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;
            
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;


            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            string answer;

            if (messageText == "/start")
            {
                answer = "Today this service is free, but in future..";
            }
            else
            {
                answer = messageText == null ? "in some reason no message here" : messageText;
            }

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: answer,
                cancellationToken: cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}