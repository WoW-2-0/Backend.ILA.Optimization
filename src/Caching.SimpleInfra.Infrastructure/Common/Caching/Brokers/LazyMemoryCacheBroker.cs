using Caching.SimpleInfra.Domain.Common.Caching;
using Caching.SimpleInfra.Persistence.Caching.Brokers;
using Force.DeepCloner;
using LazyCache;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Caching.Brokers;

public class LazyMemoryCacheBroker(IAppCache appCache, IOptions<CacheSettings> cacheSettings) : ICacheBroker
{
    private readonly MemoryCacheEntryOptions _entryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSettings.Value.AbsoluteExpirationInSeconds),
        SlidingExpiration = TimeSpan.FromSeconds(cacheSettings.Value.SlidingExpirationInSeconds)
    };

    public async ValueTask<T> GetAsync<T>(string key) => await appCache.GetAsync<T>(key);

    public ValueTask<bool> TryGetAsync<T>(string key, out T value)
    {
        return new ValueTask<bool>(appCache.TryGetValue(key, out value));
    }

    public ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default)
    {
        var currentEntryOptions = _entryOptions;

        if (entryOptions != default)
        {
            currentEntryOptions = _entryOptions.DeepClone();

            currentEntryOptions.AbsoluteExpirationRelativeToNow =
                entryOptions.AbsoluteExpirationRelativeToNow ?? currentEntryOptions.AbsoluteExpirationRelativeToNow;
            currentEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration ?? currentEntryOptions.SlidingExpiration;
        }

        appCache.Add(key, value, currentEntryOptions);

        return ValueTask.CompletedTask;
    }

    public async ValueTask<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CacheEntryOptions? entryOptions = default)
    {
        var currentEntryOptions = _entryOptions;

        if (entryOptions != default)
        {
            currentEntryOptions = _entryOptions.DeepClone();

            currentEntryOptions.AbsoluteExpirationRelativeToNow =
                entryOptions.AbsoluteExpirationRelativeToNow ?? currentEntryOptions.AbsoluteExpirationRelativeToNow;
            currentEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration ?? currentEntryOptions.SlidingExpiration;
        }

        return await appCache.GetOrAddAsync(key, valueFactory, currentEntryOptions);
    }

    public ValueTask DeleteAsync(string key)
    {
        appCache.Remove(key);

        return ValueTask.CompletedTask;
    }
}