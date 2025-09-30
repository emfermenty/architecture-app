using architectureProject.Models.enums;

namespace architectureProject.Models
{
    public abstract class Vehicle
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public double MaxWeight { get; set; }
        public double MaxVolume { get; set; }
        public double Speed { get; set; } 
        public double FuelConsumption { get; set; } 
        public VehicleType VehicleType { get; set; }
        public abstract double CalculateOperatingCost(double distance);
        public abstract string GetVehicleInfo();
        
        public List<Shipping> Shippings { get; set; } = new List<Shipping>();
    }
}