using LogisticService.Domain.Enums;

namespace LogisticService.Infrastructure.Entity;

public class VehicleEntity
{
    public Guid Id { get; set; }
    public string Model { get; set; } = null!;
    public double MaxWeight { get; set; }
    public double MaxVolume { get; set; }
    public double Speed { get; set; }
    public double FuelConsumption { get; set; }
    public VehicleType VehicleType { get; set; }
    
    public ICollection<ShippingEntity> Shippings { get; set; } = new List<ShippingEntity>();
}