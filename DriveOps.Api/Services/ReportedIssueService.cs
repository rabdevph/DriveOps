using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.ReportedIssue;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class ReportedIssue(DriveOpsContext dbContext) : IReportedIssue
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<ServiceResult<ReportedIssuePaginatedResultDto<ReportedIssueDetailsDto>>> GetAllAsync(
        string jobOrderNumber,
        int page,
        int pageSize)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<ReportedIssuePaginatedResultDto<ReportedIssueDetailsDto>>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Ensure page and pageSize are within valid bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Filter reported issues for the specific job order
        var query = _dbContext.ReportedIssues
            .Where(ri => ri.JobOrderId == jobOrder!.Id)
            .AsQueryable();

        // Get total count and calculate pagination metadata
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Apply pagination and project to DTOs
        var reportedIssues = await query
            .OrderBy(ri => ri.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(ri => ri.JobOrder)
            .Select(ri => ri.ToReportedIssueDetailsDto())
            .ToListAsync();

        var result = new ReportedIssuePaginatedResultDto<ReportedIssueDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = reportedIssues
        };

        // Return paginated result
        return ServiceResult<ReportedIssuePaginatedResultDto<ReportedIssueDetailsDto>>.Ok(result);
    }

    public async Task<ServiceResult<ReportedIssueDetailsDto>> GetByIdAsync(string jobOrderNumber, int id)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<ReportedIssueDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Retrieve reported issue
        var reportedIssue = await _dbContext.ReportedIssues
            .Include(ri => ri.JobOrder)
            .FirstOrDefaultAsync(ri => ri.Id == id && ri.JobOrder.JobOrderNumber == jobOrderNumber);

        // Validate reported issue exists
        validationResult = ReportedIssueValidator.ValidateReportedIssueExists<ReportedIssueDetailsDto>(reportedIssue, id);
        if (validationResult is not null)
            return validationResult;

        // Return reported issue
        var result = reportedIssue!.ToReportedIssueDetailsDto();
        return ServiceResult<ReportedIssueDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<ReportedIssueDetailsDto>> CreateAsync(string jobOrderNumber, ReportedIssueCreateDto dto)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<ReportedIssueDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status
        validationResult = JobOrderValidator.ValidateJobOrderStatus<ReportedIssueDetailsDto>(jobOrder!);
        if (validationResult is not null)
            return validationResult;

        // Save new reported issue
        var newReportedIssue = dto.ToEntity();
        newReportedIssue.JobOrderId = jobOrder!.Id;
        await _dbContext.ReportedIssues.AddAsync(newReportedIssue);
        await _dbContext.SaveChangesAsync();

        // Return new reported issue
        var result = newReportedIssue.ToReportedIssueDetailsDto();
        return ServiceResult<ReportedIssueDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<ReportedIssueDetailsDto>> UpdateAsync(
        string jobOrderNumber,
        int id,
        ReportedIssueUpdateDto dto)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<ReportedIssueDetailsDto>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status
        validationResult = JobOrderValidator.ValidateJobOrderStatus<ReportedIssueDetailsDto>(jobOrder!);
        if (validationResult is not null)
            return validationResult;

        // Find reported issue by ID and job order
        var reportedIssue = await _dbContext.ReportedIssues.FindAsync(id);

        // Validate reported issue exists
        validationResult = ReportedIssueValidator.ValidateReportedIssueExists<ReportedIssueDetailsDto>(reportedIssue, id);
        if (validationResult is not null)
            return validationResult;

        // Update reported issue description and timestamp
        reportedIssue!.Description = dto.Description;
        reportedIssue.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        // Return updated reported issue
        var result = reportedIssue.ToReportedIssueDetailsDto();
        return ServiceResult<ReportedIssueDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(string jobOrderNumber, int id)
    {
        // Find job order
        var jobOrder = await _dbContext.JobOrders.FirstOrDefaultAsync(jo => jo.JobOrderNumber == jobOrderNumber);

        // Validate job order exists
        var validationResult = JobOrderValidator.ValidateJobOrderExists<bool>(jobOrder, jobOrderNumber);
        if (validationResult is not null)
            return validationResult;

        // Validate job order status
        validationResult = JobOrderValidator.ValidateJobOrderStatus<bool>(jobOrder!);
        if (validationResult is not null)
            return validationResult;

        // Find reported issue within the specific job order
        var reportedIssue = await _dbContext.ReportedIssues
            .FirstOrDefaultAsync(ri => ri.Id == id && ri.JobOrderId == jobOrder!.Id);

        // Validate reported issue exists
        validationResult = ReportedIssueValidator.ValidateReportedIssueExists<bool>(reportedIssue, id);
        if (validationResult is not null)
            return validationResult;

        // Remove reported issue from database
        _dbContext.ReportedIssues.Remove(reportedIssue!);
        await _dbContext.SaveChangesAsync();

        // Return success result
        return ServiceResult<bool>.Ok(true);
    }
}
