using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Api.Contracts.Commands;
using TelegramBot.Api.Commands;
using TelegramBot.Api.Commands.NameGenerationCommands;
using TelegramBot.Api.Contracts.Commands.NameGenerationCommands;

namespace TelegramBot.Api.Configuration
{
    public static class CommandsConfiguration
    {
        public static void ConfigureCommands(this IServiceCollection services)
        {
            services.AddSingleton<IStartMessageCommand, StartMessageCommand>();
            services.AddSingleton<IRepeatMessageCommand, RepeatMessageCommand>();

            // Name generation commands
            services.AddSingleton<IStartNameGenerationCommand, StartNameGenerationCommand>();
            services.AddSingleton<INameHandlingCommand, NameHandlingCommand>();
        }
    }
}
