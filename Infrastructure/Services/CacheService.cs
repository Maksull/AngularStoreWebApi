using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Infrastructure.Services
{
    public sealed class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            string? cachedValue = await _cache.GetStringAsync(key, CancellationToken.None);

            if (cachedValue != null)
            {
                T? value = JsonSerializer.Deserialize<T>(cachedValue);

                return value;
            }

            return null;
        }

        public async Task SetAsync<T>(string key, T value) where T : class
        {
            string cacheValue = JsonSerializer.Serialize(value);

            var cacheEntryOptions = new DistributedCacheEntryOptions()
                                                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

            await _cache.SetStringAsync(key, cacheValue, cacheEntryOptions, CancellationToken.None);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
