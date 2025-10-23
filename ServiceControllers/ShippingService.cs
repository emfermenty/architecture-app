using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Models.enums;
using architectureProject.Models.Observer.Interfaces;
using architectureProject.Repository;
using architectureProject.Services;

public class ShippingService
{
    private readonly IShippingsRepository _shippingsRepository;
    private readonly ShippingOptimizer _shippingOptimizer;
    private readonly IReadOnlyDictionary<ShippingType, IShippingFactory> _factories;
    private readonly CommandHandler _commandHandler;
    private readonly IObserverManager _observerManager;

    public ShippingService(
        IShippingsRepository shippingsRepository,
        ShippingOptimizer shippingOptimizer,
        IEnumerable<IShippingFactory> factories,
        CommandHandler commandHandler,
        IObserverManager observerManager)
    {
        _shippingsRepository = shippingsRepository;
        _shippingOptimizer = shippingOptimizer;
        _factories = factories.ToDictionary(f => f.Type);
        _commandHandler = commandHandler;
        _observerManager = observerManager;
    }
    
    public async Task<object?>  CreateShippingWithVehicleAsync(CreateShippingCommand command)
    {
        var createCommand = new CreateShippingWithVehicleCommand(
            command, 
            _factories, 
            _shippingsRepository,
            _observerManager);
        
        await _commandHandler.HandleAsync(createCommand);
        return createCommand.Result;
    }

    public async Task<ShippingQuotesDto> GetOptimalShippingAsync(ShippingRequest request)
    {
        var command = new GetOptimalShippingCommand(request, _shippingOptimizer);
        await _commandHandler.HandleAsync(command);
        return command.Result;
    }

    // Методы для Undo
    public async Task UndoLastCommandAsync()
    {
        await _commandHandler.UndoAsync();
    }

    public bool CanUndo => _commandHandler.CanUndo;

    // ВАШИ СТАРЫЕ МЕТОДЫ ОСТАЮТСЯ БЕЗ ИЗМЕНЕНИЙ
    public ShippingQuotesDto GetOptimalShipping(ShippingRequest request)
    {
        var optimalShipping = _shippingOptimizer.SelectOptimalShipping(request);
        if (optimalShipping == null)
            throw new InvalidOperationException("Нет подходящих вариантов доставки.");

        Console.WriteLine($"{optimalShipping.ShippingType} + {optimalShipping.TrackingNumber} + {optimalShipping.Distance} + {optimalShipping.Id}");

        return new ShippingQuotesDto
        {
            ShippingType = optimalShipping.ShippingType,
            Cost = optimalShipping.CalculateCost(),
            Duration = optimalShipping.CalculateDuration(),
            Description = GetShippingDescription(optimalShipping.ShippingType),
        };
    }

    public List<ShippingQuote> GetShippingQuotes(ShippingRequest request)
    {
        return _shippingOptimizer.GetShippingQuotes(request);
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
        return dto;
    }

    public object? CreateShippingWithVehicle(CreateShippingCommand command)
    {
        // Старая синхронная версия для обратной совместимости
        if (!_factories.TryGetValue(command.Type, out var factory))
            throw new ArgumentException($"Неизвестный тип перевозки: {command.Type}");
 
        if (!factory.ValidateShipping(command.Distance, command.Weight, command.Volume))
            return null;

        var shipping = factory.CreateShipping();
        var vehicle = factory.TakeOptimalVehicle();
        shipping.Vehicle = vehicle;
        shipping.VehicleId = vehicle.Id;
        shipping.Duration = TimeSpan.FromHours(command.Distance / vehicle.Speed);
        shipping.Id = Guid.NewGuid();
        shipping.Cost = shipping.CalculateCost();
        shipping.TrackingNumber = Guid.NewGuid().ToString()[..8].ToUpper();
        shipping.Distance = command.Distance;
        shipping.Weight = command.Weight;
        shipping.Volume = command.Volume;
        shipping.TypeDescription = GetShippingDescription(shipping.ShippingType);
        
        _shippingsRepository.AddShippingAsync(shipping);
        return shipping;
    }

    public List<ShippingDto?> GetAllShippings()
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