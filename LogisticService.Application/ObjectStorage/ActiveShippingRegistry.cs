using System.Collections.Concurrent;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;

namespace LogisticService.Application.ObjectStorage;

public class ActiveShippingRegistry
{
    private readonly ConcurrentDictionary<string, Shipping> _active = new();

    public Shipping? Get(string trackingNumber)
        => _active.TryGetValue(trackingNumber, out var s) ? s : null;

    public void Add(Shipping shipping)
    {
        if (shipping.Status != ShippingStatus.Cancelled.ToString() ||
            shipping.Status != ShippingStatus.Delivered.ToString())
        {
            _active[shipping.TrackingNumber] = shipping;
        }
    }

    public void Remove(Shipping shipping)
    {
        _active.TryRemove(shipping.TrackingNumber, out _);
    }
}