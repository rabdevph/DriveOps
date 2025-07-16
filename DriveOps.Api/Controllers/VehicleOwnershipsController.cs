using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.VehicleOwnership;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

/// <summary>
/// API controller for managing vehicle ownerships, including creation and transfer.
/// </summary>
[ApiController]
[Route("api/vehicle-ownerships")]
public class VehicleOwnershipsController(IVehicleOwnershipService vehicleOwnershipService) : ApiBaseController
{
    private readonly IVehicleOwnershipService _vehicleOwnershipService = vehicleOwnershipService;

    /// <summary>
    /// Gets the details of vehicle ownership by ID.
    /// </summary>
    /// <param name="id">Vehicle ownership ID.</param>
    /// <returns>
    /// 200 Vehicle ownership details
    /// 404 Vehicle ownership not found
    /// 500 Unexpected error
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        int id)
    {
        var result = await _vehicleOwnershipService.GetByIdAsync(id);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new vehicle ownership.
    /// </summary>
    /// <param name="dto">Vehicle ownership data</param>
    /// <returns>
    /// 201 Vehicle ownership created
    /// 404 Invalid input
    /// 500 Unexpected error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] VehicleOwnershipCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _vehicleOwnershipService.CreateAsync(dto);

        if (!result.Success)
            return HandleServiceError(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Transfer a vehicle ownership.
    /// </summary>
    /// <param name="dto">Vehicle ownership transfer data.</param>
    /// <returns>
    /// 201 Vehicle ownership transferred
    /// 404 Invalid input
    /// 500 Unexpected error
    /// </returns>
    [HttpPost("transfer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Transfer(
        [FromBody] VehicleOwnershipTransferDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _vehicleOwnershipService.TransferAsync(dto);

        if (!result.Success)
            return HandleServiceError(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }
}
