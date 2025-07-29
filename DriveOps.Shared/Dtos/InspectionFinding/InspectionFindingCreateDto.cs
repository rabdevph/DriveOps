using System.ComponentModel.DataAnnotations;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.InspectionFinding;

public class InspectionFindingCreateDto
{
    [Required]
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(1024)]
    public string Recommendation { get; set; } = string.Empty;

    [Required]
    public FindingSeverity Severity { get; set; }
}