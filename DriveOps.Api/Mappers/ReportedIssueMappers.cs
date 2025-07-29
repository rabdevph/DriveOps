using DriveOps.Api.Models;
using DriveOps.Shared.Dtos.ReportedIssue;

namespace DriveOps.Api.Mappers;

public static class ReportedIssueMappers
{
    public static ReportedIssue ToEntity(this ReportedIssueCreateDto dto)
    {
        var reportedIssue = new ReportedIssue
        {
            Description = dto.Description
        };

        return reportedIssue;
    }

    public static ReportedIssueDetailsDto ToReportedIssueDetailsDto(this ReportedIssue reportedIssue)
    {
        return new ReportedIssueDetailsDto
        {
            Id = reportedIssue.Id,
            JobOrderId = reportedIssue.JobOrderId,
            Description = reportedIssue.Description,
            CreatedAt = reportedIssue.CreatedAt,
            UpdatedAt = reportedIssue.UpdatedAt
        };
    }
}
