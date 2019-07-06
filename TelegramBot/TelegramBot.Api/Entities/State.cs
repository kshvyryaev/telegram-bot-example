using System;
using Telegram.Bot.Types;
using TelegramBot.Api.Contracts.Commands;

namespace TelegramBot.Api.Entities
{
    public class State
    {
        public State(Update update, ICommand command)
        {
            Update = update ?? throw new ArgumentNullException(nameof(update));
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public Update Update { get; }

        public ICommand Command { get; }
    }
}
