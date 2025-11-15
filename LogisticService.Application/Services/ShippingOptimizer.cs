using LogisticService.Application.DTO;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;

namespace LogisticService.Application.Services;

public class ShippingOptimizer
{
    private readonly Dictionary<ShippingType, IShippingFactory> _factories;

    public ShippingOptimizer(IEnumerable<IShippingFactory> factories)
    {
        _factories = factories.ToDictionary(f => f.Type);
    }
    public Shipping? SelectOptimalShipping(ShippingRequest request)
    {
        var availableOptions = new List<Shipping>();
        
        foreach (var factory in _factories.Values)
        {
            if (!factory.ValidateShipping(request.Distance, request.Weight, request.Volume))
                continue;

            var shipping = factory.CreateShipping();
            shipping.Distance = request.Distance;
            shipping.Weight = request.Weight;
            shipping.Volume = request.Volume;

            availableOptions.Add(shipping);
        }
        
        return availableOptions.OrderBy(s => 
            s.CalculateCost() * 0.6 + 
            s.CalculateDuration().TotalHours * 10 * 0.3
        ).FirstOrDefault();
    }
    public List<ShippingQuote> GetShippingQuotes(ShippingRequest request)
    {
        var quotes = new List<ShippingQuote>();

        foreach (var (type, factory) in _factories)
        {
            if (factory.ValidateShipping(request.Distance, request.Weight, request.Volume))
            {
                var shipping = factory.CreateShipping();
                shipping.Distance = request.Distance;
                shipping.Weight = request.Weight;
                shipping.Volume = request.Volume;

                quotes.Add(new ShippingQuote
                {
                    Type = type,
                    Cost = shipping.CalculateCost(),
                    Duration = shipping.CalculateDuration(),
                    MaxWeight = factory.GetMaxWeight(),
                    MaxVolume = factory.GetMaxVolume()
                });
            }
        }

        return quotes.OrderBy(q => q.Cost).ToList();
    }
}