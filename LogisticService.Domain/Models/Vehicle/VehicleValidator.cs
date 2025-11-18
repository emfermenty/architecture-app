namespace LogisticService.Domain.Models.Vehicle;

public static class VehicleValidator
{
    public static void Validate(
        VehicleValidationRules rules, 
        double maxWeight, 
        double maxVolume, 
        double speed, 
        double fuelConsumption)
    {
        var errors = new List<string>();

        if (maxWeight > rules.MaxWeight)
            errors.Add($"Максимальный вес {rules.VehicleTypeName} не может превышать {rules.MaxWeight} кг");

        if (maxVolume > rules.MaxVolume)
            errors.Add($"Максимальный объем {rules.VehicleTypeName} не может превышать {rules.MaxVolume} м³");

        if (speed > rules.MaxSpeed)
            errors.Add($"Скорость {rules.VehicleTypeName} не может превышать {rules.MaxSpeed} км/ч");

        if (fuelConsumption > rules.MaxFuelConsumption)
            errors.Add($"Расход топлива {rules.VehicleTypeName} не может превышать {rules.MaxFuelConsumption} л/100км");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));
    }
    
    public static void ValidateForCreation(
        VehicleValidationRules rules,
        string model,
        double maxWeight,
        double maxVolume,
        double speed,
        double fuelConsumption)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Модель не может быть пустой");
        
        if (maxWeight <= 0)
            throw new ArgumentException("Вес должен быть больше 0");

        if (maxVolume <= 0)
            throw new ArgumentException("Объем должен быть больше 0");

        if (speed <= 0)
            throw new ArgumentException("Скорость должна быть больше 0");

        if (fuelConsumption <= 0)
            throw new ArgumentException("Расход топлива должен быть больше 0");

        // Валидация специфичных ограничений
        Validate(rules, maxWeight, maxVolume, speed, fuelConsumption);
    }
}