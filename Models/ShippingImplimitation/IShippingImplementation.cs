namespace architectureProject.Models.ShippingImplimitation;

public interface IShippingImplementation
{
    double CalculateCost(double distance, double weight, double volume);
    TimeSpan CalculateDuration(double distance);
    double GetMaxWeight();
    double GetMaxVolume();
}