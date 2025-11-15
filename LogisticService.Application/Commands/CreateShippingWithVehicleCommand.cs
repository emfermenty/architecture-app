using LogisticService.Application.Commands.interfaces;
using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;
using LogisticService.Domain.Models.Vehicle.Abstract;
using LogisticService.Domain.Observer;
using LogisticService.Infrastructure.Repository.Interfaces;

public class CreateShippingWithVehicleCommand : ICommand
{
    private readonly CreateShippingCommandDTO _request;
    private readonly IReadOnlyDictionary<ShippingType, IShippingFactory> _factories;
    private readonly IShippingsRepository _shippingsRepository;
    private readonly IObserverManager _observerManager;
    private readonly IVehicleProvider _vehicleProvider;

    public object? Result { get; private set; }

    public CreateShippingWithVehicleCommand(
        CreateShippingCommandDTO request,
        IReadOnlyDictionary<ShippingType, IShippingFactory> factories,
        IShippingsRepository shippingsRepository,
        IObserverManager observerManager,
        IVehicleProvider vehicleProvider)
    {
        _request = request;
        _factories = factories;
        _shippingsRepository = shippingsRepository;
        _observerManager = observerManager;
        _vehicleProvider = vehicleProvider;
    }

    public Task<bool> CanExecuteAsync()
    {
        if (!_factories.ContainsKey(_request.Type))
            return Task.FromResult(false);

        return Task.FromResult(_request.Distance > 0 && _request.Weight > 0 && _request.Volume > 0);
    }

    public async Task ExecuteAsync()
    {
        if (!await CanExecuteAsync())
            throw new InvalidOperationException("Command validation failed");
        
        var vehicle = await _vehicleProvider.GetOptimalVehicleAsync(
            MapShippingToVehicle(_request.Type), _request.Distance, _request.Weight, _request.Volume
        );
        if (vehicle == null)
            throw new InvalidOperationException($"No available vehicles for {_request.Type}");

        var factory = _factories[_request.Type];
        var shipping = BuildShipping(factory, vehicle);
        RegisterObservers(shipping);
        await SaveAndNotifyAsync(shipping);

        Result = shipping;
        Console.WriteLine($"Created shipping {shipping.TrackingNumber} with vehicle {vehicle.Id}");
    }

    public Task UndoAsync()
    {
        Result = null;
        return Task.CompletedTask;
    }

    private Shipping BuildShipping(IShippingFactory factory, Vehicle vehicle)
    {
        var shipping = factory.CreateShipping();
        shipping.Vehicle = vehicle;
        shipping.VehicleId = vehicle.Id;
        shipping.Id = Guid.NewGuid();
        shipping.Distance = _request.Distance;
        shipping.Weight = _request.Weight;
        shipping.Volume = _request.Volume;
        shipping.Duration = TimeSpan.FromHours(_request.Distance / vehicle.Speed);
        shipping.Cost = shipping.CalculateCost();
        shipping.TrackingNumber = $"TRK{Guid.NewGuid():N8}".ToUpper();
        shipping.TypeDescription = GetShippingDescription(shipping.ShippingType);
        return shipping;
    }

    private void RegisterObservers(Shipping shipping) => _observerManager.RegisterObservers(shipping);

    private async Task SaveAndNotifyAsync(Shipping shipping)
    {
        await _shippingsRepository.AddShippingAsync(shipping);
        await shipping.NotifyCreated();
    }

    private string GetShippingDescription(ShippingType type) => type switch
    {
        ShippingType.Truck => "Грузовик",
        ShippingType.Sea => "По морю",
        ShippingType.Train => "Поездом",
        ShippingType.Air => "Самолетом",
        _ => "Неизвестный тип перевозки"
    };
    private VehicleType MapShippingToVehicle(ShippingType shippingType) => shippingType switch
    {
        ShippingType.Truck => VehicleType.Truck,
        ShippingType.Sea => VehicleType.CargoShip,
        ShippingType.Train => VehicleType.FreightTrain,
        ShippingType.Air => VehicleType.CargoPlain,
        _ => throw new ArgumentOutOfRangeException(nameof(shippingType), "Unknown shipping type")
    };
}
