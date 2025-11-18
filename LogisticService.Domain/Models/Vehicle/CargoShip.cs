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
        : base(id, model, maxWeight, maxVolume, speed, fuelConsumption, VehicleType.CargoShip) 
    {
        VehicleValidator.Validate(
            VehicleValidationRules.CargoShip,
            maxWeight, maxVolume, speed, fuelConsumption);
    }

    public override double CalculateOperatingCost(double distance)
    {
        if (distance <= 0)
            throw new ArgumentException("Дистанция должна быть больше 0", nameof(distance));

        double fuelCost = (distance / 100) * FuelConsumption * 0.8;
        double portFees = (distance / 1000) * 3000;
        double crewCost = (distance / 500) * 5000; // стоимость экипажа
        return fuelCost + portFees + crewCost;
    }

    public override string GetVehicleInfo()
    {
        return $"Грузовое судно {Model} - {MaxWeight}кг, {MaxVolume}м³, скорость: {Speed} узлов";
    }

    // Фабричный метод для создания
    public static CargoShip Create(
        string model, 
        double maxWeight, 
        double maxVolume, 
        double speed, 
        double fuelConsumption)
    {
        VehicleValidator.ValidateForCreation(
            VehicleValidationRules.CargoShip,
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
        
        return new CargoShip(
            Guid.NewGuid(),
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
    }
}