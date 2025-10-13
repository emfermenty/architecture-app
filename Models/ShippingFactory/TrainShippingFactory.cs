using architectureProject.Models.enums;
using architectureProject.Repository;

namespace architectureProject.Models.ShippingFactory;

public class TrainShippingFactory : IShippingFactory
{
    private readonly IVehicleRepository _vehicleRepository;

    public TrainShippingFactory(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public Shipping CreateShipping()
    {
        return new TrainShipping();
    }
    public Vehicle? TakeOptimalVehicle()
    {
        return _vehicleRepository.TakeVehicleByShipping(VehicleType);
    }
    public VehicleType VehicleType => VehicleType.Truck;
    public ShippingType Type => ShippingType.Train;
    public double GetMaxWeight() => 50000;
    public double GetMaxVolume() => 5000;

    public bool ValidateShipping(double distance, double weight, double volume)
    {
        return weight <= GetMaxWeight() && volume <= GetMaxVolume() && distance >= 500;
    }
}