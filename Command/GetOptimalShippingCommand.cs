﻿using architectureProject.Command;
using architectureProject.DTO;
using architectureProject.Models.enums;
using architectureProject.Services;

public class GetOptimalShippingCommand : ICommand
{
    private readonly ShippingRequest _request;
    private readonly ShippingOptimizer _shippingOptimizer;
    
    public ShippingQuotesDto Result { get; private set; }

    public GetOptimalShippingCommand(
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

        var optimalShipping = _shippingOptimizer.SelectOptimalShipping(_request);
        if (optimalShipping == null)
            throw new InvalidOperationException("No suitable shipping options found");

        Console.WriteLine($"{optimalShipping.ShippingType} + {optimalShipping.TrackingNumber} + {optimalShipping.Distance} + {optimalShipping.Id}");

        Result = new ShippingQuotesDto
        {
            ShippingType = optimalShipping.ShippingType,
            Cost = optimalShipping.CalculateCost(),
            Duration = optimalShipping.CalculateDuration(),
            Description = GetShippingDescription(optimalShipping.ShippingType),
        };
    }

    public Task UndoAsync()
    {
        // Для запросов отмена не требуется
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