using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Infrastructure.Entity;

namespace LogisticService.Infrastructure.Mappers;

public static class ShippingMapper
{
    public static Shipping ToDomain(ShippingEntity entity)
    {
        Shipping shipping = entity.ShippingType switch
        {
            ShippingType.Air       => new AirShipping(),
            ShippingType.Train     => new TrainShipping(),
            ShippingType.Sea       => new SeaShipping(),
            ShippingType.Truck     => new TruckShipping(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        shipping.Id = entity.Id;
        shipping.TrackingNumber = entity.TrackingNumber;
        shipping.Distance = entity.Distance;
        shipping.Weight = entity.Weight;
        shipping.Volume = entity.Volume;
        shipping.ShippingType = entity.ShippingType;
        shipping.Cost = entity.Cost;
        shipping.Duration = entity.Duration;
        shipping.VehicleId = entity.VehicleId;
        shipping.TypeDescription = entity.TypeDescription;
        shipping.Status = entity.Status; 
        
        if (entity.Vehicle != null)
        {
            shipping.Vehicle = VehicleMapper.ToDomain(entity.Vehicle);
        }

        return shipping;
    }

    public static List<Shipping> ToDomain(List<ShippingEntity> entities)
    {
        return entities.Select(ToDomain).ToList();
    }
    public static ShippingEntity ToEntity(Shipping shipping)
    {
        return new ShippingEntity
        {
            Id = shipping.Id,
            TrackingNumber = shipping.TrackingNumber,
            Distance = shipping.Distance,
            Weight = shipping.Weight,
            Volume = shipping.Volume,
            ShippingType = shipping.ShippingType,
            Cost = shipping.Cost,
            Duration = shipping.Duration,
            VehicleId = shipping.VehicleId,
            Status = shipping.Status,
            StartShipping = shipping.StartShipping,
            TypeDescription = shipping.TypeDescription,
            Vehicle = shipping.Vehicle != null ? 
                VehicleMapper.ToEntity(shipping.Vehicle) : null
        };
    }

    public static List<ShippingEntity> ToEntity(List<Shipping> shippings)
    {
        return shippings.Select(ToEntity).ToList();
    }
}