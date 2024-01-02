using Starter.Application.Contracts.Caching;

namespace Starter.InfraStructure.Caching;
public class CacheKeyService : ICacheKeyService
{
    public string GetCacheKey(string name, object id)
    {
        return $"{name}-{id}";
    }
}
