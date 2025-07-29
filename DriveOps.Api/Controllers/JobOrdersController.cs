using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.JobOrder;
using DriveOps.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

[ApiController]
[Route("api/joborders")]
[Produces("application/json")]
public class JobOrdersController(IJobOrderService jobOrderService) : ApiBaseController
{
    private readonly IJobOrderService _jobOrderService = jobOrderService;

    /// <summary>
    /// Gets a paginated list of job orders, optionally filtered by status and/or customer ID.
    /// </summary>
    /// <param name="status">Optional job order status filter.</param>
    /// <param name="customerId">Optional customer ID filter.</param>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10, max: 100).</param>
    /// <returns>
    /// 200 Paginated list of job orders  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] JobOrderStatus? status,
        [FromQuery] int? customerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _jobOrderService.GetAllAsync(status, customerId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Gets the full details of a job order by ID, including customer, vehicle, and technician information.
    /// </summary>
    /// <param name="id">The job order ID.</param>
    /// <returns>
    /// 200 Job order details retrieved successfully  
    /// 404 Job order not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        int id)
    {
        var result = await _jobOrderService.GetByIdAsync(id);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new job order with the specified customer, vehicle, and technician.
    /// </summary>
    /// <param name="dto">The job order creation data.</param>
    /// <returns>
    /// 201 Job order created successfully  
    /// 400 Invalid input or duplicate job order number  
    /// 404 Customer, vehicle, or technician not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] JobOrderCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _jobOrderService.CreateAsync(dto);

        if (!result.Success)
            return HandleServiceError(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Updates the core details of a job order (customer, vehicle, or technician assignment).
    /// </summary>
    /// <param name="id">The job order ID.</param>
    /// <param name="dto">The job order details to update.</param>
    /// <returns>
    /// 200 Job order details updated successfully  
    /// 400 Invalid input  
    /// 404 Job order, customer, vehicle, or technician not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDetails(
        int id,
        [FromBody] JobOrderDetailsPatchDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _jobOrderService.PatchDetailsAsync(id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the status of a job order (e.g., Pending, InProgress, Completed, Cancelled).
    /// </summary>
    /// <param name="id">The job order ID.</param>
    /// <param name="dto">The new status to set.</param>
    /// <returns>
    /// 200 Job order status updated successfully  
    /// 400 Invalid input  
    /// 404 Job order not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] JobOrderStatusUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _jobOrderService.UpdateStatusAsync(id, dto);

        if (!result.Success)
            return HandleServiceError(result);

        return Ok(result.Data);
    }
}
