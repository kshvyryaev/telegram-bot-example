using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Api.Contracts.CommandFactories;
using TelegramBot.Api.Contracts.Commands;

namespace TelegramBot.Api.CommandFactories
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IMessageCommandFactory _messageCommandFactory;

        public CommandFactory(IMessageCommandFactory messageCommandFactory)
        {
            _messageCommandFactory = messageCommandFactory
                ?? throw new ArgumentNullException(nameof(messageCommandFactory));
        }

        public ICommand GetCommand(Update update)
        {
            if (update == null)
            {
                return null;
            }

            switch (update.Type)
            {
                case UpdateType.Message:
                    return _messageCommandFactory.GetMessageCommand(update.Message);
                case UpdateType.EditedMessage:
                case UpdateType.InlineQuery:
                case UpdateType.ChosenInlineResult:
                case UpdateType.CallbackQuery:
                case UpdateType.ChannelPost:
                case UpdateType.EditedChannelPost:
                case UpdateType.ShippingQuery:
                case UpdateType.PreCheckoutQuery:
                case UpdateType.Poll:
                case UpdateType.Unknown:
                default:
                    return null;
            }
        }
    }
}
