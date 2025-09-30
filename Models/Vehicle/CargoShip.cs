using architectureProject.Models.enums;

namespace architectureProject.Models;

public class CargoShip : Vehicle
{
    public CargoShip()
    {
        VehicleType = VehicleType.CargoShip;
    }
    public override double CalculateOperatingCost(double distance)
    {
        double fuelCost = (distance / 100) * FuelConsumption * 0.8;
        double crewCost = (distance / 500) * 2000;
        double portFees = (distance / 1000) * 5000;
        return fuelCost + crewCost + portFees;
    }
    public override string GetVehicleInfo()
    {
        return $"Судно {Model} - {MaxWeight}кг, {MaxVolume}м";
    }
}