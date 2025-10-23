using architectureProject.Command;
using architectureProject.Models;
using architectureProject.Models.enums;
using architectureProject.Models.Observer.Interfaces;
using architectureProject.Repository;

public class CreateShippingWithVehicleCommand : ICommand
{
    private readonly IObserverManager _observerManager;
    private readonly CreateShippingCommand _request;
    private readonly IReadOnlyDictionary<ShippingType, IShippingFactory> _factories;
    private readonly IShippingsRepository _shippingsRepository;
    
    public object? Result { get; private set; }
    private Shipping? _createdShipping;

    public CreateShippingWithVehicleCommand(
        CreateShippingCommand request,
        IReadOnlyDictionary<ShippingType, IShippingFactory> factories,
        IShippingsRepository shippingsRepository,
        IObserverManager observerManager)
    {
        _request = request;
        _factories = factories;
        _shippingsRepository = shippingsRepository;
        _observerManager = observerManager;
    }

    public Task<bool> CanExecuteAsync()
    {
        if (!_factories.TryGetValue(_request.Type, out var factory))
        {
            Console.WriteLine($"Unknown shipping type: {_request.Type}");
            return Task.FromResult(false);
        }

        return Task.FromResult(factory.ValidateShipping(_request.Distance, _request.Weight, _request.Volume));
    }

    public async Task ExecuteAsync()
    {
        if (!await CanExecuteAsync())
            throw new InvalidOperationException("Command validation failed");

        var factory = _factories[_request.Type];
        
        var shipping = factory.CreateShipping();
        var vehicle = factory.TakeOptimalVehicle();
        
        if (vehicle == null)
            throw new InvalidOperationException($"No available vehicles for {_request.Type}");
        shipping.Vehicle = vehicle;
        shipping.VehicleId = vehicle.Id;
        shipping.Duration = TimeSpan.FromHours(_request.Distance / vehicle.Speed);
        shipping.Id = Guid.NewGuid();
        shipping.Cost = shipping.CalculateCost();
        shipping.TrackingNumber = GenerateTrackingNumber();
        shipping.Distance = _request.Distance;
        shipping.Weight = _request.Weight;
        shipping.Cost = shipping.CalculateCost();
        shipping.Volume = _request.Volume;
        shipping.TypeDescription = GetShippingDescription(shipping.ShippingType);
        
        _observerManager.RegisterObservers(shipping);
        
        await _shippingsRepository.AddShippingAsync(shipping);
        
        await shipping.NotifyCreated();
        
        _createdShipping = shipping;
        Result = shipping;

        Console.WriteLine($"Created shipping {shipping.TrackingNumber} with vehicle {vehicle.Id}");
    }

    public async Task UndoAsync()
    {
        if (_createdShipping != null)
        {
            await _shippingsRepository.DeleteShippingAsync(_createdShipping.Id);
            
            Console.WriteLine($"Undo: Deleted shipping {_createdShipping.TrackingNumber}");
            Result = null;
            _createdShipping = null;
        }
    }

    private string GenerateTrackingNumber() => 
        $"TRK{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

    private string GetShippingDescription(ShippingType type) => type switch
    {
        ShippingType.Truck => "Грузовик",
        ShippingType.Sea => "По морю",
        ShippingType.Train => "Поездом", 
        ShippingType.Air => "Самолетом",
        _ => "Неизвестный тип перевозки"
    };
}