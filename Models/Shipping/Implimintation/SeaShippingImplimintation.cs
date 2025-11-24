using gprcoptimizer.Models.Shipping.Implimintation.Interfaces;

namespace gprcoptimizer.Models.Shipping.Implimintation
{
    public class SeaShippingImplimentation : IShippingImplementation
    {
        public double CalculateCost(double Distance, double weight, double Volume)
        {
            double baseCost = Distance * 0.05;
            double volumeCost = Volume * 15;
            return baseCost + volumeCost;
        }
        public TimeSpan CalculateDuration(double Distance)
        {
            double hours = (Distance / 60);
            return TimeSpan.FromHours(hours);
        }
    }
}
