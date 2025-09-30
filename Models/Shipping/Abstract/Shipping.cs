using architectureProject.Models.enums;

namespace architectureProject.Models;

public abstract class Shipping
{
    public Guid Id {get; set;}
    public string TrackingNumber {get; set;}
    public double Distance {get; set;}
    public double Weight {get; set;}
    public double Volume {get; set;}
    public ShippingType ShippingType {get; set; }
    public double Cost { get; set; }
    public TimeSpan Duration { get; set; }
    public abstract double CalculateCost();
    public abstract TimeSpan CalculateDuration();
    public Guid VehicleId {get; set;}
    public Vehicle Vehicle { get; set; } = null!;
    public string TypeDescription { get; set; }
}