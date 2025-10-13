using architectureProject.Models;

namespace architectureProject.Repository;

public interface IShippingsRepository
{
    List<Shipping> GetAllShippingsAsync();
    Task<Shipping?> GetShippingAsync(Guid id);
    Task<Guid> DeleteShippingAsync(Guid id);
    Task UpdateShippingAsync(Shipping shipping);
    Task AddShippingAsync(Shipping shipping);
}