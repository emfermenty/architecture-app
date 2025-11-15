using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Infrastructure.Repository.Interfaces;

namespace LogisticService.Application.Services;

public class VehicleProvider : IVehicleProvider
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleProvider(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle?> GetOptimalVehicleAsync(VehicleType type, double distance, double weight, double volume)
    {
        var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync(type);
        return vehicles;
    }
}