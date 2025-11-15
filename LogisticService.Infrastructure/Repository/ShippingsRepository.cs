using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Infrastructure.Context;
using LogisticService.Infrastructure.Mappers;
using LogisticService.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogisticService.Infrastructure.Repository;

public class ShippingsRepository : IShippingsRepository
{
    private readonly ApplicationContext _context;

    public ShippingsRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Shipping>> GetAllShippingsAsync()
    {
        var entity = await _context.Shippings
            .Include(x => x.Vehicle)
            .ToListAsync();
        return ShippingMapper.ToDomain(entity);
    }
    
    public async Task<Shipping?> GetShippingAsync(Guid id)
    {
        var entity =  await _context.Shippings
            .FirstOrDefaultAsync(s => s.Id == id);
        return  ShippingMapper.ToDomain(entity);
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
        var entity = ShippingMapper.ToEntity(shipping);
        _context.Shippings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddShippingAsync(Shipping shipping)
    {
        var entity = ShippingMapper.ToEntity(shipping);
        _context.Shippings.Add(entity);
        await _context.SaveChangesAsync();
    }
}