using System.Text.Json.Serialization;
using architectureProject.Models.enums;

namespace architectureProject.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(AirShipping), "AirShipping")]
[JsonDerivedType(typeof(TrainShipping), "TrainShipping")]
[JsonDerivedType(typeof(SeaShipping), "SeaShipping")]
[JsonDerivedType(typeof(TruckShipping), "TruckShipping")]
public abstract class Shipping
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
    public Vehicle Vehicle { get; set; } = null!;
    public string TypeDescription { get; set; } = null!;

    public abstract double CalculateCost();
    public abstract TimeSpan CalculateDuration();
}