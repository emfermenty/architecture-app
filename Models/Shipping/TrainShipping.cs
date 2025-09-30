using architectureProject.Models.enums;

namespace architectureProject.Models;

public class TrainShipping : Shipping
{
    public TrainShipping()
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