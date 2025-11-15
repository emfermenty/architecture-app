using LogisticService.Application.DTO;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Infrastructure.Repository.Interfaces;

namespace LogisticService.Application.Services;

public class VehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle> CreateAsync(CreateVehicleCommandDTO vehicledto)
    {
        var id = Guid.NewGuid();

        Vehicle vehicle = vehicledto.VehicleType switch
        {
            VehicleType.Truck => new Truck(
                id,
                vehicledto.Model,
                vehicledto.MaxWeight,
                vehicledto.MaxVolume,
                vehicledto.Speed,
                vehicledto.FuelConsumption),

            VehicleType.CargoShip => new CargoShip(
                id,
                vehicledto.Model,
                vehicledto.MaxWeight,
                vehicledto.MaxVolume,
                vehicledto.Speed,
                vehicledto.FuelConsumption),

            VehicleType.CargoPlain => new CargoPlain(
                id,
                vehicledto.Model,
                vehicledto.MaxWeight,
                vehicledto.MaxVolume,
                vehicledto.Speed,
                vehicledto.FuelConsumption),

            VehicleType.FreightTrain => new FreightTrain(
                id,
                vehicledto.Model,
                vehicledto.MaxWeight,
                vehicledto.MaxVolume,
                vehicledto.Speed,
                vehicledto.FuelConsumption),

            _ => throw new ArgumentException("Unsupported vehicle type")
        };

        await _vehicleRepository.CreateAsync(vehicle);
        return vehicle;
    }
}