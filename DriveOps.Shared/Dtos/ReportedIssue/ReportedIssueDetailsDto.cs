namespace DriveOps.Shared.Dtos.ReportedIssue;

public class ReportedIssueDetailsDto
{
    public int Id { get; set; }
    public int JobOrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
