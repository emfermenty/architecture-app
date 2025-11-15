using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;

namespace LogisticService.Domain.Models.Shipping.ShippingFactory;

public class TrainShippingFactory : IShippingFactory
{
    
    public VehicleType VehicleType => VehicleType.Truck;
    public ShippingType Type => ShippingType.Train;
    public double GetMaxWeight() => 50000;
    public double GetMaxVolume() => 5000;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance >= 500;
    }
    public Abstract.Shipping CreateShipping()
    {
        return new TrainShipping();
    }
}