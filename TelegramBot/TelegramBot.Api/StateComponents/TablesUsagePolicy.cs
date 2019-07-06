using TelegramBot.Api.Contracts.StateComponents;

namespace TelegramBot.Api.StateComponents
{
    public class TablesUsagePolicy : ITablesUsagePolicy
    {
        public TablesUsagePolicy(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }

        public bool IsAvailable { get; }
    }
}
