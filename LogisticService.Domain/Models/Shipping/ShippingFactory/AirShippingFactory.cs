using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;

namespace LogisticService.Domain.Models.Shipping.ShippingFactory;

public class AirShippingFactory : IShippingFactory
{
    public ShippingType Type => ShippingType.Air;
    public VehicleType VehicleType => VehicleType.CargoPlain;
    public double GetMaxVolume() => 100;
    public double GetMaxWeight() => 10;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance >= 500;
    }
    public Abstract.Shipping CreateShipping()
    {
        return new AirShipping();
    }
}