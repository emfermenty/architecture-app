using architectureProject.Models.enums;
using architectureProject.Models.ShippingImplimitation;

namespace architectureProject.Models;

public class TrainShipping : Shipping
{
    public TrainShipping() : base(new TrainShippingImplimentation())
    {
        Id = Guid.NewGuid();
        ShippingType = ShippingType.Train;
    }
    
    public override double CalculateCost()
    {
        double baseCost = Distance * 0.05; 
        return baseCost;
    }
    public override TimeSpan CalculateDuration()
    {
        double hours = (Distance / 60);
        return TimeSpan.FromHours(hours);
    }

}