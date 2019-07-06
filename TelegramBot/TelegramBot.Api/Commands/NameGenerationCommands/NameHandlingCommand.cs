using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Api.Contracts.Commands.NameGenerationCommands;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Api.Extensions;

namespace TelegramBot.Api.Commands.NameGenerationCommands
{
    public class NameHandlingCommand : INameHandlingCommand
    {
        private readonly IStateMachineManager _stateMachineManager;

        public NameHandlingCommand(IStateMachineManager stateMachineManager)
        {
            _stateMachineManager = stateMachineManager
                ?? throw new ArgumentNullException(nameof(stateMachineManager));
        }

        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            string uniqueKey = update.GetSenderUniqueKey();

            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                $"Your name is {update.Message.Text}",
                parseMode: ParseMode.Markdown);

            await _stateMachineManager.DeleteStateMachine(uniqueKey);
        }
    }
}
