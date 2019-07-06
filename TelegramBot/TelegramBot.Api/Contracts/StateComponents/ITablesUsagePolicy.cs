namespace TelegramBot.Api.Contracts.StateComponents
{
    public interface ITablesUsagePolicy
    {
        bool IsAvailable { get; }
    }
}
