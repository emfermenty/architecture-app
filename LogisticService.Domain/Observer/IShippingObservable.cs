namespace LogisticService.Domain.Observer;

public interface IShippingObservable
{
    void Attach(IShippingObserver observer);
    void Detach(IShippingObserver observer);
    Task NotifyStatusChanged(string oldStatus, string newStatus);
    Task NotifyCreated();
    Task NotifyCompleted();
}