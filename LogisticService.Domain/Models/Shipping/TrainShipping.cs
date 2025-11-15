using LogisticService.Domain.Models.Shipping.Implimintation;
using LogisticService.Domain.Enums;

namespace LogisticService.Domain.Models.Shipping;

public class TrainShipping : Abstract.Shipping
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