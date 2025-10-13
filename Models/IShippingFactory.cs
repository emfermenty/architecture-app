using architectureProject.Models.enums;

namespace architectureProject.Models;

public interface IShippingFactory
{
    ShippingType Type { get; }
    Shipping CreateShipping();
    Vehicle? TakeOptimalVehicle();
    bool ValidateShipping(double distance, double weight, double volume);
    double GetMaxWeight();
    double GetMaxVolume();
}