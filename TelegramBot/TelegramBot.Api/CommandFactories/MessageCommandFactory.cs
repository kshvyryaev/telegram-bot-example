using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using TelegramBot.Api.Contracts.CommandFactories;
using TelegramBot.Api.Contracts.Commands;
using TelegramBot.Api.Contracts.Commands.NameGenerationCommands;

namespace TelegramBot.Api.CommandFactories
{
    public class MessageCommandFactory : IMessageCommandFactory
    {
        #region Constants

        private const string StartMessageCommand = "/start";
        private const string StartNameGenerationCommand = "/start_name_generation";

        #endregion Constants

        #region Fields

        private readonly Dictionary<string, ICommand> _commands;
        private readonly ICommand _defaultCommand;

        #endregion Fields

        #region Constructors

        public MessageCommandFactory(
            IStartMessageCommand startMessageCommand,
            IRepeatMessageCommand repeatMessageCommand,
            IStartNameGenerationCommand startNameGenerationCommand)
        {
            if (startMessageCommand == null)
            {
                throw new ArgumentNullException(nameof(startMessageCommand));
            }

            if (repeatMessageCommand == null)
            {
                throw new ArgumentNullException(nameof(repeatMessageCommand));
            }

            if (startNameGenerationCommand == null)
            {
                throw new ArgumentNullException(nameof(startNameGenerationCommand));
            }

            _commands = new Dictionary<string, ICommand>
            {
                { StartMessageCommand, startMessageCommand },
                { StartNameGenerationCommand, startNameGenerationCommand }
            };

            _defaultCommand = repeatMessageCommand;
        }

        #endregion Constructors

        #region Methods

        public ICommand GetMessageCommand(Message message)
        {
            if (message == null)
            {
                return null;
            }

            if (_commands.TryGetValue(message.Text, out ICommand command))
            {
                return command;
            }

            return _defaultCommand;
        }

        #endregion Methods
    }
}
