using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;

namespace LogisticService.Domain.Models.Shipping.ShippingFactory;

public class SeaShippingFactory : IShippingFactory
{
    public ShippingType Type => ShippingType.Sea;
    public VehicleType VehicleType => VehicleType.CargoShip;
    public double GetMaxWeight() => 100000;
    public double GetMaxVolume() => 1000;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance <= 5000;
    }

    public Abstract.Shipping CreateShipping()
    {
        return new SeaShipping();
    }
}