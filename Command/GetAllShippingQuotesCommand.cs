﻿using architectureProject.DTO;
using architectureProject.Services;

namespace architectureProject.Command;

public class GetAllShippingQuotesCommand : ICommand
{
    private readonly ShippingRequest _request;
    private readonly ShippingOptimizer _shippingOptimizer;
    
    public List<ShippingQuote> Result { get; private set; } = new();

    public GetAllShippingQuotesCommand(
        ShippingRequest request,
        ShippingOptimizer shippingOptimizer)
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