namespace architectureProject.Models.Observer.Interfaces;

public interface IShippingObserver
{
    Task OnShippingStatusChanged(Shipping shipping, string oldStatus, string newStatus);
    Task OnShippingCreated(Shipping shipping);
    Task OnShippingCompleted(Shipping shipping);
}