using architectureProject.Models.enums;
using architectureProject.Models.ShippingImplimitation;

namespace architectureProject.Models;

public class AirShipping : Shipping
{
    public AirShipping() : base(new AirShippingImplementation())
    {
        Id = Guid.NewGuid();
        ShippingType = ShippingType.Air;
    }
    
    public override double CalculateCost()
    {
        double baseCost = Distance * 0.25; 
        double weightCost = Weight * 2.5; 
        return baseCost + weightCost;
    }
    public override TimeSpan CalculateDuration()
    {
        double hours = (Distance / 800); 
        double additionalHours = 4; 
        return TimeSpan.FromHours(hours + additionalHours);
    }
}