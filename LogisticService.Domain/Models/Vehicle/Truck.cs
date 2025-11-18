using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;

namespace LogisticService.Domain.Models.Vehicle;

public class Truck : Abstract.Vehicle
{
    private const double MAX_TRUCK_WEIGHT = 25000; 
    private const double MAX_TRUCK_VOLUME = 120;   

    public Truck(
        Guid id,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption)
        : base(id, model, maxWeight, maxVolume, speed, fuelConsumption, VehicleType.Truck)
    {
        VehicleValidator.Validate(
            VehicleValidationRules.Truck,
            maxWeight, maxVolume, speed, fuelConsumption);
    }

    public override double CalculateOperatingCost(double distance)
    {
        if (distance <= 0)
            throw new ArgumentException("Дистанция должна быть больше 0", nameof(distance));

        double fuelCost = (distance / 100) * FuelConsumption * 1.8;
        double driverCost = (distance / 80) * 25;
        double maintenance = distance * 0.02;
        return fuelCost + driverCost + maintenance;
    }

    public override string GetVehicleInfo()
    {
        return $"Грузовик {Model} - {MaxWeight}кг, {MaxVolume}м³, расход: {FuelConsumption}л/100км";
    }
    
    public static Truck Create(
        string model, 
        double maxWeight, 
        double maxVolume, 
        double speed, 
        double fuelConsumption)
    {
        VehicleValidator.ValidateForCreation(
            VehicleValidationRules.Truck,
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
        
        return new Truck(
            Guid.NewGuid(),
            model,
            maxWeight,
            maxVolume,
            speed,
            fuelConsumption);
    }
    
    public bool CanCarryShipping(Shipping.Abstract.Shipping shipping)
    {
        if (shipping == null)
            throw new ArgumentNullException(nameof(shipping));

        return CanCarry(shipping.Weight, shipping.Volume);
    }
    
    public void AddShipping(Shipping.Abstract.Shipping shipping)
    {
        if (shipping == null)
            throw new ArgumentNullException(nameof(shipping));

        if (!CanCarryShipping(shipping))
            throw new InvalidOperationException(
                $"Грузовик не может перевезти груз: вес {shipping.Weight}кг > {MaxWeight}кг " +
                $"или объем {shipping.Volume}м³ > {MaxVolume}м³");

        Shippings.Add(shipping);
    }
}