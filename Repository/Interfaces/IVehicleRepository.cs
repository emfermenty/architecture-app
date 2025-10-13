using architectureProject.Models;
using architectureProject.Models.enums;

namespace architectureProject.Repository;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetAsync(Guid id);
    Vehicle? TakeVehicleByShipping(VehicleType shippingType);
    Task<Guid> DeleteAsync(Guid id);
    Task CreateAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
}