namespace architectureProject.Models.ShippingFactory;

public class TrackShippingFactory : IShippingFactory
{
    public Shipping CreateShipping()
    {
        return new TruckShipping();
    }
    public Vehicle CreateVehicle()
    {
        var random = new Random();
        return new Truck 
        { 
            Id = Guid.NewGuid(),
            Model = "Volvo FH16",
            MaxWeight = 25000,
            MaxVolume = 90,
            Speed = 80,
            FuelConsumption = 35,
        };
    }
    public double GetMaxWeight() => 25000;
    public double GetMaxVolume() => 90;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() &&
               volume <= GetMaxVolume() &&
               distance <= 5000;
    }
}