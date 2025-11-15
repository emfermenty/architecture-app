using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Vehicle.Abstract;

namespace LogisticService.Application.Services;

public interface IVehicleProvider
{
    Task<Vehicle?> GetOptimalVehicleAsync(VehicleType type, double distance, double weight, double volume);
}