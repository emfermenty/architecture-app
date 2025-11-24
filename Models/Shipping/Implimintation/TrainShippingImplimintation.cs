using gprcoptimizer.Models.Shipping.Implimintation.Interfaces;

namespace gprcoptimizer.Models.Shipping.Implimintation
{
    public class TrainShippingImplimentation : IShippingImplementation
    {
        public double CalculateCost(double Distance, double weight, double Volume)
        {
            double baseCost = Distance * 0.05;
            return baseCost;
        }
        public TimeSpan CalculateDuration(double Distance)
        {
            double hours = (Distance / 60);
            return TimeSpan.FromHours(hours);
        }
    }
}
