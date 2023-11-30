using System.Text;
using Caching.SimpleInfra.Domain.Common.Caching;
using Caching.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Caching.Brokers;

public class RedisDistributedCacheBroker(IOptions<CacheSettings> cacheSettings, IDistributedCache distributedCache) : ICacheBroker
{
    private readonly DistributedCacheEntryOptions _entryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSettings.Value.AbsoluteExpirationInSeconds),
        SlidingExpiration = TimeSpan.FromSeconds(cacheSettings.Value.SlidingExpirationInSeconds)
    };

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        var value = await distributedCache.GetAsync(key);
        return value is not null ? JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value)) : default;
    }

    public ValueTask<bool> TryGetAsync<T>(string key, out T value)
    {
        var foundEntry = distributedCache.GetString(key);

        if (foundEntry is not null)
        {
            value = JsonConvert.DeserializeObject<T>(foundEntry);
            return ValueTask.FromResult(true);
        }

        value = default;
        return ValueTask.FromResult(false);
    }

    public ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CacheEntryOptions? entryOptions = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default)
    {
        var options = new DistributedCacheEntryOptions();
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), _entryOptions);
    }

    public ValueTask DeleteAsync(string key)
    {
        throw new NotImplementedException();
    }
}