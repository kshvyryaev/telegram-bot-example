using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Api.Contracts.CommandFactories;
using TelegramBot.Api.CommandFactories;

namespace TelegramBot.Api.Configuration
{
    public static class FactoriesConfiguration
    {
        public static void ConfigureFactories(this IServiceCollection services)
        {
            services.AddSingleton<ICommandFactory, CommandFactory>();
            services.AddSingleton<IMessageCommandFactory, MessageCommandFactory>();
        }
    }
}
