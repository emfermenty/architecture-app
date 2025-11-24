using gprcoptimizer.Models.Enums;
using gprcoptimizer.Models.Shipping.ShippingFactory.Interfaces;

namespace gprcoptimizer.Models.Shipping.ShippingFactory
{
    public class SeaShippingFactory : IShippingFactory
    {
        public ShippingType Type => ShippingType.Sea;
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
}
