using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using LogisticService.Domain.Observer;
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
    [HttpGet("test-observers/{trackingNumber}")]
    public async Task<ActionResult> TestObservers(string trackingNumber)
    {
        try
        {
            var shipping = await _shippingService.GetByTrackingNumberAsync(trackingNumber);
            if (shipping == null)
                return NotFound($"Shipping with tracking number {trackingNumber} not found");

            var observerInfo = new
            {
                TrackingNumber = shipping.TrackingNumber,
                ObserverCount = shipping.GetObserverCount(),
                Observers = shipping.GetObserverInfo(),
                CurrentStatus = shipping.Status
            };

            return Ok(observerInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("GetAllShippings")]
    public async Task<ActionResult> GetAllShippings()
    {
        var shippings = await _shippingService.GetAllShippings();
        if (shippings == null || shippings.Count == 0)
        {
            return BadRequest("Нет перевозок");
        }
        return Ok(shippings);
    }
    [HttpGet("test-di-observers")]
    public ActionResult TestDIObservers([FromServices] IObserverManager observerManager)
    {
        try
        {
            // Используем рефлексию чтобы получить список наблюдателей
            var observerManagerType = observerManager.GetType();
            var observersField = observerManagerType.GetField("_observers", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
            if (observersField != null)
            {
                var observers = observersField.GetValue(observerManager) as IEnumerable<IShippingObserver>;
            
                return Ok(new
                {
                    ObserverManagerType = observerManager.GetType().Name,
                    ObserversCount = observers?.Count() ?? 0,
                    ObserverTypes = observers?.Select(o => o.GetType().Name).ToList() ?? new List<string>()
                });
            }
        
            return Ok(new { message = "Cannot access observers field" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}