using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.ReportedIssue;

public class ReportedIssueUpdateDto
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
