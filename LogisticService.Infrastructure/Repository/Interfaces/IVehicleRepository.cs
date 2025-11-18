using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;

namespace LogisticService.Infrastructure.Repository;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<List<Vehicle>> GetAvailableVehiclesAsync(VehicleType type);
    Task<Vehicle?> GetAsync(Guid id);
    Vehicle? TakeVehicleByShipping(VehicleType shippingType);
    Task<Guid> DeleteAsync(Guid id);
    Task CreateAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task<List<Vehicle>> GetFreeVehiclesAsync(VehicleType vehicleType);
}