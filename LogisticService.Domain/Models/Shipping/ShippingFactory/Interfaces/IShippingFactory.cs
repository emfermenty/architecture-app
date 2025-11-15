using LogisticService.Domain.Enums;

namespace LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;

public interface IShippingFactory
{
    ShippingType Type { get; }
    Abstract.Shipping CreateShipping();
    bool ValidateShipping(double distance, double weight, double volume);
    double GetMaxWeight();
    double GetMaxVolume();
    VehicleType VehicleType { get; }
}