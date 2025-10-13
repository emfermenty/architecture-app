using architectureProject.DTO;
using architectureProject.Models.enums;
using architectureProject.ServiceControllers;
using architectureProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace architectureProject.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly ShippingService _shippingService;

    public ShippingController(ShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    [HttpPost("quotes")]
    public IActionResult GetShippingQuotes([FromBody] ShippingRequest request)
    {
        var quotes = _shippingService.GetShippingQuotes(request);
        return Ok(quotes);
    }

    [HttpPost("optimize")]
    public IActionResult GetOptimalShipping([FromBody] ShippingRequest request)
    {
        var optimal = _shippingService.GetOptimalShipping(request);
        if (optimal == null)
            return BadRequest("Нет подходящих способов перевозки для данных параметров.");

        return Ok(optimal);
    }

    [HttpPost("create")]
    public IActionResult CreateShipping([FromBody] CreateShippingCommand command)
    {
        var result = _shippingService.CreateShipping(command);
        if (result == null)
            return BadRequest("Невозможно создать логистику для данных параметров.");

        return Ok(result);
    }

    [HttpPost("create-with-vehicle")]
    public IActionResult CreateShippingWithVehicle([FromBody] CreateShippingCommand command)
    {
        var result = _shippingService.CreateShippingWithVehicle(command);
        if (result == null)
            return BadRequest("Невозможно создать перевозку с транспортом.");

        return Ok(result);
    }

    [HttpGet("GetAllShippings")]
    public IActionResult GetAllShippings()
    {
        return Ok(_shippingService.GetAllShippings());
    }
}