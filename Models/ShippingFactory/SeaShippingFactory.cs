namespace architectureProject.Models.ShippingFactory;

public class SeaShippingFactory : IShippingFactory
{
    public Shipping CreateShipping()
    {
        return new SeaShipping();
    }
    public Vehicle CreateVehicle()
    {
        return new CargoShip
        {
            Id = Guid.NewGuid(),
            Model = "Maersk Triple-E",
            MaxWeight = 100000,
            MaxVolume = 1000,
            Speed = 25,
            FuelConsumption = 200,
        };
    }

    public double GetMaxWeight() => 100000;
    public double GetMaxVolume() => 1000;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance <= 5000;
    }
}