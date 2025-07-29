using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.InspectionFinding;

public class InspectionFindingStatusUpdateDto
{
    [Required]
    public bool IsResolved { get; set; }
}