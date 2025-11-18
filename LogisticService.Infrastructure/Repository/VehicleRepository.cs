using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Infrastructure.Context;
using LogisticService.Infrastructure.Entity;
using LogisticService.Infrastructure.Mappers;
using LogisticService.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogisticService.Infrastructure.Repository;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationContext _context;

    public VehicleRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        var entity =  await _context.Vehicles
            .ToListAsync();
        if (entity.Count == 0)
        {
            return new List<Vehicle>();
        }
        return entity.Select(VehicleMapper.ToDomain)
            .ToList();
    }

    public async Task<List<Vehicle>> GetAvailableVehiclesAsync(VehicleType type)
    {
        var entities = await _context.Vehicles
            .AsNoTracking()
            .Where(x => x.VehicleType == type)
            .ToListAsync(); 
    
        return entities.Select(VehicleMapper.ToDomain).ToList();
    }
    
    public async Task<Vehicle?> GetAsync(Guid id)
    {
        var entity = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id);
        return entity == null ? null : VehicleMapper.ToDomain(entity);
    }

    public Vehicle? TakeVehicleByShipping(VehicleType shippingType)
    {
        var vehicle = _context.Vehicles
            .FirstOrDefault(v => v.VehicleType == shippingType);
        ArgumentNullException.ThrowIfNull(vehicle);
        return VehicleMapper.ToDomain(vehicle);
    }
    public async Task<Guid> DeleteAsync(Guid id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return vehicle.Id;
    }

    public async Task CreateAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(VehicleMapper.ToEntity(vehicle));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(VehicleMapper.ToEntity(vehicle));
        await _context.SaveChangesAsync();
    }

    public async Task<List<Vehicle>> GetFreeVehiclesAsync(VehicleType vehicleType)
    {
        var freeVehicles = await _context.Vehicles
            .Where(v => v.VehicleType == vehicleType)
            .Where(v => !_context.Shippings
                .Any(s => s.VehicleId == v.Id && 
                          (s.Status == ShippingStatus.Created.ToString() || 
                           s.Status == ShippingStatus.InTransit.ToString())))
            .ToListAsync();

        return freeVehicles.Select(VehicleMapper.ToDomain).ToList();
    }
}