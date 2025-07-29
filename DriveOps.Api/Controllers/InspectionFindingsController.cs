using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.InspectionFinding;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

[ApiController]
[Route("api/joborders/{jobOrderNumber}/findings")]
[Produces("application/json")]
public class InspectionFindingsController(IInspectionFindingService inspectionFindingService) : ApiBaseController
{
    private readonly IInspectionFindingService _inspectionFindingService = inspectionFindingService;

    /// <summary>
    /// Gets a paginated list of inspection findings for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10, max: 100).</param>
    /// <returns>
    /// 200 Paginated list of inspection findings retrieved successfully  
    /// 404 Job order not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromRoute] string jobOrderNumber,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _inspectionFindingService.GetAllAsync(jobOrderNumber, page, pageSize);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the details of an inspection finding by ID for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The inspection finding ID.</param>
    /// <returns>
    /// 200 Inspection finding details retrieved successfully  
    /// 404 Job order or inspection finding not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        [FromRoute] string jobOrderNumber,
        int id)
    {
        var result = await _inspectionFindingService.GetByIdAsync(jobOrderNumber, id);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new inspection finding for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="dto">The inspection finding creation data.</param>
    /// <returns>
    /// 201 Inspection finding created successfully  
    /// 400 Invalid input  
    /// 404 Job order not found  
    /// 409 Job order status not valid for creating findings  
    /// 500 Unexpected error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromRoute] string jobOrderNumber,
        [FromBody] InspectionFindingCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _inspectionFindingService.CreateAsync(jobOrderNumber, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return CreatedAtAction(nameof(GetById), new { jobOrderNumber, id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Updates an inspection finding for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The inspection finding ID.</param>
    /// <param name="dto">The inspection finding update data.</param>
    /// <returns>
    /// 200 Inspection finding updated successfully  
    /// 400 Invalid input or invalid severity downgrade  
    /// 404 Job order or inspection finding not found  
    /// 409 Job order status not valid or finding already resolved  
    /// 500 Unexpected error
    /// </returns>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        [FromRoute] string jobOrderNumber,
        int id,
        [FromBody] InspectionFindingUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _inspectionFindingService.UpdateAsync(jobOrderNumber, id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the resolved status of an inspection finding.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The inspection finding ID.</param>
    /// <param name="dto">The status update data.</param>
    /// <returns>
    /// 200 Inspection finding status updated successfully  
    /// 400 Invalid input or recommendation required for resolved findings  
    /// 404 Job order or inspection finding not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] string jobOrderNumber,
        int id,
        [FromBody] InspectionFindingStatusUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _inspectionFindingService.UpdateStatusAsync(jobOrderNumber, id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Deletes an inspection finding from a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The inspection finding ID.</param>
    /// <returns>
    /// 204 Inspection finding deleted successfully  
    /// 404 Job order or inspection finding not found  
    /// 409 Job order status not valid or finding already resolved  
    /// 500 Unexpected error
    /// </returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(
        [FromRoute] string jobOrderNumber,
        int id)
    {
        var result = await _inspectionFindingService.DeleteAsync(jobOrderNumber, id);

        if (!result.Success)
            return HandleServiceError(result);

        return NoContent();
    }
}
