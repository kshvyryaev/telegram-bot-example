using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Api.Contracts.Commands;

namespace TelegramBot.Api.Commands
{
    public class RepeatMessageCommand : IRepeatMessageCommand
    {
        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                update.Message.Text,
                parseMode: ParseMode.Markdown,
                replyToMessageId: update.Message.MessageId);
        }
    }
}
