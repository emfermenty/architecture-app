using LogisticService.Application.Commands.interfaces;
using LogisticService.Application.DTO;
using LogisticService.Application.Services;

namespace LogisticService.Application.Commands;

public class GetAllShippingQuotesCommand : ICommand
{
    private readonly ShippingRequest _request;
    private readonly IShippingOptimizer _shippingOptimizer;
    
    public List<ShippingQuote> Result { get; private set; } = new();

    public GetAllShippingQuotesCommand(
        ShippingRequest request,
        IShippingOptimizer shippingOptimizer)
    {
        _request = request;
        _shippingOptimizer = shippingOptimizer;
    }

    public Task<bool> CanExecuteAsync()
    {
        return Task.FromResult(_request.Distance > 0 && _request.Weight > 0 && _request.Volume > 0);
    }

    public async Task ExecuteAsync()
    {
        if (!await CanExecuteAsync())
            throw new InvalidOperationException("Invalid shipping request");

        Result = _shippingOptimizer.GetShippingQuotes(_request);
    }

    public Task UndoAsync()
    {
        Result.Clear();
        return Task.CompletedTask;
    }
}