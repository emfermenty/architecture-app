using ShippingOptimization.Grpc;
using LogisticService.Application.Services;
using LogisticService.Domain.Models.Shipping.Abstract;
using ShippingQuote = LogisticService.Application.DTO.ShippingQuote;

using ShippingRequest = LogisticService.Application.DTO.ShippingRequest;

namespace LogisticService.Infrastructure.External;

public class GrpcShippingOptimizer : IShippingOptimizer
{
    private readonly ShippingOptimizerService.ShippingOptimizerServiceClient _client;
    
    public GrpcShippingOptimizer(ShippingOptimizerService.ShippingOptimizerServiceClient client)
    {
        _client = client;
    }

    public ShippingQuote? SelectOptimalShipping(ShippingRequest request)
    {
        try
        {
            var grpcRequest = new ShippingOptimization.Grpc.ShippingRequest 
            {
                Distance = request.Distance,
                Weight = request.Weight,
                Volume = request.Volume
            };

            var response = _client.SelectOptimalShipping(grpcRequest);
            
            if (response.ResultCase == global::ShippingOptimization.Grpc.ShippingResponse.ResultOneofCase.OptimalShipping)
            {
                return GrpcShippingMapper.ToDomainQuote(response.OptimalShipping);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"gRPC call failed: {ex.Message}", ex);
        }
    }
    public List<ShippingQuote> GetShippingQuotes(ShippingRequest request)
    {
        try
        {
            var grpcRequest = new global::ShippingOptimization.Grpc.ShippingRequest 
            {
                Distance = request.Distance,
                Weight = request.Weight,
                Volume = request.Volume
            };

            var response = _client.GetShippingQuotes(grpcRequest);
            
            return response.Quotes.Select(GrpcShippingMapper.ToDomainQuote).ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"gRPC call failed: {ex.Message}", ex);
        }
    }
}