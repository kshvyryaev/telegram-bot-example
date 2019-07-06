using System.Threading.Tasks;

namespace TelegramBot.Domain.Contracts.Repositories
{
    public interface ITablesRepository
    {
        Task SetAsync<TValue>(string tableName, string key, TValue value)
            where TValue : class;

        Task<TValue> GetAsync<TValue>(string tableName, string key)
            where TValue : class;

        Task DeleteAsync<TValue>(string tableName, string key)
            where TValue : class;
    }
}
