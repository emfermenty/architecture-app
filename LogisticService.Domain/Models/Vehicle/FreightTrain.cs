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
        VehicleValidator.Validate(
            VehicleValidationRules.FreightTrain,
            maxWeight, maxVolume, speed, fuelConsumption);
    }

    public override double CalculateOperatingCost(double distance)
    {
        if (distance <= 0)
            throw new ArgumentException("Дистанция должна быть больше 0", nameof(distance));

        double energyCost = (distance / 100) * FuelConsumption * 0.15;
        double railwayFees = distance * 0.08;
        return energyCost + railwayFees;
    }

    public override string GetVehicleInfo()
    {
        return $"Грузовой поезд {Model} - {MaxWeight}кг, {MaxVolume}м³";
    }
    
    public static FreightTrain Create(
        string model, 
        double maxWeight, 
        double maxVolume, 
        double speed, 
        double fuelConsumption)
    {
        VehicleValidator.ValidateForCreation(
            VehicleValidationRules.FreightTrain,
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
        
        return new FreightTrain(
            Guid.NewGuid(),
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
    }
}