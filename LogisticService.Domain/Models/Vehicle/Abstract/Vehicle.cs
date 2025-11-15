using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping;

namespace LogisticService.Domain.Models.Vehicle.Abstract;

public abstract class Vehicle
{
    public Guid Id { get; set; }
    public string Model { get; set; }
    public double MaxWeight { get; set; }
    public double MaxVolume { get; set; }
    public double Speed { get; set; } 
    public double FuelConsumption { get; set; } 
    public VehicleType VehicleType { get; set; }
    public abstract double CalculateOperatingCost(double distance);
    public abstract string GetVehicleInfo();
        
    public List<Shipping.Abstract.Shipping> Shippings { get; set; } = new List<Shipping.Abstract.Shipping>();

    protected Vehicle(
        Guid id,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption,
        VehicleType type)
    {
        Id = id;
        Model = model;
        MaxWeight = maxWeight;
        MaxVolume = maxVolume;
        Speed = speed;
        FuelConsumption = fuelConsumption;
        VehicleType = type;
    }
}