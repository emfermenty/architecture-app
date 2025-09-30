using architectureProject.Models.enums;

namespace architectureProject.Models;

public class FreightTrain : Vehicle
{
    public FreightTrain()
    {
        VehicleType = VehicleType.FreightTrain;
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