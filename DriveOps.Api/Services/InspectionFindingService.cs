using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.InspectionFinding;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class InspectionFindingService(DriveOpsContext dbContext) : IInspectionFindingService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<ServiceResult<InspectionFindingPaginatedResultDto<InspectionFindingDetailsDto>>> GetAllAsync(
        string jobOrderNumber,
        int page,
        int pageSize)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<InspectionFindingPaginatedResultDto<InspectionFindingDetailsDto>>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Ensure page and pageSize are within valid bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Filter inspection findings for the specific job order
        var query = _dbContext.InspectionFindings
            .Where(fi => fi.JobOrderId == jobOrder!.Id)
            .AsQueryable();

        // Get total count and calculate pagination metadata
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Apply pagination and project to DTOs
        var inspectionFindings = await query
            .OrderBy(fi => fi.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(fi => fi.JobOrder)
            .Select(fi => fi.ToInspectionFindingDetailsDto())
            .ToListAsync();

        var result = new InspectionFindingPaginatedResultDto<InspectionFindingDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = inspectionFindings
        };

        // Return paginated result
        return ServiceResult<InspectionFindingPaginatedResultDto<InspectionFindingDetailsDto>>.Ok(result);
    }

    public async Task<ServiceResult<InspectionFindingDetailsDto>> GetByIdAsync(string jobOrderNumber, int id)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<InspectionFindingDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Retrieve inspection finding
        var inspectionFinding = await _dbContext.InspectionFindings
            .Include(fi => fi.JobOrder)
            .FirstOrDefaultAsync(fi => fi.Id == id && fi.JobOrder.JobOrderNumber == jobOrderNumber);

        // Validate inspection finding exists
        validationResult = InspectionFindingValidator.ValidateInspectionFindingExists<InspectionFindingDetailsDto>(inspectionFinding, id);
        if (validationResult is not null)
            return validationResult;

        // Return inspection finding
        var result = inspectionFinding!.ToInspectionFindingDetailsDto();
        return ServiceResult<InspectionFindingDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<InspectionFindingDetailsDto>> CreateAsync(string jobOrderNumber, InspectionFindingCreateDto dto)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<InspectionFindingDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status for creation
        validationResult = JobOrderValidator.ValidateJobOrderStatusForOperation<InspectionFindingDetailsDto>(jobOrder!, "create", "inspection finding");
        if (validationResult is not null)
            return validationResult;

        // Save new inspection finding
        var newInspectionFinding = dto.ToEntity();
        newInspectionFinding.JobOrderId = jobOrder!.Id;
        await _dbContext.InspectionFindings.AddAsync(newInspectionFinding);
        await _dbContext.SaveChangesAsync();

        // Return new inspection finding
        var result = newInspectionFinding.ToInspectionFindingDetailsDto();
        return ServiceResult<InspectionFindingDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<InspectionFindingDetailsDto>> UpdateAsync(
        string jobOrderNumber,
        int id,
        InspectionFindingUpdateDto dto)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<InspectionFindingDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status for updates
        validationResult = JobOrderValidator.ValidateJobOrderStatusForOperation<InspectionFindingDetailsDto>(jobOrder!, "update", "inspection finding");
        if (validationResult is not null)
            return validationResult;

        // Find inspection finding within the specific job order
        var inspectionFinding = await _dbContext.InspectionFindings
            .FirstOrDefaultAsync(fi => fi.Id == id && fi.JobOrderId == jobOrder!.Id);

        // Validate inspection finding exists
        validationResult = InspectionFindingValidator.ValidateInspectionFindingExists<InspectionFindingDetailsDto>(inspectionFinding, id);
        if (validationResult is not null)
            return validationResult;

        // Validate inspection finding can be modified
        validationResult = InspectionFindingValidator.ValidateInspectionFindingCanBeModified<InspectionFindingDetailsDto>(inspectionFinding!, id);
        if (validationResult is not null)
            return validationResult;

        // Validate severity downgrade
        validationResult = InspectionFindingValidator.ValidateSeverityDowngrade<InspectionFindingDetailsDto>(inspectionFinding!, dto.Severity);
        if (validationResult is not null)
            return validationResult;

        // Update inspection finding details and timestamp
        inspectionFinding!.Description = dto.Description;
        inspectionFinding.Recommendation = dto.Recommendation;
        inspectionFinding.Severity = dto.Severity;
        inspectionFinding.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        // Return updated inspection finding
        var result = inspectionFinding.ToInspectionFindingDetailsDto();
        return ServiceResult<InspectionFindingDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<InspectionFindingDetailsDto>> UpdateStatusAsync(
        string jobOrderNumber,
        int id,
        InspectionFindingStatusUpdateDto dto)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<InspectionFindingDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status for status updates
        validationResult = JobOrderValidator.ValidateJobOrderStatusForOperation<InspectionFindingDetailsDto>(jobOrder!, "update status", "inspection finding");
        if (validationResult is not null)
            return validationResult;

        // Find inspection finding within the specific job order
        var inspectionFinding = await _dbContext.InspectionFindings
            .FirstOrDefaultAsync(fi => fi.Id == id && fi.JobOrderId == jobOrder!.Id);

        // Validate inspection finding exists
        validationResult = InspectionFindingValidator.ValidateInspectionFindingExists<InspectionFindingDetailsDto>(inspectionFinding, id);
        if (validationResult is not null)
            return validationResult;

        // Update status and timestamp
        inspectionFinding!.IsResolved = dto.IsResolved;
        inspectionFinding.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        // Return updated inspection finding
        var result = inspectionFinding.ToInspectionFindingDetailsDto();
        return ServiceResult<InspectionFindingDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(string jobOrderNumber, int id)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<bool>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status for deletion
        validationResult = JobOrderValidator.ValidateJobOrderStatusForOperation<bool>(jobOrder!, "delete", "inspection finding");
        if (validationResult is not null)
            return validationResult;

        // Find inspection finding within the specific job order
        var inspectionFinding = await _dbContext.InspectionFindings
            .FirstOrDefaultAsync(fi => fi.Id == id && fi.JobOrderId == jobOrder!.Id);

        // Validate inspection finding exists
        validationResult = InspectionFindingValidator.ValidateInspectionFindingExists<bool>(inspectionFinding, id);
        if (validationResult is not null)
            return validationResult;

        // Validate inspection finding can be deleted
        validationResult = InspectionFindingValidator.ValidateInspectionFindingCanBeDeleted<bool>(inspectionFinding!, id);
        if (validationResult is not null)
            return validationResult;

        // Remove inspection finding from database
        _dbContext.InspectionFindings.Remove(inspectionFinding!);
        await _dbContext.SaveChangesAsync();

        // Return success result
        return ServiceResult<bool>.Ok(true);
    }
}
