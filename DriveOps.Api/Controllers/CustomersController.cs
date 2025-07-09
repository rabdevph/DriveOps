using DriveOps.Api.Interfaces;
using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

/// <summary>
/// API controller for managing customers, including creation, updates, and retrieval.
/// </summary>
[ApiController]
[Route("api/customers")]
[Produces("application/json")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    /// <summary>
    /// Gets a paginated list of customers, optionally filtered by type.
    /// </summary>
    /// <param name="type">Optional customer type filter.</param>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10).</param>
    /// <returns>
    /// 200 Paginated list of customers  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] CustomerType? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _customerService.GetAllAsync(type, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Gets the full details of a customer by ID, including their vehicles.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <param name="onlyCurrent">Whether to include only currently owned vehicles.</param>
    /// <returns>
    /// 200 Customer details  
    /// 404 Customer not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        int id,
        [FromQuery] bool onlyCurrent)
    {
        var result = await _customerService.GetByIdAsync(id, onlyCurrent);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Creates a new customer (individual or company).
    /// </summary>
    /// <param name="dto">Customer data.</param>
    /// <returns>
    /// 201 Customer created  
    /// 400 Invalid input  
    /// 500 Unexpected error
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CustomerCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCustomer = await _customerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
    }

    /// <summary>
    /// Updates an existing customer's details.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <param name="dto">Updated customer data.</param>
    /// <returns>
    /// 200 Customer updated  
    /// 400 Invalid input  
    /// 404 Customer not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDetails(
        int id,
        [FromBody] CustomerDetailsUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _customerService.UpdateDetailsAsync(id, dto);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    /// <summary>
    /// Updates the status of a customer (e.g., active or inactive).
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <param name="dto">New status.</param>
    /// <returns>
    /// 200 Customer status updated  
    /// 400 Invalid input  
    /// 404 Customer not found  
    /// 500 Unexpected error
    /// </returns>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] CustomerStatusUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _customerService.UpdateStatusAsync(id, dto);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }
}
