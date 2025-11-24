using gprcoptimizer.Services.Interfaces;
using Grpc.Core;
using ShippingOptimization.Grpc;

namespace gprcoptimizer.Services
{
    public class ShippingOptimizerGrpcService : ShippingOptimizerService.ShippingOptimizerServiceBase
    {
        private readonly IShippingOptimizer _optimizer;

        public ShippingOptimizerGrpcService(IShippingOptimizer optimizer)
        {
            _optimizer = optimizer;
        }

        public override Task<ShippingResponse> SelectOptimalShipping(ShippingRequest request, ServerCallContext context)
        {
            try
            {
                var internalRequest = new gprcoptimizer.DTO.ShippingRequest
                {
                    Distance = request.Distance,
                    Weight = request.Weight,
                    Volume = request.Volume
                };

                var optimalShipping = _optimizer.SelectOptimalShipping(internalRequest);

                if (optimalShipping != null)
                {
                    var response = new ShippingResponse
                    {
                        OptimalShipping = new ShippingQuote
                        {
                            Type = (ShippingType)optimalShipping.ShippingType,
                            Cost = optimalShipping.CalculateCost(),
                            Duration = new Duration
                            {
                                Days = (int)optimalShipping.CalculateDuration().TotalDays,
                                Hours = (int)optimalShipping.CalculateDuration().TotalHours % 24,
                                Minutes = (int)optimalShipping.CalculateDuration().TotalMinutes % 60
                            },
                            MaxWeight = optimalShipping.Weight,
                            MaxVolume = optimalShipping.Volume
                        }
                    };
                    return Task.FromResult(response);
                }
                else
                {
                    return Task.FromResult(new ShippingResponse
                    {
                        Error = new Error
                        {
                            Message = "No suitable shipping method found",
                            Code = "NO_SHIPPING_METHOD"
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ShippingResponse
                {
                    Error = new Error
                    {
                        Message = ex.Message,
                        Code = "INTERNAL_ERROR"
                    }
                });
            }
        }

        public override Task<ShippingQuotesResponse> GetShippingQuotes(ShippingRequest request, ServerCallContext context)
        {
            try
            {
                var internalRequest = new gprcoptimizer.DTO.ShippingRequest
                {
                    Distance = request.Distance,
                    Weight = request.Weight,
                    Volume = request.Volume
                };

                var internalQuotes = _optimizer.GetShippingQuotes(internalRequest);
                var response = new ShippingQuotesResponse();

                foreach (var internalQuote in internalQuotes)
                {
                    response.Quotes.Add(new ShippingQuote
                    {
                        Type = (ShippingType)internalQuote.Type,
                        Cost = internalQuote.Cost,
                        Duration = new Duration
                        {
                            Days = (int)internalQuote.Duration.TotalDays,
                            Hours = (int)internalQuote.Duration.TotalHours % 24,
                            Minutes = (int)internalQuote.Duration.TotalMinutes % 60
                        },
                        MaxWeight = internalQuote.MaxWeight,
                        MaxVolume = internalQuote.MaxVolume
                    });
                }

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}