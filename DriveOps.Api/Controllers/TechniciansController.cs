using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.Technician;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

/// <summary>
/// API controller for managing technicians, including creation, updates, and retrieval.
/// </summary>
[ApiController]
[Route("api/technicians")]
[Produces("application/json")]
public class TechniciansController(ITechnicianService technicianService) : ApiBaseController
{
    private readonly ITechnicianService _technicianService = technicianService;

    /// <summary>
    /// Gets a paginated list of technicians.
    /// </summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10).</param>
    /// <returns>
    /// 200 Paginated list of technicians  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _technicianService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Gets the full details of a technician by ID.
    /// </summary>
    /// <param name="id">Technician ID.</param>
    /// <returns>
    /// 200 Technician details  
    /// 404 Technician not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        int id)
    {
        var result = await _technicianService.GetByIdAsync(id);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new technician.
    /// </summary>
    /// <param name="dto">Technician data.</param>
    /// <returns>
    /// 201 Technician created  
    /// 400 Invalid input  
    /// 500 Unexpected error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] TechnicianCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _technicianService.CreateAsync(dto);

        if (!result.Success)
            return HandleServiceError(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Updates an existing technician's details.
    /// </summary>
    /// <param name="id">Technician ID.</param>
    /// <param name="dto">Updated technician data.</param>
    /// <returns>
    /// 200 Technician updated  
    /// 400 Invalid input  
    /// 404 Technician not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDetails(
        int id,
        [FromBody] TechnicianDetailsUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _technicianService.UpdateDetailsAsync(id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the status of a technician (e.g., active or inactive).
    /// </summary>
    /// <param name="id">Technician ID.</param>
    /// <param name="dto">New status.</param>
    /// <returns>
    /// 200 Technician status updated  
    /// 400 Invalid input  
    /// 404 Technician not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] TechnicianStatusUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _technicianService.UpdateStatusAsync(id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }
}
