using architectureProject.Models.enums;

namespace architectureProject.Models;

public class SeaShipping : Shipping
{
    public SeaShipping()
    {
        Id = Guid.NewGuid();
        ShippingType = ShippingType.Sea;
    }
    public override double CalculateCost()
    {
        double baseCost = Distance * 0.02; 
        double volumeCost = Volume * 15; 
        return baseCost + volumeCost;
    }
    public override TimeSpan CalculateDuration()
    {
        double days = (Distance / 500);
        return TimeSpan.FromDays(days);
    }
}