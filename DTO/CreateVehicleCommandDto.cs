using architectureProject.Models.enums;

namespace architectureProject.DTO;

public class CreateVehicleCommandDto
{
    public VehicleType VehicleType { get; set; }
    public string Model { get; set; }
    public double MaxWeight { get; set; }
    public double MaxVolume { get; set; }
    public double Speed { get; set; }
    public double FuelConsumption { get; set; }
}