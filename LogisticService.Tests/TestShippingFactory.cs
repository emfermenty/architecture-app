// LogisticService.Application.Tests/TestDoubles/TestShippingFactory.cs
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.Implimintation.Interfaces;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;

namespace LogisticService.Application.Tests.TestDoubles;

public class TestShippingFactory : IShippingFactory
{
    private readonly ShippingType _type;
    private readonly bool _isValid;
    private readonly IShippingImplementation _implementation;
    private readonly double _maxWeight;
    private readonly double _maxVolume;
    private readonly double _maxDistance;

    public TestShippingFactory(
        ShippingType type,
        bool isValid = true,
        IShippingImplementation? implementation = null,
        double maxWeight = 1000,
        double maxVolume = 100,
        double maxDistance = 10000)
    {
        _type = type;
        _isValid = isValid;
        _implementation = implementation ?? new TestShippingImplementation();
        _maxWeight = maxWeight;
        _maxVolume = maxVolume;
        _maxDistance = maxDistance;
    }

    public ShippingType Type => _type;

    public Shipping CreateShipping()
    {
        return new TestShipping(_implementation);
    }

    public double GetMaxVolume() => _maxVolume;
    public VehicleType VehicleType { get; }

    public double GetMaxWeight() => _maxWeight;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        if (!_isValid)
            return false;
        
        return distance > 0 && distance <= _maxDistance &&
               weight > 0 && weight <= _maxWeight &&
               volume > 0 && volume <= _maxVolume;
    }
}