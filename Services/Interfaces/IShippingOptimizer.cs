using gprcoptimizer.DTO;
using gprcoptimizer.Models.Shipping.Abstract;

namespace gprcoptimizer.Services.Interfaces
{
    public interface IShippingOptimizer
    {
        Shipping? SelectOptimalShipping(ShippingRequest request);
        List<ShippingQuote> GetShippingQuotes(ShippingRequest request);
    }
}
