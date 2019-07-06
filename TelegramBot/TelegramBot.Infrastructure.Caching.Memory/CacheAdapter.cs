using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using TelegramBot.Domain.Contracts.Adapters;

namespace TelegramBot.Infrastructure.Caching.Memory
{
    public class CacheAdapter : ICacheAdapter
    {
        private const double DefaultCacheExpirationInMinutes = 10;

        private readonly MemoryCache _memoryCache;

        public CacheAdapter()
        {
            _memoryCache = MemoryCache.Default;
        }

        public void Set<TValue>(string key, TValue value, TimeSpan? cacheExpiration = null)
            where TValue : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            DateTime absoluteExpiration = cacheExpiration.HasValue
                ? DateTime.Now.AddMilliseconds(cacheExpiration.Value.TotalMilliseconds)
                : DateTime.Now.AddMinutes(DefaultCacheExpirationInMinutes);

            _memoryCache.Set(key, value, absoluteExpiration);
        }

        public TValue Get<TValue>(string key)
            where TValue : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _memoryCache.Get(key) as TValue;
        }

        public void Delete(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_memoryCache.Contains(key))
            {
                _memoryCache.Remove(key);
            }
        }

        public void Clear()
        {
            List<string> cacheKeys = _memoryCache.Select(x => x.Key).ToList();

            foreach (var cacheKey in cacheKeys)
            {
                Delete(cacheKey);
            }
        }
    }
}
