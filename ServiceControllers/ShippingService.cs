using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Models.enums;
using architectureProject.Models.ShippingFactory;
using architectureProject.Repository;
using architectureProject.Services;

namespace architectureProject.ServiceControllers;

public class ShippingService
{
    private readonly ShippingsRepository _shippingsRepository;
    private readonly ShippingOptimizer _shippingOptimizer;

    public ShippingService(ShippingsRepository shippingsRepository, ShippingOptimizer shippingOptimizer)
    {
        _shippingsRepository = shippingsRepository;
        _shippingOptimizer = shippingOptimizer;
    }

    public ShippingQuotesDto GetOptimalShipping(ShippingRequest request)
    {
        var optimalShipping = _shippingOptimizer.SelectOptimalShipping(request);
        Console.WriteLine($"{optimalShipping.ShippingType} + {optimalShipping.TrackingNumber} + {optimalShipping.Distance} + {optimalShipping.Id}");
            
        var result = new ShippingQuotesDto
        {
            ShippingType = optimalShipping.ShippingType,
            Cost = optimalShipping.CalculateCost(),
            Duration = optimalShipping.CalculateDuration(),
            Description = GetShippingDescription(optimalShipping.ShippingType),
        };
        return result;
    }
    private string GetShippingDescription(ShippingType type)
    {
        return type switch
        {
            ShippingType.Truck => "Грузовик",
            ShippingType.Sea => "По морю",
            ShippingType.Train => "Поездом",
            ShippingType.Air => "Самолетом",
            _ => "Неизвестный тип перевозки"
        };
    }

    public ShippingDto? CreateShipping(CreateShippingCommand command)
    {
        IShippingFactory factory = command.Type switch
        {
            ShippingType.Truck => new TrackShippingFactory(),
            ShippingType.Sea => new SeaShippingFactory(),
            ShippingType.Train => new TrainShippingFactory(),
            ShippingType.Air => new AirShippingFactory(),
            _ => throw new ArgumentException("Неизвестный тип перевозки")
        };
        if (!factory.ValidateShipping(command.Distance, command.Weight, command.Volume))
            return null;
        var shipping = factory.CreateShipping();
        shipping.Distance = command.Distance;
        shipping.Weight = command.Weight;
        shipping.Volume = command.Volume;
        var result = new ShippingDto
        {
            Id = shipping.Id,
            TrackingNumber = shipping.Id.ToString().Substring(0, 8),
            ShippingType = shipping.ShippingType,
            Distance = shipping.Distance,
            Weight = shipping.Weight,
            Volume = shipping.Volume,
            Cost = shipping.CalculateCost()
        };
        return result;
    }
}