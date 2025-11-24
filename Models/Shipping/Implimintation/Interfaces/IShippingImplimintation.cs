namespace gprcoptimizer.Models.Shipping.Implimintation.Interfaces
{
    public interface IShippingImplementation
    {
        double CalculateCost(double distance, double weight, double volume);
        TimeSpan CalculateDuration(double distance);
    }
}
