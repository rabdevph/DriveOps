using DriveOps.Api.Data;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Models;
using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class CustomerService(DriveOpsContext dbContext) : ICustomerService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<CustomerPaginatedResultDto<CustomerDetailsDto>> GetAllAsync(CustomerType? type, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var query = _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(c => c.Type == type.Value);
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

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

    public async Task<CustomerDetailsDto?> GetByIdAsync(int id, bool onlyCurrent)
    {
        var customer = await _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .Include(c => c.VehicleOwnerships)
                .ThenInclude(vo => vo.Vehicle)
            .FirstOrDefaultAsync(c => c.Id == id);

        return customer?.ToCustomerDetailsDto();
    }

    public async Task<CustomerDetailsDto> CreateAsync(CustomerCreateDto dto)
    {
        var customer = dto.ToEntity();
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();

        return customer.ToCustomerDetailsDto();
    }

    public async Task<CustomerDetailsDto?> UpdateDetailsAsync(int id, CustomerDetailsUpdateDto dto)
    {
        var existingCustomer = await _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCustomer is null)
            return null;

        existingCustomer.Type = dto.Type;
        existingCustomer.Email = dto.Email;
        existingCustomer.PhoneNumber = dto.PhoneNumber;
        existingCustomer.Address = dto.Address;
        existingCustomer.Notes = dto.Notes;

        if (dto.Type == CustomerType.Individual && dto.Individual is not null)
        {
            existingCustomer.IndividualCustomer ??= new IndividualCustomer();
            existingCustomer.IndividualCustomer.FirstName = dto.Individual.FirstName;
            existingCustomer.IndividualCustomer.LastName = dto.Individual.LastName;
            existingCustomer.CompanyCustomer = null;
        }

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

        return existingCustomer.ToCustomerDetailsDto();
    }

    public async Task<CustomerDetailsDto?> UpdateStatusAsync(int id, CustomerStatusUpdateDto dto)
    {
        var existingCustomer = await _dbContext.Customers
            .Include(c => c.IndividualCustomer)
            .Include(c => c.CompanyCustomer)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCustomer is null)
            return null;

        existingCustomer.Status = dto.Status;
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return existingCustomer.ToCustomerDetailsDto();
    }
}
