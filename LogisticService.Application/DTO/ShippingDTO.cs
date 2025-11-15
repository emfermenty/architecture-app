using LogisticService.Domain.Enums;

namespace LogisticService.Application.DTO;

public class ShippingDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = null!;
    public double Distance { get; set; }
    public double Weight { get; set; }
    public double Volume { get; set; }
    public ShippingType ShippingType { get; set; }
    public double Cost { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid VehicleId { get; set; }
    public string VehicleModel { get; set; } = null!; 
    public string TypeDescription { get; set; } = null!;
}