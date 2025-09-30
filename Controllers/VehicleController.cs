using architectureProject.DTO;
using architectureProject.Models;
using architectureProject.ServiceControllers;
using Microsoft.AspNetCore.Mvc;

namespace architectureProject.Controllers;

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
    public IActionResult CreateVehicle([FromBody] CreateVehicleCommandDto command)
    {
        try
        {
            var vehicle = await _vehicleService.CreateAsync(command);
            return Ok(new { 
                Id = vehicle.Id,
                VehicleType = vehicle.VehicleType,
                Model = vehicle.Model,
                VehicleInfo = vehicle.GetVehicleInfo()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}