namespace LogisticService.Domain.Models.Vehicle;

public class VehicleValidationRules
{
    public double MaxWeight { get; }
    public double MaxVolume { get; }
    public double MaxSpeed { get; }
    public double MaxFuelConsumption { get; }
    public string VehicleTypeName { get; }

    public VehicleValidationRules(
        double maxWeight, 
        double maxVolume, 
        double maxSpeed, 
        double maxFuelConsumption, 
        string vehicleTypeName)
    {
        MaxWeight = maxWeight;
        MaxVolume = maxVolume;
        MaxSpeed = maxSpeed;
        MaxFuelConsumption = maxFuelConsumption;
        VehicleTypeName = vehicleTypeName;
    }

    // Предопределенные правила для каждого типа транспорта
    public static VehicleValidationRules Truck => new(25000, 120, 120, 50, "грузовика");
    public static VehicleValidationRules CargoShip => new(100000, 1000, 60, 70, "корабля");
    public static VehicleValidationRules CargoPlain => new(100000, 500, 1000, 8000, "самолета");
    public static VehicleValidationRules FreightTrain => new(50000, 5000, 80, 100, "поезда");
}