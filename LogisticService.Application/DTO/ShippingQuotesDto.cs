using LogisticService.Domain.Enums;

namespace LogisticService.Application.DTO;

public class ShippingQuotesDto
{
    public ShippingType ShippingType { get; set; }
    public double Cost { get; set; }
    public TimeSpan Duration { get; set; }
    public string Description { get; set; }
}