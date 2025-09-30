using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Repository;

namespace architectureProject.ServiceControllers;

public class VehicleService
{
    private readonly VehicleRepository _vehicleRepository;

    public VehicleService(VehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public async Task CreateAsync(CreateVehicleCommandDto vehicle)
    {
        await _vehicleRepository.CreateAsync(vehicle);
    }
}