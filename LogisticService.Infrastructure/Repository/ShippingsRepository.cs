using LogisticService.Application.Services;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Infrastructure.Context;
using LogisticService.Infrastructure.Mappers;
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
            .Where(s => s.Status == ShippingStatus.Created.ToString() ||
                        s.Status == ShippingStatus.InTransit.ToString())
            .Include(x => x.Vehicle)
            .ToListAsync();
        return ShippingMapper.ToDomain(entity);
    }

    public async Task<Shipping?> GetShippingAsync(Guid id)
    {
        var entity = await _context.Shippings
            .FirstOrDefaultAsync(s => s.Id == id);
        return ShippingMapper.ToDomain(entity);
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

    public async Task<Shipping?> GetByTrackingNumberAsync(string TrackingNumber)
    {
        var entitry = await _context.Shippings
            .FirstOrDefaultAsync(s => s.TrackingNumber == TrackingNumber);
        return ShippingMapper.ToDomain(entitry);
    }

    public async Task ChangeStatusAsync(Shipping domain, string newStatus)
    {
        var entity = await _context.Shippings
            .FirstAsync(x => x.Id == domain.Id); 
        entity.Status = newStatus;
        await _context.SaveChangesAsync();
    }
}