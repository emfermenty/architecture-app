using LogisticService.Domain.Enums;

namespace LogisticService.Domain.Models.Vehicle;

public class CargoShip : Abstract.Vehicle
{
    public CargoShip(
        Guid id,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption)
        : base(id, model, maxWeight, maxVolume, speed, fuelConsumption, VehicleType.CargoPlain)
    {
    }
    public override double CalculateOperatingCost(double distance)
    {
        double fuelCost = (distance / 100) * FuelConsumption * 0.8;
        double portFees = (distance / 1000) * 3000;
        return fuelCost + portFees;
    }
    public override string GetVehicleInfo()
    {
        return $"Судно {Model} - {MaxWeight}кг, {MaxVolume}м";
    }
}