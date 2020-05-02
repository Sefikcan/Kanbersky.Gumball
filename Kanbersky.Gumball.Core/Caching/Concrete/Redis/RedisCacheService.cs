using Kanbersky.Gumball.Core.Caching.Abstract;
using Kanbersky.Gumball.Core.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;

namespace Kanbersky.Gumball.Core.Caching.Concrete.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public void Add(string key, object data, double duration)
        {
            var serializedItems = data.SerializeAsJson();
            var byteItems = Encoding.UTF8.GetBytes(serializedItems);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(duration), //Cache süresi
                //AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5), // Redis’de ilgili keye ait cache’in mutlaka yani işlem yapılsın ya da yapılmasın 5sn sonunda düşeceğinin belirlenmesidir.
                //SlidingExpiration = TimeSpan.FromSeconds(10) //Redis’de ilgili key ile belli bir zaman mesela bu örnekte 5sn içinde hiçbir işlem yapılmaz ise, cache’in düşeceğinin belirlenmesidir.
            };

            _distributedCache.Set(key, byteItems, options);
        }

        public T Get<T>(string key)
        {
            var getCacheResponse = _distributedCache.Get(key);
            if (getCacheResponse != null)
            {
                var objectString = Encoding.UTF8.GetString(getCacheResponse);
                return objectString.DeserializeAs<T>();
            }

            return default;
        }

        public object Get(string key)
        {
            var getCacheResponse = _distributedCache.Get(key);
            if (getCacheResponse != null)
            {
                var objectString = Encoding.UTF8.GetString(getCacheResponse);
                return objectString.DeserializeAs<object>();
            }

            return null;
        }

        public bool IsExists(string key)
        {
            return _distributedCache.Get(key) != null ? true : false;
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
