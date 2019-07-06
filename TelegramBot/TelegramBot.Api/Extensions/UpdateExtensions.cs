using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Api.Extensions
{
    public static class UpdateExtensions
    {
        public static string GetSenderUniqueKey(this Update update)
        {
            if (update == null)
            {
                return null;
            }

            switch (update.Type)
            {
                case UpdateType.Message:
                    return update.Message.From.Username;
                case UpdateType.InlineQuery:
                    return update.InlineQuery.From.Username;
                case UpdateType.ChosenInlineResult:
                    return update.ChosenInlineResult.From.Username;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.From.Username;
                case UpdateType.EditedMessage:
                    return update.EditedMessage.From.Username;
                case UpdateType.ChannelPost:
                    return update.ChannelPost.From.Username;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost.From.Username;
                case UpdateType.ShippingQuery:
                    return update.ShippingQuery.From.Username;
                case UpdateType.PreCheckoutQuery:
                    return update.PreCheckoutQuery.From.Username;
                case UpdateType.Poll:
                case UpdateType.Unknown:
                default:
                    return null;
            }
        }
    }
}
