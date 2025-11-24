using gprcoptimizer.Models.Shipping.Implimintation;
using gprcoptimizer.Models.Enums;

namespace gprcoptimizer.Models.Shipping
{
    public class TruckShipping : Abstract.Shipping
    {
        public TruckShipping() : base(new TruckShippingImplementation())
        {
            Id = Guid.NewGuid();
            ShippingType = ShippingType.Truck;
        }

        public override double CalculateCost()
        {
            double baseCost = Distance * 0.12;
            double weightCost = Weight * 0.08;
            return baseCost + weightCost;
        }
        public override TimeSpan CalculateDuration()
        {
            double hours = (Distance / 80);
            return TimeSpan.FromHours(hours);
        }
    }
}
