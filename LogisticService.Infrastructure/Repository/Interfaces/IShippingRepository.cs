using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Infrastructure.Repository.Interfaces;

public interface IShippingsRepository
{
    Task<List<Shipping>> GetAllShippingsAsync();
    Task<Shipping?> GetShippingAsync(Guid id);
    Task<Guid> DeleteShippingAsync(Guid id);
    Task UpdateShippingAsync(Shipping shipping);
    Task AddShippingAsync(Shipping shipping);
}