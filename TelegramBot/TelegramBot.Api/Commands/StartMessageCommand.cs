using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Api.Contracts.Commands;

namespace TelegramBot.Api.Commands
{
    public class StartMessageCommand : IStartMessageCommand
    {
        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Hello I am ASP.NET Core Bot.",
                parseMode: ParseMode.Markdown,
                replyMarkup: new ReplyKeyboardMarkup
                {
                    Keyboard = new List<List<KeyboardButton>>
                    {
                        new List<KeyboardButton>
                        {
                            new KeyboardButton("Click"),
                            new KeyboardButton("Test")
                        }
                    },
                    ResizeKeyboard = true
                });
        }
    }
}
