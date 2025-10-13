using System.Security.AccessControl;
using architectureProject.Models.enums;
using architectureProject.Repository;

namespace architectureProject.Models.ShippingFactory;

public class AirShippingFactory : IShippingFactory
{
    private readonly IVehicleRepository _vehicleRepository;
    public AirShippingFactory(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public Shipping CreateShipping()
    {
        return new AirShipping();
    }

    public ShippingType Type => ShippingType.Air;
    public VehicleType VehicleType => VehicleType.CargoPlain;
    public Vehicle? TakeOptimalVehicle()
    {
        return _vehicleRepository.TakeVehicleByShipping(VehicleType);
    }
    public double GetMaxVolume() => 100;
    public double GetMaxWeight() => 10;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance >= 500;
    }
}