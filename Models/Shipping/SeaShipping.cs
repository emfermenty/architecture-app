using gprcoptimizer.Models.Shipping.Implimintation;
using gprcoptimizer.Models.Enums;

namespace gprcoptimizer.Models.Shipping
{
    public class SeaShipping : Abstract.Shipping
    {
        public SeaShipping() : base(new SeaShippingImplimentation())
        {
            Id = Guid.NewGuid();
            ShippingType = ShippingType.Sea;
        }
        public override double CalculateCost()
        {
            double baseCost = Distance * 0.02;
            double volumeCost = Volume * 15;
            return baseCost + volumeCost;
        }
        public override TimeSpan CalculateDuration()
        {
            double days = (Distance / 500);
            return TimeSpan.FromDays(days);
        }
    }
}
