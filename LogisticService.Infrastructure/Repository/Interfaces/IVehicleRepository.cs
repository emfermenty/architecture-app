using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Infrastructure.Entity;

namespace LogisticService.Infrastructure.Repository.Interfaces;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetAsync(Guid id);
    Task<Vehicle?> GetAvailableVehiclesAsync(VehicleType type);
    Vehicle? TakeVehicleByShipping(VehicleType shippingType);
    Task<Guid> DeleteAsync(Guid id);
    Task CreateAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
}