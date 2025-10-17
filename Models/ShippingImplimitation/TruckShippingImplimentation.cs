namespace architectureProject.Models.ShippingImplimitation;

public class TruckShippingImplementation : IShippingImplementation
{
    public double CalculateCost(double distance, double weight, double volume)
    {
        double baseCost = distance * 0.12;
        double weightCost = weight * 0.08;
        return baseCost + weightCost;
    }

    public TimeSpan CalculateDuration(double distance)
    {
        double hours = distance / 80;
        return TimeSpan.FromHours(hours);
    }
    
    public double GetMaxWeight() => 25000;
    public double GetMaxVolume() => 90;
}