using System.Text.Json;
using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Models.enums;
using Microsoft.Extensions.Caching.Distributed;

namespace architectureProject.Repository.Decorators;

public class CachedShippingsRepository : IShippingsRepository
{
    private readonly IShippingsRepository _shippingsRepository;
    private readonly IDistributedCache _cache;

    public CachedShippingsRepository(IShippingsRepository shippingsRepository, IDistributedCache distributedCache)
    {
        _shippingsRepository = shippingsRepository;
        _cache = distributedCache;
    }
    
    public List<ShippingDto?> GetAllShippingsAsync()
    {
        const string cachekey = "all_shippings_cache";
        var cachedata = _cache.GetString(cachekey);
        if (cachedata != null) {
            Console.WriteLine("Данные получены из кэша");
            return JsonSerializer.Deserialize<List<ShippingDto?>>(cachedata);
        }
        Console.WriteLine("Данные получены из базы данных");
        var data = _shippingsRepository.GetAllShippingsAsync();
        _cache.SetStringAsync(
            cachekey,
            JsonSerializer.Serialize(data),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) }
        );
        Console.WriteLine("Данные получены из кэша");
        return data;
    }

    public async Task<Shipping?> GetShippingAsync(Guid id)
    {
        string cachekey = $"post_{id}";
        var cachedata = await _cache.GetStringAsync(cachekey);
        if (cachedata != null)
        {
            return JsonSerializer.Deserialize<Shipping>(cachedata);
        }
        var data = await _shippingsRepository.GetShippingAsync(id);
        if(data != null)
        {
            await _cache.SetStringAsync(cachekey,
                JsonSerializer.Serialize(data),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)}
            );
        }
        return data;
    }

    public async Task<Guid> DeleteShippingAsync(Guid id)
    {
        await _shippingsRepository.DeleteShippingAsync(id);
        await _cache.RemoveAsync($"post_{id}");
        await _cache.RemoveAsync("all_posts_cache");
        return id;
    }

    public async Task UpdateShippingAsync(Shipping shipping)
    {
        await _shippingsRepository.UpdateShippingAsync(shipping);
        await _cache.RemoveAsync($"{shipping.Id}");
        await _cache.RemoveAsync("all_posts_cache");
    }

    public async Task AddShippingAsync(Shipping shipping)
    {
        await _shippingsRepository.AddShippingAsync(shipping);
        await _cache.RemoveAsync("all_posts_cache");
    }
}