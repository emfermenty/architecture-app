using LogisticService.Application.DTO;

namespace LogisticService.Application.Services;

public interface IShippingOptimizer
{
    ShippingQuote? SelectOptimalShipping(ShippingRequest request);
    List<ShippingQuote> GetShippingQuotes(ShippingRequest request);
}