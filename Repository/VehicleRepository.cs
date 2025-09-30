using architectureProject.Data;
using architectureProject.DTO;
using architectureProject.Models;
using Microsoft.EntityFrameworkCore;

namespace architectureProject.Repository;

public class VehicleRepository
{
    private readonly ApplicationContext _context;

    public VehicleRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Vehicle?> GetAsync(Guid id)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }
    public async Task<Guid> DeleteAsync(Guid id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return vehicle.Id;
    }

    public async Task CreateAsync(CreateVehicleCommandDto vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }
}