using LogisticService.Domain.Enums;

namespace LogisticService.Application.DTO;

public class CreateVehicleCommandDTO
{
    public VehicleType VehicleType { get; set; }
    public string Model { get; set; }
    public double MaxWeight { get; set; }
    public double MaxVolume { get; set; }
    public double Speed { get; set; }
    public double FuelConsumption { get; set; }
}