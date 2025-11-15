using LogisticService.Domain.Enums;
namespace LogisticService.Domain.Models.Vehicle;

public class CargoPlain : Abstract.Vehicle
{
    public CargoPlain(
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