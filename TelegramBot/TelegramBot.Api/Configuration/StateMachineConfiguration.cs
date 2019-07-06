using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Api.StateComponents;

namespace TelegramBot.Api.Configuration
{
    public static class StateComponentsConfiguration
    {
        private const string IsTableStorageEnable = "IsTableStorageEnable";

        public static void ConfigureStateComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IStateMachineManager, StateMachineManager>();

            var isTableStorageEnable = configuration.GetValue<bool>(IsTableStorageEnable);
            services.AddSingleton<ITablesUsagePolicy>(new TablesUsagePolicy(isTableStorageEnable));
        }
    }
}
