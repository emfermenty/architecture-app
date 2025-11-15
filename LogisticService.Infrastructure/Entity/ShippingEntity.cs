using LogisticService.Domain.Enums;

namespace LogisticService.Infrastructure.Entity;

public class ShippingEntity
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
    
    public VehicleEntity Vehicle { get; set; } = null!;
        
    public string Status { get; set; } = ShippingStatus.Created.ToString();
    public DateTime? StartShipping { get; set; } = DateTime.UtcNow;
    public string TypeDescription { get; set; } = null!;
}