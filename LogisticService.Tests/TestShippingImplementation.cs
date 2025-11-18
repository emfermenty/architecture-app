// LogisticService.Application.Tests/TestDoubles/TestShippingImplementation.cs
using LogisticService.Domain.Models.Shipping.Implimintation.Interfaces;

namespace LogisticService.Application.Tests.TestDoubles;

public class TestShippingImplementation : IShippingImplementation
{
    private readonly double _costMultiplier;
    private readonly double _speed;

    public TestShippingImplementation(double costMultiplier = 1.0, double speed = 50.0)
    {
        _costMultiplier = costMultiplier;
        _speed = speed;
    }

    public double CalculateCost(double distance, double weight, double volume)
    {
        return distance * weight * 0.1 * _costMultiplier;
    }

    public TimeSpan CalculateDuration(double distance)
    {
        return TimeSpan.FromHours(distance / _speed);
    }
}