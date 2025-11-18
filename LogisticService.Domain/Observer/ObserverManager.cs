using LogisticService.Domain.Models.Shipping.Abstract;
using Microsoft.Extensions.Logging;

namespace LogisticService.Domain.Observer;

public class ObserverManager : IObserverManager
{
    private readonly IEnumerable<IShippingObserver> _observers;
    private readonly ILogger<ObserverManager> _logger;

    public ObserverManager(IEnumerable<IShippingObserver> observers, ILogger<ObserverManager> logger)
    {
        _observers = observers;
        _logger = logger;
    }

    public void RegisterObservers(Shipping shipping)
    {
        foreach (var observer in _observers)
        {
            shipping.Attach(observer);
        }
    }

    public int GetObserverCount() => _observers.Count();
    
    public List<string> GetObserverNames() => _observers.Select(o => o.GetType().Name).ToList();
}