
 using architectureProject.Models.enums;

 public class CreateShippingCommand
{
    public ShippingType Type { get; set; }
    public double Distance { get; set; }
    public double Weight { get; set; }
    public double Volume { get; set; }
}