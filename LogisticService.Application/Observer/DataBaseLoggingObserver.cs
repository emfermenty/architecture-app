using LogisticService.Application.Services;
using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Domain.Observer;

public class DatabaseObserver : IShippingObserver
{
    private readonly IShippingsRepository _shippingRepository;

    public DatabaseObserver(IShippingsRepository shippingRepository)
    {
        _shippingRepository = shippingRepository;
    }
    public async Task OnShippingStatusChanged(Shipping shipping, string oldStatus, string newStatus)
    {
        await _shippingRepository.ChangeStatusAsync(shipping, newStatus);
        Console.WriteLine($"📝 DB Log: {shipping.TrackingNumber} {oldStatus} TO {newStatus}");
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