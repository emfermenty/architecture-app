using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly VehicleService _vehicleService;

    public VehicleController(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommandDTO command)
    {
        try
        {
            var vehicle = await _vehicleService.CreateAsync(command);
            return Ok(vehicle);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}