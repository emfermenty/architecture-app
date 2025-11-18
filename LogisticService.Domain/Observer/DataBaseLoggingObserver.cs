using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Domain.Observer;

public class DatabaseLoggingObserver : IShippingObserver
{
    public async Task OnShippingStatusChanged(Shipping shipping, string oldStatus, string newStatus)
    {
        //await _shippingsRepository.LogStatusChangeAsync(shipping.Id, oldStatus, newStatus);
        Console.WriteLine($"📝 DB Log: {shipping.TrackingNumber} {oldStatus} → {newStatus}");
    }

    public async Task OnShippingCreated(Shipping shipping)
    {
        //await _shippingsRepository.LogShippingCreationAsync(shipping);
        Console.WriteLine($"📝 DB Log: New shipping {shipping.TrackingNumber} created");
    }

    public async Task OnShippingCompleted(Shipping shipping)
    {
        //await _shippingsRepository.MarkAsCompletedAsync(shipping.Id);
        Console.WriteLine($"📝 DB Log: Shipping {shipping.TrackingNumber} completed");
    }
    
}