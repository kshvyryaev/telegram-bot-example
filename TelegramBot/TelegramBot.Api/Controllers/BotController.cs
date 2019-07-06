using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Api.Contracts.CommandFactories;
using TelegramBot.Api.Contracts.Commands;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Api.Extensions;

namespace TelegramBot.Api.Controllers
{
    public class BotController : Controller
    {
        private readonly ITelegramBotClient _client;
        private readonly IStateMachineManager _stateMachineFactory;
        private readonly ICommandFactory _commandFactory;

        public BotController(
            ITelegramBotClient client,
            IStateMachineManager stateMachineFactory,
            ICommandFactory commandFactory)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _stateMachineFactory = stateMachineFactory ?? throw new ArgumentNullException(nameof(stateMachineFactory));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        [HttpPost]
        public async Task<IActionResult> Execute([FromBody]Update update)
        {
            if (update == null)
            {
                return Ok();
            }

            string senderUniqueKey = update.GetSenderUniqueKey();
            IStateMachine stateMachine = await _stateMachineFactory.GetStateMachine(senderUniqueKey);

            if (stateMachine != null)
            {
                await stateMachine.ActiveCommand?.ExecuteAsync(_client, update);
            }
            else
            {
                ICommand command = _commandFactory.GetCommand(update);
                await command?.ExecuteAsync(_client, update);
            }

            return Ok();
        }
    }
}