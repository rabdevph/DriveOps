using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.ReportedIssue;

public class ReportedIssueCreateDto
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
