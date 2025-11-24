using gprcoptimizer.Models.Enums;
using gprcoptimizer.Models.Shipping.Implimintation.Interfaces;

namespace gprcoptimizer.Models.Shipping.Abstract
{
    public abstract class Shipping
    {
        protected readonly IShippingImplementation implementation;
        public Guid Id { get; set; }
        public DateTime? StartShipping { get; private set; } = DateTime.UtcNow;
        public string TrackingNumber { get; set; } = null!;
        public double Distance { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public ShippingType ShippingType { get; set; }
        public double Cost { get; set; }
        public TimeSpan Duration { get; set; }
        public virtual double CalculateCost()
        {
            return implementation.CalculateCost(Distance, Weight, Volume);
        }

        public virtual TimeSpan CalculateDuration()
        {
            return implementation.CalculateDuration(Distance);
        }
        protected Shipping(IShippingImplementation implementation)
        {
            this.implementation = implementation;
            Id = Guid.NewGuid();
        }
    }
}
