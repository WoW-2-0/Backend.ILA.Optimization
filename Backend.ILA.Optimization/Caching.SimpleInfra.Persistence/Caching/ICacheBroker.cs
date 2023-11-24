namespace Caching.SimpleInfra.Persistence.Caching;

public interface ICacheBroker
{
    ValueTask<T> GetAsync<T>(string key);

    ValueTask<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory);
    
    ValueTask SetAsync<T>(string key, T value);

    ValueTask DeleteAsync(string key);
}