using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.Models.enums;
using architectureProject.Models.ShippingFactory;
using architectureProject.ServiceControllers;
using architectureProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace architectureProject.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class ShippingController : ControllerBase
{
    private readonly ShippingOptimizer _shippingOptimizer;
    private readonly ShippingService _shippingService;

    public ShippingController(ShippingOptimizer shippingOptimizer, ShippingService shippingService)
    {
        _shippingOptimizer = shippingOptimizer;
        _shippingService = shippingService;
    }

    [HttpPost("quotes")]
    public IActionResult GetShippingQuotes([FromBody] ShippingRequest request)
    {
        var quotes = _shippingOptimizer.GetShippingQuotes(request);
        return Ok(quotes);
    }

    [HttpPost("optimize")]
    public IActionResult GetOptimalShipping([FromBody] ShippingRequest request)
    {
        var optimalShipping = _shippingService.GetOptimalShipping(request);
        if (optimalShipping == null)
            return BadRequest("Нет подходящих способов перевозки для данных параметров");
        return Ok(optimalShipping);
    }
    
    [HttpPost("create")]
    public IActionResult CreateShipping([FromBody] CreateShippingCommand command)
    {
        var result = _shippingService.CreateShipping(command);
        if (result == null)
        {
            return BadRequest("Не возможно создать логистику");
        }
        return Ok(_shippingService.CreateShipping(command));
    }
    
    [HttpPost("create-with-vetching")]
    public IActionResult CreateShippingWithVetching([FromBody] CreateShippingCommand command)
    {
        IShippingFactory factory = command.Type switch
        {
            ShippingType.Truck => new TrackShippingFactory(),
            ShippingType.Sea => new SeaShippingFactory(),
            ShippingType.Train => new TrainShippingFactory(),
            ShippingType.Air => new AirShippingFactory(),
            _ => throw new ArgumentException("Неизвестный тип перевозки")
        };
        
        if (!factory.ValidateShipping(command.Distance, command.Weight, command.Volume))
            return BadRequest("Данные параметры недопустимы для выбранного типа перевозки");
        
        var shipping = factory.CreateShipping();
        var vehicle = factory.CreateVehicle();
    
        shipping.Id = Guid.NewGuid();
        shipping.TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        shipping.Distance = command.Distance;
        shipping.Weight = command.Weight;
        shipping.Volume = command.Volume;
        
        return Ok(new { 
            Id = shipping.Id,
            TrackingNumber = shipping.TrackingNumber,
            ShippingType = shipping.ShippingType,
            TypeDescription = GetShippingDescription(shipping.ShippingType),
            Distance = shipping.Distance,
            Weight = shipping.Weight,
            Volume = shipping.Volume,
            Cost = shipping.CalculateCost(),
            Duration = shipping.CalculateDuration(),
            
            Vehicle = new {
                Id = vehicle.Id,
                Model = vehicle.Model,
                VehicleType = vehicle.VehicleType,
                VehicleInfo = vehicle.GetVehicleInfo(),
                OperatingCost = vehicle.CalculateOperatingCost(command.Distance),
                MaxWeight = vehicle.MaxWeight,
                MaxVolume = vehicle.MaxVolume
            },
            
            Profit = shipping.CalculateCost() - vehicle.CalculateOperatingCost(command.Distance)
        });
    }
    
    private string GetShippingDescription(ShippingType type)
    {
        return type switch
        {
            ShippingType.Truck => "Грузовик",
            ShippingType.Sea => "По морю",
            ShippingType.Train => "Поездом",
            ShippingType.Air => "Самолетом",
            _ => "Неизвестный тип перевозки"
        };
    }
    private string GetVehicleDescription(VehicleType type)
    {
        return type switch
        {
            VehicleType.Truck => "Грузовик",
            VehicleType.CargoShip => "Грузовое судно",
            VehicleType.FreightTrain => "Грузовой поезд", 
            VehicleType.CargoPlain => "Грузовой самолет",
            _ => "Неизвестный тип транспорта"
        };
    }
}