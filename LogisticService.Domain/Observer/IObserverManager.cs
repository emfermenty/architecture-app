using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Domain.Observer;

public interface IObserverManager
{
    void RegisterObservers(Shipping shipping);
    int GetObserverCount(); 
    List<string> GetObserverNames(); 
}