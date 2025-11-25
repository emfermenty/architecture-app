using LogisticService.Application.Commands.interfaces;
using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using LogisticService.Domain.Enums;

namespace LogisticService.Application.Commands;

public class GetOptimalShippingCommand : ICommand
{
    private readonly ShippingRequest _request;
    private readonly IShippingOptimizer _shippingOptimizer;
    
    public ShippingQuote Result { get; internal set; }

    public GetOptimalShippingCommand(
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

        var optimalShipping = _shippingOptimizer.SelectOptimalShipping(_request);
        if (optimalShipping == null)
            throw new InvalidOperationException("No suitable shipping options found");

        Result = optimalShipping;
    }

    public Task UndoAsync()
    {
        Result = null;
        return Task.CompletedTask;
    }

    private string GetShippingDescription(ShippingType type) => type switch
    {
        ShippingType.Truck => "Грузовик",
        ShippingType.Sea => "По морю",
        ShippingType.Train => "Поездом",
        ShippingType.Air => "Самолетом",
        _ => "Неизвестный тип перевозки"
    };
}