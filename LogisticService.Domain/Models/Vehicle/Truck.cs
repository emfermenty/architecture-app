using LogisticService.Domain.Enums;

namespace LogisticService.Domain.Models.Vehicle;

public class Truck : Abstract.Vehicle
{
    public Truck(Guid id,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption)
        : base(id, model, maxWeight, maxVolume, speed, fuelConsumption, VehicleType.Truck)
    {
    }
    public override double CalculateOperatingCost(double distance)
    {
        double fuelCost = (distance / 100) * FuelConsumption * 1.8;
        double driverCost = (distance / 80) * 25;
        double maintenance = distance * 0.02;
        return fuelCost + driverCost + maintenance;
    }
    public override string GetVehicleInfo()
    {
        return $"Грузовик {Model} - {MaxWeight}кг, {MaxVolume}м³";
    }
}