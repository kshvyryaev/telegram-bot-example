using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Api.Contracts.Commands.NameGenerationCommands;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Api.Entities;
using TelegramBot.Api.Extensions;
using TelegramBot.Api.StateComponents;

namespace TelegramBot.Api.Commands.NameGenerationCommands
{
    public class StartNameGenerationCommand : IStartNameGenerationCommand
    {
        private readonly IStateMachineManager _stateMachineManager;
        private readonly INameHandlingCommand _nameHandlingCommand;

        public StartNameGenerationCommand(
            IStateMachineManager stateMachineManager,
            INameHandlingCommand nameHandlingCommand)
        {
            _stateMachineManager = stateMachineManager
                ?? throw new ArgumentNullException(nameof(stateMachineManager));

            _nameHandlingCommand = nameHandlingCommand
                ?? throw new ArgumentNullException(nameof(nameHandlingCommand));
        }

        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            string uniqueKey = update.GetSenderUniqueKey();
            IStateMachine stateMachine = new StateMachine();
            stateMachine.AddState(new State(update, this));
            stateMachine.ActiveCommand = _nameHandlingCommand;

            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Please, enter your name:",
                parseMode: ParseMode.Markdown);

            await _stateMachineManager.SetStateMachine(uniqueKey, stateMachine);
        }
    }
}
