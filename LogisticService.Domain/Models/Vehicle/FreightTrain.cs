using LogisticService.Domain.Enums;

namespace LogisticService.Domain.Models.Vehicle;

public class FreightTrain : Abstract.Vehicle
{
    public FreightTrain(
        Guid id,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption)
        : base(id, model, maxWeight, maxVolume, speed, fuelConsumption, VehicleType.FreightTrain)
    {
    }
    public override double CalculateOperatingCost(double distance)
    {
        double energyCost = (distance / 100) * FuelConsumption * 0.15;
        double railwayFees = distance * 0.08;
        return energyCost + railwayFees;
    }
    public override string GetVehicleInfo()
    {
        return $"Грузовой поезд- {Model} вагонов, {MaxWeight}кг";
    }
}