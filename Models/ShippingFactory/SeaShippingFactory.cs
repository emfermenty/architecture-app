using architectureProject.Models.enums;
using architectureProject.Repository;

namespace architectureProject.Models.ShippingFactory;

public class SeaShippingFactory : IShippingFactory
{
    private readonly IVehicleRepository _vehicleRepository;

    public SeaShippingFactory(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public Shipping CreateShipping()
    {
        return new SeaShipping();
    }
    
    public Vehicle? TakeOptimalVehicle()
    {
        return _vehicleRepository.TakeVehicleByShipping(VehicleType);
    }

    public ShippingType Type => ShippingType.Sea;
    public VehicleType VehicleType => VehicleType.CargoShip;
    public double GetMaxWeight() => 100000;
    public double GetMaxVolume() => 1000;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance <= 5000;
    }
}