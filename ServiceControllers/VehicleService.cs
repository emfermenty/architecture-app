using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Models.enums;
using architectureProject.Repository;

namespace architectureProject.ServiceControllers;

public class VehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public async Task<Vehicle> CreateAsync(CreateVehicleCommandDto vehicledto)
    {
        Vehicle vehicle = vehicledto.VehicleType switch
        {
            VehicleType.Truck => new Truck(),
            VehicleType.CargoShip => new CargoShip(),
            VehicleType.CargoPlain => new CargoPlain(),
            VehicleType.FreightTrain => new FreightTrain(),
            _ => throw new ArgumentException("Unsupported vehicle type")
        };
        vehicle.Id = Guid.NewGuid(); 
        vehicle.Model = vehicledto.Model;
        vehicle.MaxWeight = vehicledto.MaxWeight;
        vehicle.MaxVolume = vehicledto.MaxVolume;
        vehicle.Speed = vehicledto.Speed;
        vehicle.FuelConsumption = vehicledto.FuelConsumption;
        vehicle.VehicleType = vehicledto.VehicleType;
        Console.WriteLine(vehicle.ToString());
        await _vehicleRepository.CreateAsync(vehicle);
        return vehicle;
    }
}