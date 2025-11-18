using LogisticService.Application.Commands;
using LogisticService.Application.Commands.interfaces;
using LogisticService.Application.DTO;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;
using LogisticService.Domain.Observer;
using LogisticService.Infrastructure.Repository.Interfaces;

namespace LogisticService.Application.Services;

public class ShippingService
{
    private readonly IShippingsRepository _shippingsRepository;
    private readonly ShippingOptimizer _shippingOptimizer;
    private readonly IReadOnlyDictionary<ShippingType, IShippingFactory> _factories;
    private readonly ICommandHandler _commandHandler;
    private readonly IObserverManager _observerManager;
    private readonly IVehicleProvider _vehicleProvider;

    public ShippingService(
        IShippingsRepository shippingsRepository,
        ShippingOptimizer shippingOptimizer,
        IEnumerable<IShippingFactory> factories,
        ICommandHandler commandHandler,
        IObserverManager observerManager,
        IVehicleProvider vehicleProvider)
    {
        _shippingsRepository = shippingsRepository;
        _shippingOptimizer = shippingOptimizer;
        _factories = factories.ToDictionary(f => f.Type);
        _commandHandler = commandHandler;
        _observerManager = observerManager;
        _vehicleProvider = vehicleProvider;
    }
    
    public async Task<object?> CreateShippingWithVehicleAsync(CreateShippingCommandDTO command)
    {
        var createCommand = new CreateShippingWithVehicleCommand(
            command,
            _factories,
            _shippingsRepository,
            _observerManager,
            _vehicleProvider
        );

        await _commandHandler.HandleAsync(createCommand);
        return createCommand.Result;
    }
    
    public async Task<ShippingQuotesDto> GetOptimalShippingAsync(ShippingRequest request)
    {
        var command = new GetOptimalShippingCommand(request, _shippingOptimizer);
        await _commandHandler.HandleAsync(command);
        return command.Result;
    }
    
    public async Task UndoLastCommandAsync() => await _commandHandler.UndoAsync();
    public bool CanUndo => _commandHandler.CanUndo;
    
    public ShippingQuotesDto GetOptimalShipping(ShippingRequest request)
    {
        var optimalShipping = _shippingOptimizer.SelectOptimalShipping(request)
            ?? throw new InvalidOperationException("Нет подходящих вариантов доставки.");

        return new ShippingQuotesDto
        {
            ShippingType = optimalShipping.ShippingType,
            Cost = optimalShipping.CalculateCost(),
            Duration = optimalShipping.CalculateDuration(),
            Description = GetShippingDescription(optimalShipping.ShippingType),
        };
    }

    public List<ShippingQuote> GetShippingQuotes(ShippingRequest request)
        => _shippingOptimizer.GetShippingQuotes(request);

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

        return new ShippingDto
        {
            Id = shipping.Id,
            TrackingNumber = shipping.Id.ToString()[..8],
            ShippingType = shipping.ShippingType,
            Distance = shipping.Distance,
            Weight = shipping.Weight,
            Volume = shipping.Volume,
            Cost = shipping.CalculateCost()
        };
    }

    public async Task<List<Shipping>?> GetAllShippings() => await _shippingsRepository.GetAllShippingsAsync();

    public async Task<Shipping?> GetByTrackingNumberAsync(string trackingNumber)
    {
        return await _shippingsRepository.GetByTrackingNumberAsync(trackingNumber);
    }
    
    private string GetShippingDescription(ShippingType type) => type switch
    {
        ShippingType.Truck => "Грузовик",
        ShippingType.Sea => "По морю",
        ShippingType.Train => "Поездом",
        ShippingType.Air => "Самолетом",
        _ => "Неизвестный тип перевозки"
    };
}
