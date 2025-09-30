namespace architectureProject.Models.ShippingFactory;

public class AirShippingFactory : IShippingFactory
{
    public Shipping CreateShipping()
    {
        return new AirShipping();
    }
    public Vehicle CreateVehicle()
    {
        var random = new Random();
        return new CargoPlain
        {
            Id = Guid.NewGuid(),
            Model = "Boeing 747-8F",
            MaxWeight = 100,
            MaxVolume = 10,
            Speed = 800,
            FuelConsumption = 12000,
        };
    }
    public double GetMaxVolume() => 100;
    public double GetMaxWeight() => 10;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance >= 500;
    }
}