using gprcoptimizer.Models.Enums;

namespace gprcoptimizer.Models.Shipping.ShippingFactory.Interfaces
{
    public interface IShippingFactory
    {
        ShippingType Type { get; }
        Abstract.Shipping CreateShipping();
        bool ValidateShipping(double distance, double weight, double volume);
        double GetMaxWeight();
        double GetMaxVolume();
    }
}
