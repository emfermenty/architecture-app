using LogisticService.Domain.Models.Shipping;
using LogisticService.Domain.Models.Shipping.Abstract;
using ShippingOptimization.Grpc;
using DomainShippingType = LogisticService.Domain.Enums.ShippingType;
using GrpcShippingType = ShippingOptimization.Grpc.ShippingType;
using DomainQuote = LogisticService.Application.DTO.ShippingQuote;

namespace LogisticService.Infrastructure.External;

public static class GrpcShippingMapper
{
    public static Shipping ToDomain(ShippingOptimization.Grpc.ShippingQuote grpc)
    {
        var shipping = Create(MapToDomainShippingType(grpc.Type));

        shipping.Cost = grpc.Cost;

        shipping.Duration = grpc.Duration != null
            ? new TimeSpan(
                (int)grpc.Duration.Days,
                (int)grpc.Duration.Hours,
                (int)grpc.Duration.Minutes,
                (int)grpc.Duration.Seconds,
                (int)grpc.Duration.Milliseconds)
            : TimeSpan.Zero;

        shipping.Weight = grpc.MaxWeight;
        shipping.Volume = grpc.MaxVolume;
        shipping.TypeDescription = grpc.Description;

        return shipping;
    }

    public static DomainQuote ToDomainQuote(global::ShippingOptimization.Grpc.ShippingQuote grpcQuote)
    {
        return new DomainQuote
        {
            Type = MapToDomainShippingType(grpcQuote.Type),
            Cost = grpcQuote.Cost,
            Duration = grpcQuote.Duration != null
                ? new TimeSpan(
                    (int)grpcQuote.Duration.Days,
                    (int)grpcQuote.Duration.Hours,
                    (int)grpcQuote.Duration.Minutes,
                    (int)grpcQuote.Duration.Seconds,
                    (int)grpcQuote.Duration.Milliseconds)
                : TimeSpan.Zero,
            MaxWeight = grpcQuote.MaxWeight,
            MaxVolume = grpcQuote.MaxVolume,
            Requirements = grpcQuote.Requirements?.ToList() ?? new List<string>()
        };
    }

    public static Shipping Create(DomainShippingType type)
    {
        return type switch
        {
            DomainShippingType.Air   => new AirShipping(),
            DomainShippingType.Sea   => new SeaShipping(),
            DomainShippingType.Train => new TrainShipping(),
            DomainShippingType.Truck => new TruckShipping(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    public static DomainShippingType MapToDomainShippingType(GrpcShippingType grpcType)
    {
        return grpcType switch
        {
            GrpcShippingType.Truck => DomainShippingType.Truck,
            GrpcShippingType.Sea   => DomainShippingType.Sea,
            GrpcShippingType.Train => DomainShippingType.Train,
            GrpcShippingType.Air   => DomainShippingType.Air,
            _                      => DomainShippingType.Truck
        };
    }
}
