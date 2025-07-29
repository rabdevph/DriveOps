using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.InspectionFinding;

public class InspectionFindingDetailsDto
{
    public int Id { get; set; }
    public int JobOrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public FindingSeverity Severity { get; set; }
    public bool IsResolved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}