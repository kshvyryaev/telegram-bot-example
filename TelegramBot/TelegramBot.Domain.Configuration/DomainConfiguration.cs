using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Domain.Contracts.Adapters;
using TelegramBot.Domain.Contracts.Repositories;

namespace TelegramBot.Domain.Configuration
{
    public static class DomainConfiguration
    {
        private const string AzureTableStorageConnectionStringKey = "AzureTableStorageConnectionString";

        public static void ConfigureDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICacheAdapter, Infrastructure.Caching.Memory.CacheAdapter>();

            string azureTableStorageConnectionString = configuration.GetConnectionString(AzureTableStorageConnectionStringKey);
            services.AddSingleton<ITablesRepository>(
                new Infrastructure.Data.AzureTableStorage.AzureTablesRepository(azureTableStorageConnectionString));
        }
    }
}
