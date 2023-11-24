using Caching.SimpleInfra.Persistence.Caching;
using LazyCache;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Caching;

public class LazyMemoryCacheBroker(IAppCache appCache, IOptions<CacheSettings> cacheSettings) : ICacheBroker
{
    private readonly CacheSettings _cacheSettings = cacheSettings.Value;

    public async ValueTask<T> GetAsync<T>(string key) => await appCache.GetAsync<T>(key);

    public ValueTask SetAsync<T>(string key, T value)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheSettings.AbsoluteExpirationInMinutes),
            SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes)
        };

        appCache.Add(key, value, options);

        return ValueTask.CompletedTask;
    }

    public async ValueTask<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheSettings.AbsoluteExpirationInMinutes),
            SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes)
        };

        return await appCache.GetOrAddAsync(key, valueFactory, options);
    }
    
    public ValueTask DeleteAsync(string key)
    {
        appCache.Remove(key);

        return ValueTask.CompletedTask;
    }
}