using DriveOps.Shared.Enums;

namespace DriveOps.Api.Models;

public class InspectionFinding
{
    public int Id { get; set; }
    public int JobOrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Recommendation { get; set; }
    public FindingSeverity Severity { get; set; }
    public bool IsResolved { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public JobOrder JobOrder { get; set; } = null!;
}
