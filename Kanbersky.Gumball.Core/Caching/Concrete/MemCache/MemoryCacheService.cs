using Kanbersky.Gumball.Core.Caching.Abstract;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Kanbersky.Gumball.Core.Caching.Concrete.MemCache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add(string key, object data, double duration)
        {
            _memoryCache.Set(key, data, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            var getCacheResponse = _memoryCache.Get(key);
            if (getCacheResponse != null)
            {
                return (T)getCacheResponse;
            }

            return default;
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsExists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
