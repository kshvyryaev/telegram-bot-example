using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Api.Contracts.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync(ITelegramBotClient client, Update update);
    }
}
