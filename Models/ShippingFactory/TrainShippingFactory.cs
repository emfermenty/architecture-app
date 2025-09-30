namespace architectureProject.Models.ShippingFactory;

public class TrainShippingFactory : IShippingFactory
{
    public Shipping CreateShipping()
    {
        return new TrainShipping();
    }
    public Vehicle CreateVehicle()
    {
        var random = new Random();
        return new FreightTrain
        {
            Id = Guid.NewGuid(),
            Model = "Электровоз ВЛ80",
            MaxWeight = 500000,
            MaxVolume = 5000,
            Speed = 60,
            FuelConsumption = 150, // кВт·ч/100км
        };
    }
    public double GetMaxWeight() => 50000;
    public double GetMaxVolume() => 5000;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance >= 500;
    }
}