using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

/// <summary>
/// API controller for managing vehicles, including creation, updates, and retrieval.
/// </summary>
[ApiController]
[Route("api/vehicles")]
[Produces("application/json")]
public class VehiclesController(IVehicleService vehicleService) : ControllerBase
{
    private readonly IVehicleService _vehicleService = vehicleService;

    /// <summary>
    /// Gets a paginated list of vehicles including current owner.
    /// </summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10).</param>
    /// <returns>
    /// 200 Paginated list of vehicles
    /// 500 Unexpected error
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _vehicleService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Gets the full details of a vehicle by ID.
    /// </summary>
    /// <param name="id">Vehicle ID.</param>
    /// <returns>
    /// 200 Vehicle details
    /// 404 Vehicle not found
    /// 500 Unexpected error
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        int id)
    {
        var result = await _vehicleService.GetByIdAsync(id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Creates a new vehicle.
    /// </summary>
    /// <param name="dto">Vehicle Data.</param>
    /// <returns>
    /// 201 Vehicle created
    /// 404 Invalid input
    /// 500 Unexpected error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] VehicleCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdVehicle = await _vehicleService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
    }

    /// <summary>
    /// Updates existing vehicle's details.
    /// </summary>
    /// <param name="id">Vehicle ID.</param>
    /// <param name="dto">Updated vehicle data.</param>
    /// <returns>
    /// 200 Vehicle updated  
    /// 400 Invalid input  
    /// 404 Vehicle not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDetails(
        int id,
        [FromBody] VehicleDetailsUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var vehicle = await _vehicleService.UpdateDetailsAsync(id, dto);

        if (vehicle is null)
            return NotFound();

        return Ok(vehicle);
    }
}
