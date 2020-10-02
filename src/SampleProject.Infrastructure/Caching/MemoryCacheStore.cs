namespace SampleProject.Infrastructure.Caching
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Caching.Memory;

    public class MemoryCacheStore : ICacheStore
    {
        private readonly Dictionary<string, TimeSpan> _expirationConfiguration;
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheStore(
            IMemoryCache memoryCache,
            Dictionary<string, TimeSpan> expirationConfiguration)
        {
            _memoryCache = memoryCache;
            _expirationConfiguration = expirationConfiguration;
        }

        public void Add<TItem>(TItem item, ICacheKey<TItem> key, TimeSpan? expirationTime = null)
        {
            string cachedObjectName = item.GetType().Name;
            TimeSpan timespan;
            if (expirationTime.HasValue)
            {
                timespan = expirationTime.Value;
            }
            else
            {
                timespan = _expirationConfiguration[cachedObjectName];
            }

            _memoryCache.Set(key.CacheKey, item, timespan);
        }

        public void Add<TItem>(TItem item, ICacheKey<TItem> key, DateTime? absoluteExpiration = null)
        {
            DateTimeOffset offset;
            if (absoluteExpiration.HasValue)
            {
                offset = absoluteExpiration.Value;
            }
            else
            {
                offset = DateTimeOffset.MaxValue;
            }

            _memoryCache.Set(key.CacheKey, item, offset);
        }

        public TItem Get<TItem>(ICacheKey<TItem> key) where TItem : class
        {
            if (_memoryCache.TryGetValue(key.CacheKey, out TItem value))
            {
                return value;
            }

            return null;
        }

        public void Remove<TItem>(ICacheKey<TItem> key)
        {
            _memoryCache.Remove(key.CacheKey);
        }
    }
}