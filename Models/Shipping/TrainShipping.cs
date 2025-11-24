using gprcoptimizer.Models.Shipping.Implimintation;
using gprcoptimizer.Models.Enums;

namespace gprcoptimizer.Models.Shipping
{
    public class TrainShipping : Abstract.Shipping
    {
        public TrainShipping() : base(new TrainShippingImplimentation())
        {
            Id = Guid.NewGuid();
            ShippingType = ShippingType.Train;
        }

        public override double CalculateCost()
        {
            double baseCost = Distance * 0.15;
            return baseCost;
        }
        public override TimeSpan CalculateDuration()
        {
            double hours = (Distance / 60);
            return TimeSpan.FromHours(hours);
        }
    }
}
