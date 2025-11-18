// LogisticService.Application.Tests/TestDoubles/TestShipping.cs
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.Implimintation.Interfaces;
using Moq;

namespace LogisticService.Application.Tests.TestDoubles;

public class TestShipping : Shipping
{
    // Конструктор принимает имплементацию, как базовый класс
    public TestShipping(IShippingImplementation implementation) : base(implementation)
    {
        // Можно задать дефолтные значения
        ShippingType = ShippingType.Truck;
    }

    // Переопределяем методы, чтобы тесты не зависели от реальной реализации
    public override double CalculateCost() => 123;
    public override TimeSpan CalculateDuration() => TimeSpan.FromHours(10);
}