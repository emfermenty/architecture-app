using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Models.enums;
using architectureProject.Models.ShippingFactory;
using architectureProject.Repository;
using architectureProject.ServiceControllers;
using architectureProject.Services;

namespace architectureProject.Facades;

public class LogisticsFacade
{
    private readonly IShippingsRepository _shippingsRepository;
    private readonly ShippingOptimizer _shippingOptimizer;
    private readonly VehicleService _vehicleService;
    private readonly IReadOnlyDictionary<ShippingType, IShippingFactory> _factories;

    public LogisticsFacade(
        IShippingsRepository shippingsRepository,
        ShippingOptimizer shippingOptimizer,
        VehicleService vehicleService,
        IEnumerable<IShippingFactory> factories)
    {
        _shippingsRepository = shippingsRepository;
        _shippingOptimizer = shippingOptimizer;
        _vehicleService = vehicleService;
        _factories = factories.ToDictionary(f => f.Type);
    }
    
    public ShippingQuotesDto GetOptimalShipping(ShippingRequest request)
    {
        var optimalShipping = _shippingOptimizer.SelectOptimalShipping(request);
        if (optimalShipping == null)
            throw new InvalidOperationException("Нет подходящих вариантов доставки.");

        return new ShippingQuotesDto
        {
            ShippingType = optimalShipping.ShippingType,
            Cost = optimalShipping.CalculateCost(),
            Duration = optimalShipping.CalculateDuration(),
            Description = GetShippingDescription(optimalShipping.ShippingType)
        };
    }
    
    public ShippingDto? CreateShipping(CreateShippingCommand command)
    {
        if (!_factories.TryGetValue(command.Type, out var factory))
            throw new ArgumentException($"Неизвестный тип перевозки: {command.Type}");

        if (!factory.ValidateShipping(command.Distance, command.Weight, command.Volume))
            return null;

        var shipping = factory.CreateShipping();
        shipping.Distance = command.Distance;
        shipping.Weight = command.Weight;
        shipping.Volume = command.Volume;

        var dto = new ShippingDto
        {
            Id = shipping.Id,
            TrackingNumber = shipping.Id.ToString()[..8],
            ShippingType = shipping.ShippingType,
            Distance = shipping.Distance,
            Weight = shipping.Weight,
            Volume = shipping.Volume,
            Cost = shipping.CalculateCost()
        };

        _shippingsRepository.AddShippingAsync(shipping); 
        return dto;
    }
    
    public object? CreateShippingWithVehicleAsync(CreateShippingCommand command)
    {
        if (!_factories.TryGetValue(command.Type, out var factory))
            throw new ArgumentException($"Неизвестный тип перевозки: {command.Type}");

        if (!factory.ValidateShipping(command.Distance, command.Weight, command.Volume))
            throw new InvalidOperationException("Параметры недопустимы для выбранного типа перевозки.");

        var shipping = factory.CreateShipping();
        var vehicle = factory.TakeOptimalVehicle() ?? throw new InvalidOperationException("Нет доступного транспорта.");

        shipping.Vehicle = vehicle;
        shipping.VehicleId = vehicle.Id;
        shipping.Distance = command.Distance;
        shipping.Weight = command.Weight;
        shipping.Volume = command.Volume;
        shipping.Duration = TimeSpan.FromHours(command.Distance / vehicle.Speed);
        shipping.Cost = shipping.CalculateCost();
        shipping.TrackingNumber = Guid.NewGuid().ToString()[..8].ToUpper();
        shipping.TypeDescription = GetShippingDescription(shipping.ShippingType);
        shipping.Id = Guid.NewGuid();

        _shippingsRepository.AddShippingAsync(shipping);

        return shipping;
    }
    
    public List<Shipping> GetAllShippingsAsync()
    {
        return _shippingsRepository.GetAllShippingsAsync();
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
}
