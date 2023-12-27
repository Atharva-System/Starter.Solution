namespace Starter.Application.Contracts.Caching;
public interface ICacheKeyService : IScopedService
{
    public string GetCacheKey(string name, object id);
}
