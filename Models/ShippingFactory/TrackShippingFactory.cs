using architectureProject.Models.enums;
using architectureProject.Repository;

namespace architectureProject.Models.ShippingFactory;

public class TrackShippingFactory : IShippingFactory
{
    private readonly IVehicleRepository _vehicleRepository;

    public TrackShippingFactory(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public Shipping CreateShipping()
    {
        return new TruckShipping();
    }
    public Vehicle? TakeOptimalVehicle()
    {
        return _vehicleRepository.TakeVehicleByShipping(VehicleType);
    }
    public VehicleType VehicleType => VehicleType.Truck;
    public ShippingType Type => ShippingType.Truck;
    public double GetMaxWeight() => 25000;
    public double GetMaxVolume() => 90;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() &&
               volume <= GetMaxVolume() &&
               distance <= 5000;
    }
}