using architectureProject.Models.Observer.Interfaces;

namespace architectureProject.Models.Observer;

public class ObserverManager : IObserverManager
{
    private readonly IEnumerable<IShippingObserver> _observers;

    public ObserverManager(IEnumerable<IShippingObserver> observers)
    {
        _observers = observers;
    }

    public void RegisterObservers(Shipping shipping)
    {
        foreach (var observer in _observers)
        {
            shipping.Attach(observer);
        }
    }
}