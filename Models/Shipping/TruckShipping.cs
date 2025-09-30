using architectureProject.Models.enums;

namespace architectureProject.Models;

public class TruckShipping : Shipping
{
    public TruckShipping()
    {
        Id = Guid.NewGuid();
        ShippingType = ShippingType.Truck;
    } 
    
    public override double CalculateCost()
    {
        double baseCost = Distance * 0.12; 
        double weightCost = Weight * 0.08;
        return baseCost + weightCost;
    }
    public override TimeSpan CalculateDuration()
    {
        double hours = (Distance / 80); 
        return TimeSpan.FromHours(hours);
    }
    
}