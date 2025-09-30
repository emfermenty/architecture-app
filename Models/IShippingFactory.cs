namespace architectureProject.Models;

public interface IShippingFactory
{
    Shipping CreateShipping();
    Vehicle CreateVehicle();
    bool ValidateShipping(double distance, double weight, double volume);
    double GetMaxWeight();
    double GetMaxVolume();
}