using architectureProject.Models.enums;

namespace architectureProject.DTO;

public class ShippingQuotesDto
{
    public ShippingType ShippingType { get; set; }
    public double Cost { get; set; }
    public TimeSpan Duration { get; set; }
    public string Description { get; set; }
}