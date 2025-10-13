using architectureProject.Data;
using architectureProject.Models;
using Microsoft.EntityFrameworkCore;

namespace architectureProject.Repository;

public class ShippingsRepository : IShippingsRepository
{
    private readonly ApplicationContext _context;

    public ShippingsRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Shipping> GetAllShippingsAsync()
    {
        return _context.Shippings
            .AsNoTracking()
            .ToList();
    }
    
    public async Task<Shipping?> GetShippingAsync(Guid id)
    {
        return await  _context.Shippings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<Guid> DeleteShippingAsync(Guid id)
    {
        var shipping = await _context.Shippings.FindAsync(id);
        _context.Shippings.Remove(shipping);
        await _context.SaveChangesAsync();
        return shipping.Id;
    }

    public async Task UpdateShippingAsync(Shipping shipping)
    {
        _context.Shippings.Update(shipping);
        await _context.SaveChangesAsync();
    }

    public async Task AddShippingAsync(Shipping shipping)
    {
        _context.Shippings.Add(shipping);
        await _context.SaveChangesAsync();
    }
}