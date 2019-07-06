using Telegram.Bot.Types;
using TelegramBot.Api.Contracts.Commands;

namespace TelegramBot.Api.Contracts.CommandFactories
{
    public interface IMessageCommandFactory
    {
        ICommand GetMessageCommand(Message message);
    }
}
