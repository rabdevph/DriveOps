using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.Technician;

public class TechnicianDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public TechnicianStatus Status { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
