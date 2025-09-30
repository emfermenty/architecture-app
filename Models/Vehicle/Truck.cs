using architectureProject.Models.enums;

namespace architectureProject.Models;

public class Truck : Vehicle
{
    public Truck()
    {
        VehicleType = VehicleType.Truck;
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