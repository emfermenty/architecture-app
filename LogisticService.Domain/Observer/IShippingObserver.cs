using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Domain.Observer;

public interface IShippingObserver
{
    Task OnShippingStatusChanged(Shipping shipping, string oldStatus, string newStatus);
    Task OnShippingCreated(Shipping shipping);
    Task OnShippingCompleted(Shipping shipping);
}