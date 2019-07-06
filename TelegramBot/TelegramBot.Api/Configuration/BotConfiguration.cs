using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramBot.Api.Helpers;

namespace TelegramBot.Api.Configuration
{
    public static class BotConfiguration
    {
        private const string BotSectionKey = "Bot";

        public static void ConfigureBot(this IServiceCollection services, IConfiguration configuration)
        {
            var botConfiguration = new BotConfigurationModel();
            configuration.GetSection(BotSectionKey).Bind(botConfiguration);

            var client = new TelegramBotClient(botConfiguration.Token);
            var fullSecureUrl = SecurePathManager.GetFullSecureUrl(botConfiguration.BaseUrl);
            client.SetWebhookAsync(fullSecureUrl).Wait();

            services.AddSingleton<ITelegramBotClient>(client);
        }
    }
}
