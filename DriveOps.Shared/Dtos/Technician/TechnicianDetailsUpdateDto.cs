using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.Technician;

public class TechnicianDetailsUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Specialization { get; set; }
}
