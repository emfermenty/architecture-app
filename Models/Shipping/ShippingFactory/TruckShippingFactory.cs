using gprcoptimizer.Models.Enums;
using gprcoptimizer.Models.Shipping.ShippingFactory.Interfaces;

namespace gprcoptimizer.Models.Shipping.ShippingFactory
{
    public class TruckShippingFactory : IShippingFactory
    {
        public ShippingType Type => ShippingType.Truck;

        public double GetMaxWeight() => 25000;
        public double GetMaxVolume() => 90;

        public bool ValidateShipping(double distance, double weight, double volume)
        {
            return weight <= GetMaxWeight() &&
                   volume <= GetMaxVolume() &&
                   distance <= 5000;
        }

        public Abstract.Shipping CreateShipping()
        {
            return new TruckShipping();
        }
    }
}
