using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Infrastructure.Entity;

namespace LogisticService.Infrastructure.Mappers;

public class VehicleMapper
{
    public static Vehicle ToDomain(VehicleEntity entity)
    {
        return entity.VehicleType switch
        {
            VehicleType.Truck => new Truck(
                entity.Id,
                entity.Model,
                entity.MaxWeight,
                entity.MaxVolume,
                entity.Speed,
                entity.FuelConsumption),

            VehicleType.CargoShip => new CargoShip(
                entity.Id,
                entity.Model,
                entity.MaxWeight,
                entity.MaxVolume,
                entity.Speed,
                entity.FuelConsumption),

            VehicleType.FreightTrain => new FreightTrain(
                entity.Id,
                entity.Model,
                entity.MaxWeight,
                entity.MaxVolume,
                entity.Speed,
                entity.FuelConsumption),

            VehicleType.CargoPlain => new CargoPlain(
                entity.Id,
                entity.Model,
                entity.MaxWeight,
                entity.MaxVolume,
                entity.Speed,
                entity.FuelConsumption),

            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static VehicleEntity ToEntity(Vehicle domain)
    {
        return new VehicleEntity
        {
            Id = domain.Id,
            Model = domain.Model,
            VehicleType = domain.VehicleType,
            MaxWeight = domain.MaxWeight,
            MaxVolume = domain.MaxVolume,
            Speed = domain.Speed,
            FuelConsumption = domain.FuelConsumption
        };
    }
}