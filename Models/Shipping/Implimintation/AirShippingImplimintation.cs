using gprcoptimizer.Models.Shipping.Implimintation.Interfaces;

namespace gprcoptimizer.Models.Shipping.Implimintation
{
    public class AirShippingImplementation : IShippingImplementation
    {
        public double CalculateCost(double distance, double weight, double volume)
        {
            double baseCost = distance * 0.25;
            double weightCost = weight * 2.5;
            return baseCost + weightCost;
        }

        public TimeSpan CalculateDuration(double distance)
        {
            double hours = (distance / 800) + 4;
            return TimeSpan.FromHours(hours);
        }
    }
}
