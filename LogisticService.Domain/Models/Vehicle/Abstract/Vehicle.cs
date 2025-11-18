using LogisticService.Domain.Enums;

namespace LogisticService.Domain.Models.Vehicle.Abstract;

public abstract class Vehicle
{
    public Guid Id { get; protected set; }
    public string Model { get; protected set; }
    public double MaxWeight { get; protected set; }
    public double MaxVolume { get; protected set; }
    public double Speed { get; protected set; } 
    public double FuelConsumption { get; protected set; } 
    public VehicleType VehicleType { get; protected set; }
    public abstract double CalculateOperatingCost(double distance);
    public abstract string GetVehicleInfo();
    
    public bool CanCarry(double weight, double volume)
    {
        return weight <= MaxWeight && volume <= MaxVolume;
    }
        
    public List<Shipping.Abstract.Shipping> Shippings { get; set; } = new List<Shipping.Abstract.Shipping>();

    protected Vehicle(
        Guid id,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption,
        VehicleType type)
    {
        // Валидация в конструкторе
        ValidateParameters(model, maxWeight, maxVolume, speed, fuelConsumption);
        
        Id = id;
        Model = model;
        MaxWeight = maxWeight;
        MaxVolume = maxVolume;
        Speed = speed;
        FuelConsumption = fuelConsumption;
        VehicleType = type;
    }
    
    protected virtual void ValidateParameters(
        string model, 
        double maxWeight, 
        double maxVolume, 
        double speed, 
        double fuelConsumption)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Модель транспортного средства не может быть пустой", nameof(model));
            
        if (maxWeight <= 0)
            throw new ArgumentException("Максимальный вес должен быть больше 0", nameof(maxWeight));
            
        if (maxVolume <= 0)
            throw new ArgumentException("Максимальный объем должен быть больше 0", nameof(maxVolume));
            
        if (speed <= 0)
            throw new ArgumentException("Скорость должна быть больше 0", nameof(speed));
            
        if (fuelConsumption <= 0)
            throw new ArgumentException("Расход топлива должен быть больше 0", nameof(fuelConsumption));
    }
}