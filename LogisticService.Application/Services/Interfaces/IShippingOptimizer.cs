using LogisticService.Application.DTO;
using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Application.Services;

public interface IShippingOptimizer
{
    Shipping? SelectOptimalShipping(ShippingRequest request);
    List<ShippingQuote> GetShippingQuotes(ShippingRequest request);
}