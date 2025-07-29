using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.JobOrder;
using DriveOps.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class JobOrderService(DriveOpsContext dbContext) : IJobOrderService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<JobOrderPaginatedResultDto<JobOrderDetailsDto>> GetAllAsync(
        JobOrderStatus? status, int? customerId, int page, int pageSize)
    {
        // Ensure page and pageSize are within valid bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Query base with necessary includes
        var query = _dbContext.JobOrders
            .Include(jo => jo.Customer!).ThenInclude(c => c.IndividualCustomer)
            .Include(jo => jo.Customer!).ThenInclude(c => c.CompanyCustomer)
            .Include(jo => jo.Vehicle)
            .Include(jo => jo.Technician)
            .AsQueryable();

        // Optional filtering by status
        if (status.HasValue)
            query = query.Where(jo => jo.Status == status.Value);

        // Optional filtering by customer
        if (customerId.HasValue)
            query = query.Where(jo => jo.CustomerId == customerId.Value);

        // Get total count and calculate pagination metadata
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Apply pagination and project to DTO
        var jobOrders = await query
            .OrderBy(jo => jo.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = jobOrders.Select(jo => jo.ToJobOrderDetailsDto()).ToList();

        return new JobOrderPaginatedResultDto<JobOrderDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = items
        };
    }

    public async Task<ServiceResult<JobOrderDetailsDto>> GetByIdAsync(int id)
    {
        // Retrieve job order and related details
        var jobOrder = await _dbContext.JobOrders
            .Include(jo => jo.Customer!).ThenInclude(c => c.IndividualCustomer)
            .Include(jo => jo.Customer!).ThenInclude(c => c.CompanyCustomer)
            .Include(jo => jo.Vehicle)
            .Include(jo => jo.Technician)
            .FirstOrDefaultAsync(jo => jo.Id == id);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<JobOrderDetailsDto>(jobOrder, id);
        if (validationResult is not null)
            return validationResult;

        // Return job order details
        var result = jobOrder!.ToJobOrderDetailsDto();
        return ServiceResult<JobOrderDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<JobOrderDetailsDto>> CreateAsync(JobOrderCreateDto dto)
    {
        // Check for duplicate job order number
        var jobOrderNumberExists = await _dbContext.JobOrders
            .AnyAsync(jo => jo.JobOrderNumber == dto.JobOrderNumber);

        var validationResult = JobOrderValidator.ValidateJobOrderNumberIsUnique(jobOrderNumberExists);
        if (validationResult is not null)
            return validationResult;

        // Validate customer exists
        var customer = await _dbContext.Customers.FindAsync(dto.CustomerId);
        var customerValidationResult = CustomerValidator.ValidateExistingCustomer<JobOrderDetailsDto>(customer, dto.CustomerId);
        if (customerValidationResult is not null)
            return customerValidationResult;

        // Validate vehicle exists
        var vehicle = await _dbContext.Vehicles.FindAsync(dto.VehicleId);
        var vehicleValidationResult = VehicleValidator.ValidateExistingVehicle<JobOrderDetailsDto>(vehicle, dto.VehicleId);
        if (vehicleValidationResult is not null)
            return vehicleValidationResult;

        // Validate technician exists
        var technician = await _dbContext.Technicians.FindAsync(dto.TechnicianId);
        var technicianValidationResult = TechnicianValidator.ValidateTechnicianExists<JobOrderDetailsDto>(technician, dto.TechnicianId);
        if (technicianValidationResult is not null)
            return technicianValidationResult;

        // Save new job order
        var newJobOrder = dto.ToEntity();
        await _dbContext.JobOrders.AddAsync(newJobOrder);
        await _dbContext.SaveChangesAsync();

        // Reload job order
        var jobOrder = await _dbContext.JobOrders
            .Include(jo => jo.Customer!).ThenInclude(c => c.IndividualCustomer)
            .Include(jo => jo.Customer!).ThenInclude(c => c.CompanyCustomer)
            .Include(jo => jo.Vehicle)
            .Include(jo => jo.Technician)
            .FirstOrDefaultAsync(jo => jo.Id == newJobOrder.Id);

        // Return job order details
        var result = jobOrder!.ToJobOrderDetailsDto();
        return ServiceResult<JobOrderDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<JobOrderDetailsDto>> PatchDetailsAsync(int id, JobOrderDetailsPatchDto dto)
    {
        // Fetch job order
        var jobOrder = await _dbContext.JobOrders.FindAsync(id);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<JobOrderDetailsDto>(jobOrder, id);
        if (validationResult is not null)
            return validationResult;

        //
        if (dto.CustomerId.HasValue)
        {
            var customer = await _dbContext.Customers.FindAsync(dto.CustomerId.Value);
            validationResult = CustomerValidator.ValidateExistingCustomer<JobOrderDetailsDto>(customer, dto.CustomerId.Value);
            if (validationResult is not null)
                return validationResult;

            jobOrder!.CustomerId = dto.CustomerId.Value;
        }

        //
        if (dto.VehicleId.HasValue)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(dto.VehicleId.Value);
            validationResult = VehicleValidator.ValidateExistingVehicle<JobOrderDetailsDto>(vehicle, dto.VehicleId.Value);
            if (validationResult is not null)
                return validationResult;

            jobOrder!.VehicleId = dto.VehicleId.Value;
        }

        //
        if (dto.TechnicianId.HasValue)
        {
            var technician = await _dbContext.Technicians.FindAsync(dto.TechnicianId.Value);
            validationResult = TechnicianValidator.ValidateTechnicianExists<JobOrderDetailsDto>(technician, dto.TechnicianId.Value);
            if (validationResult is not null)
                return validationResult;

            jobOrder!.TechnicianId = dto.TechnicianId.Value;
        }

        jobOrder!.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        // Reload job order
        var updatedJobOrder = await _dbContext.JobOrders
            .Include(jo => jo.Customer!).ThenInclude(c => c.IndividualCustomer)
            .Include(jo => jo.Customer!).ThenInclude(c => c.CompanyCustomer)
            .Include(jo => jo.Vehicle)
            .Include(jo => jo.Technician)
            .FirstOrDefaultAsync(jo => jo.Id == jobOrder.Id);

        var result = updatedJobOrder!.ToJobOrderDetailsDto();
        return ServiceResult<JobOrderDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<JobOrderDetailsDto>> UpdateStatusAsync(int id, JobOrderStatusUpdateDto dto)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FindAsync(id);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<JobOrderDetailsDto>(jobOrder, id);
        if (validationResult is not null)
            return validationResult;

        // Update status
        jobOrder!.Status = dto.Status;

        jobOrder.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        // Reload job order
        var updatedJobOrder = await _dbContext.JobOrders
            .Include(jo => jo.Customer!).ThenInclude(c => c.IndividualCustomer)
            .Include(jo => jo.Customer!).ThenInclude(c => c.CompanyCustomer)
            .Include(jo => jo.Vehicle)
            .Include(jo => jo.Technician)
            .FirstOrDefaultAsync(jo => jo.Id == jobOrder.Id);

        var result = updatedJobOrder!.ToJobOrderDetailsDto();
        return ServiceResult<JobOrderDetailsDto>.Ok(result);
    }
}
