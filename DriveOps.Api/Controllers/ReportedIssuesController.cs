using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.ReportedIssue;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

[ApiController]
[Route("api/joborders/{jobOrderNumber}/issues")]
[Produces("application/json")]
public class ReportedIssuesController(IReportedIssue reportedIssueService) : ApiBaseController
{
    private readonly IReportedIssue _reportedIssueService = reportedIssueService;

    /// <summary>
    /// Gets a paginated list of reported issues for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10, max: 100).</param>
    /// <returns>
    /// 200 Paginated list of reported issues retrieved successfully  
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
        var result = await _reportedIssueService.GetAllAsync(jobOrderNumber, page, pageSize);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the details of a reported issue by ID for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The reported issue ID.</param>
    /// <returns>
    /// 200 Reported issue details retrieved successfully  
    /// 404 Job order or reported issue not found  
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
        var result = await _reportedIssueService.GetByIdAsync(jobOrderNumber, id);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new reported issue for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="dto">The reported issue creation data.</param>
    /// <returns>
    /// 201 Reported issue created successfully  
    /// 400 Invalid input  
    /// 404 Job order not found  
    /// 409 Job order status not valid for creating issues  
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
        [FromBody] ReportedIssueCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reportedIssueService.CreateAsync(jobOrderNumber, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return CreatedAtAction(nameof(GetById), new { jobOrderNumber, id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Updates a reported issue for a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The reported issue ID.</param>
    /// <param name="dto">The reported issue update data.</param>
    /// <returns>
    /// 200 Reported issue updated successfully  
    /// 400 Invalid input  
    /// 404 Job order or reported issue not found  
    /// 409 Job order status not valid for updating issues  
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
        [FromBody] ReportedIssueUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reportedIssueService.UpdateAsync(jobOrderNumber, id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Deletes a reported issue from a specific job order.
    /// </summary>
    /// <param name="jobOrderNumber">The job order number.</param>
    /// <param name="id">The reported issue ID.</param>
    /// <returns>
    /// 204 Reported issue deleted successfully  
    /// 404 Job order or reported issue not found  
    /// 409 Job order status not valid for deleting issues  
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
        var result = await _reportedIssueService.DeleteAsync(jobOrderNumber, id);

        if (!result.Success)
            return HandleServiceError(result);

        return NoContent();
    }
}
