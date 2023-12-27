﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Starter.Application.Contracts.Caching;

namespace Starter.InfraStructure.Caching;
public class LocalCacheService : ICacheService
{
    private readonly ILogger<LocalCacheService> _logger;
    private readonly IMemoryCache _cache;

    public LocalCacheService(IMemoryCache cache, ILogger<LocalCacheService> logger) =>
        (_cache, _logger) = (cache, logger);
    public T? Get<T>(string key) =>
        _cache.Get<T>(key);
    
    public Task<T?> GetAsync<T>(string key, CancellationToken token = default) =>
        Task.FromResult(default(T?));
   
    public void Refresh(string key) =>
        _cache.TryGetValue(key, out _);

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        Refresh(key);
        return Task.CompletedTask;
    }

    public void Remove(string key) =>
        _cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        slidingExpiration ??= TimeSpan.FromMinutes(10); // Default expiration time of 10 minutes.

        _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        _logger.LogDebug($"Added to Cache : {key}", key);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        Set(key, value, slidingExpiration);
        return Task.CompletedTask;
    }
}
