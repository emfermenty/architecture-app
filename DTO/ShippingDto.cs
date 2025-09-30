using architectureProject.Models;
using architectureProject.Models.enums;

namespace architectureProject.DTO;

public class ShippingDto : Shipping
{
    public Guid Id {get; set;}
    public string TrackingNumber {get; set;}
    public double Distance {get; set;}
    public double Weight {get; set;}
    public double Volume {get; set;}
    public ShippingType ShippingType {get; set; }
    public double Cost { get; set; }
    public TimeSpan Duration { get; set; }
    public string TypeDescription { get; set; }
    public override double CalculateCost() => 15;
    public override TimeSpan CalculateDuration() => TimeSpan.FromHours(1);
}