using System.ComponentModel.DataAnnotations;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.Technician;

public class TechnicianStatusUpdateDto
{
    [Required]
    public TechnicianStatus Status { get; set; }
}
