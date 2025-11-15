using LogisticService.Domain.Enums;

namespace LogisticService.Application.DTO;

public class CreateShippingCommandDTO
{
    public ShippingType Type { get; set; }
    public double Distance { get; set; }
    public double Weight { get; set; }
    public double Volume { get; set; }
}