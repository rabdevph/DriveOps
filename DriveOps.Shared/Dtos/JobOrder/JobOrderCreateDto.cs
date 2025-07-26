using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.JobOrder;

public class JobOrderCreateDto
{
    [Required]
    public string JobOrderNumber { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [Required]
    public int TechnicianId { get; set; }
}
