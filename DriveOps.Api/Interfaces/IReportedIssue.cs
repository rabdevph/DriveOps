using DriveOps.Api.Models;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.ReportedIssue;

namespace DriveOps.Api.Interfaces;

public interface IReportedIssue
{
    Task<ServiceResult<ReportedIssuePaginatedResultDto<ReportedIssueDetailsDto>>> GetAllAsync(string jobOrderNumber, int page, int pageSize);
    Task<ServiceResult<ReportedIssueDetailsDto>> GetByIdAsync(string jobOrderNumber, int id);
    Task<ServiceResult<ReportedIssueDetailsDto>> CreateAsync(string jobOrderNumber, ReportedIssueCreateDto dto);
    Task<ServiceResult<ReportedIssueDetailsDto>> UpdateAsync(string jobOrderNumber, int id, ReportedIssueUpdateDto dto);
    Task<ServiceResult<bool>> DeleteAsync(string jobOrderNumber, int id);
}
