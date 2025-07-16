using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Models;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class CustomerService(DriveOpsContext dbContext) : ICustomerService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<CustomerPaginatedResultDto<CustomerDetailsDto>> GetAllAsync(CustomerType? type, int page, int pageSize)
    {
        // Ensure page and pageSize are within valid bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Query base with necessary includes
        var query = _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .AsQueryable();

        // Optional filtering by customer type
        if (type.HasValue)
        {
            query = query.Where(c => c.Type == type.Value);
        }

        // Get total count and calculate pagination metadata
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Apply pagination and project to DTOs
        var customers = await query
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => c.ToCustomerDetailsDto())
            .ToListAsync();

        return new CustomerPaginatedResultDto<CustomerDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = customers
        };
    }

    public async Task<ServiceResult<CustomerDetailsDto>> GetByIdAsync(int id, bool onlyCurrent)
    {
        // Retrieve customer with all related data
        var customer = await _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .Include(c => c.VehicleOwnerships)
                .ThenInclude(vo => vo.Vehicle)
            .FirstOrDefaultAsync(c => c.Id == id);

        // Validate customer exists
        var validationResult = CustomerValidator.ValidateExistingCustomer<CustomerDetailsDto>(customer, id);
        if (validationResult is not null)
            return validationResult;

        // Return customer details
        var result = customer!.ToCustomerDetailsDto(onlyCurrent);
        return ServiceResult<CustomerDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<CustomerDetailsDto>> CreateAsync(CustomerCreateDto dto)
    {
        // Check for duplicate email or phone number
        var contactDetailsExists = await _dbContext.Customers
            .AnyAsync(c => c.Email == dto.Email || c.PhoneNumber == dto.PhoneNumber);

        var validationResult = CustomerValidator.ValidateExistingContacts(contactDetailsExists);
        if (validationResult is not null)
            return validationResult;

        // Validate subtype-specific fields (Individual or Company)
        validationResult = CustomerValidator.ValidateSubtypeDetails(dto);
        if (validationResult is not null)
            return validationResult;

        // Save new customer
        var customer = dto.ToEntity();
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();

        var result = customer.ToCustomerDetailsDto();
        return ServiceResult<CustomerDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<CustomerDetailsDto>> UpdateDetailsAsync(int id, CustomerDetailsUpdateDto dto)
    {
        // Fetch customer with subtype entities
        var existingCustomer = await _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .FirstOrDefaultAsync(c => c.Id == id);

        // Validate customer exists
        var validationResult = CustomerValidator.ValidateExistingCustomer<CustomerDetailsDto>(existingCustomer, id);
        if (validationResult is not null)
            return validationResult;

        // Check for contact conflicts with other customers
        var contactDetailsExists = await _dbContext.Customers
            .AnyAsync(c => c.Id != id && (c.Email == dto.Email || c.PhoneNumber == dto.PhoneNumber));

        validationResult = CustomerValidator.ValidateExistingContacts(contactDetailsExists);
        if (validationResult is not null)
            return validationResult;

        // Validate subtype-specific fields (Individual or Company)
        validationResult = CustomerValidator.ValidateSubtypeDetails(dto);
        if (validationResult is not null)
            return validationResult;

        // Update shared fields
        existingCustomer!.Type = dto.Type;
        existingCustomer.Email = dto.Email;
        existingCustomer.PhoneNumber = dto.PhoneNumber;
        existingCustomer.Address = dto.Address;
        existingCustomer.Notes = dto.Notes;

        // Update subtype: Individual
        if (dto.Type == CustomerType.Individual && dto.Individual is not null)
        {
            existingCustomer.IndividualCustomer ??= new IndividualCustomer();
            existingCustomer.IndividualCustomer.FirstName = dto.Individual.FirstName;
            existingCustomer.IndividualCustomer.LastName = dto.Individual.LastName;
            existingCustomer.CompanyCustomer = null;
        }

        // Update subtype: company
        if (dto.Type == CustomerType.Company && dto.Company is not null)
        {
            existingCustomer.CompanyCustomer ??= new CompanyCustomer();
            existingCustomer.CompanyCustomer.CompanyName = dto.Company.CompanyName;
            existingCustomer.CompanyCustomer.ContactPerson = dto.Company.ContactPerson;
            existingCustomer.CompanyCustomer.Position = dto.Company.Position;
            existingCustomer.IndividualCustomer = null;
        }

        existingCustomer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        var result = existingCustomer.ToCustomerDetailsDto();
        return ServiceResult<CustomerDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<CustomerDetailsDto>> UpdateStatusAsync(int id, CustomerStatusUpdateDto dto)
    {
        // Fetch customer
        var existingCustomer = await _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .FirstOrDefaultAsync(c => c.Id == id);

        // Check customer exists
        var validationResult = CustomerValidator.ValidateExistingCustomer<CustomerDetailsDto>(existingCustomer, id);
        if (validationResult is not null)
            return validationResult;

        // Update status
        existingCustomer!.Status = dto.Status;
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        var result = existingCustomer.ToCustomerDetailsDto();
        return ServiceResult<CustomerDetailsDto>.Ok(result);
    }
}
