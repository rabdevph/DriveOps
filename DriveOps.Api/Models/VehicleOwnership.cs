namespace DriveOps.Api.Models;

public class VehicleOwnership
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public DateTime? OwnershipStartDate { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? OwnershipEndDate { get; set; }
    public string? Notes { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
