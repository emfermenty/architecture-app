using gprcoptimizer.Models.Enums;

namespace gprcoptimizer.DTO
{
    public class ShippingQuote
    {
        public ShippingType Type { get; set; }
        public double Cost { get; set; }
        public TimeSpan Duration { get; set; }
        public List<string> Requirements { get; set; }
        public double MaxWeight { get; set; }
        public double MaxVolume { get; set; }

        public string GetDescription()
        {
            return Type switch
            {
                ShippingType.Truck => " Дальнобойные перевозки",
                ShippingType.Sea => " Морские перевозки",
                ShippingType.Train => " Железнодорожные перевозки",
                ShippingType.Air => "✈ Авиаперевозки",
                _ => "Неизвестный тип перевозки"
            };
        }
    }
}
