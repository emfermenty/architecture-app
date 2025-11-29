using LogisticService.Domain.Enums;

namespace LogisticService.Application.DTO;

public class ChangeStatusDTO
{
    public string trackingNumber { get; set; }
    public ShippingStatus shippingStatus {get;set;}
}