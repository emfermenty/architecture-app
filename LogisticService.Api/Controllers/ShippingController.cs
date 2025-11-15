using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

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
    public IActionResult? CreateShipping([FromBody] CreateShippingCommand command)
    {
        var result = _shippingService.CreateShipping(command);
        if (result == null)
            return BadRequest("Невозможно создать логистику для данных параметров.");

        return Ok(result);
    }

    [HttpPost("with-vehicle-async")]
    public async Task<ActionResult<object>> CreateShippingWithVehicleAsync([FromBody] CreateShippingCommandDTO command)
    {
        try
        {
            var result = await _shippingService.CreateShippingWithVehicleAsync(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("undo")]
    public async Task<ActionResult> Undo()
    {
        if (_shippingService.CanUndo)
        {
            await _shippingService.UndoLastCommandAsync();
            return Ok("Last operation undone");
        }
        
        return BadRequest("No commands to undo");
    }

    [HttpGet("GetAllShippings")]
    public IActionResult GetAllShippings()
    {
        return Ok(_shippingService.GetAllShippings());
    }
}