using System;

namespace TelegramBot.Domain.Contracts.Adapters
{
    public interface ICacheAdapter
    {
        void Set<TValue>(string key, TValue value, TimeSpan? cacheExpiration = null)
            where TValue : class;

        TValue Get<TValue>(string key)
            where TValue : class;

        void Delete(string key);

        void Clear();
    }
}
