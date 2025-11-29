using DomainShippingType = LogisticService.Domain.Enums.ShippingType;
using GrpcShippingType = ShippingOptimization.Grpc.ShippingType;
using DomainQuote = LogisticService.Application.DTO.ShippingQuote;

namespace LogisticService.Infrastructure.External;

public static class GrpcShippingMapper
{
    
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
