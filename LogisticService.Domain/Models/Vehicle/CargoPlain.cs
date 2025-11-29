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
        VehicleValidator.Validate(
            VehicleValidationRules.CargoPlain,
            maxWeight, maxVolume, speed, fuelConsumption);
    }

    public override double CalculateOperatingCost(double distance)
    {
        if (distance <= 0)
            throw new ArgumentException("Дистанция должна быть больше 0", nameof(distance));

        double fuelCost = (distance / 100) * FuelConsumption * 2.5;
        double crewCost = (distance / 800) * 5000;
        double airportFees = 3000;
        double airTrafficControl = distance * 0.5; // услуги УВД
        
        return fuelCost + crewCost + airportFees + airTrafficControl;
    }

    public override string GetVehicleInfo()
    {
        return $"Грузовой самолет {Model} - {MaxWeight}кг, {MaxVolume}м³, скорость: {Speed} км/ч";
    }
    
    public static CargoPlain Create(
        string model, 
        double maxWeight, 
        double maxVolume, 
        double speed, 
        double fuelConsumption)
    {
        VehicleValidator.ValidateForCreation(
            VehicleValidationRules.CargoPlain,
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
        
        return new CargoPlain(
            Guid.NewGuid(),
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
    }
}