using DriveOps.Shared.Enums;

namespace DriveOps.Api.Models;

public class Mechanic
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public MechanicStatus Status { get; set; } = MechanicStatus.Active;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
