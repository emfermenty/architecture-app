using architectureProject.Models.enums;

namespace architectureProject.Models;

public class CargoPlain : Vehicle
{
    public CargoPlain()
    {
        VehicleType = VehicleType.CargoPlain;
    }
    public override double CalculateOperatingCost(double distance)
    {
        double fuelCost = (distance / 100) * FuelConsumption * 2.5;
        double crewCost = (distance / 800) * 5000;
        double airportFees = 3000;
        return fuelCost + crewCost + airportFees;
    }
    public override string GetVehicleInfo()
    {
        return $"Грузовой самолет {Model} - {MaxWeight}кг";
    }
}