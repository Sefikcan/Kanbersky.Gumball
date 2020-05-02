namespace Kanbersky.Gumball.Core.Caching.Abstract
{
    public interface ICacheService
    {
        T Get<T>(string key);

        object Get(string key);

        void Add(string key, object data, double duration);

        bool IsExists(string key);

        void Remove(string key);
    }
}
