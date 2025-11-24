using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Application.Services;

namespace LogisticService.Application.Services;

public class VehicleProvider : IVehicleProvider
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleProvider(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle?> GetOptimalVehicleAsync(VehicleType type, double distance, double weight, double volume)
    {
        var freeVehicles = await _vehicleRepository.GetFreeVehiclesAsync(type);
    
        if (freeVehicles == null || !freeVehicles.Any())
        {
            Console.WriteLine($"❌ No FREE vehicles found for type: {type}");
            return null;
        }

        Console.WriteLine($"🔍 Found {freeVehicles.Count} FREE vehicles for type: {type}");
        
        var suitableVehicles = freeVehicles
            .Where(v => v.CanCarry(weight, volume))
            .ToList();

        if (!suitableVehicles.Any())
        {
            Console.WriteLine($"❌ No suitable FREE vehicles for weight: {weight}, volume: {volume}");
            foreach (var v in freeVehicles)
            {
                Console.WriteLine($"   Available FREE: {v.GetVehicleInfo()}, CanCarry: {v.CanCarry(weight, volume)}");
            }
            return null;
        }
        var optimalVehicle = suitableVehicles
            .OrderBy(v => v.CalculateOperatingCost(distance))
            .First();

        Console.WriteLine($"✅ Selected FREE optimal vehicle: {optimalVehicle.GetVehicleInfo()}");
        return optimalVehicle;
    }
}