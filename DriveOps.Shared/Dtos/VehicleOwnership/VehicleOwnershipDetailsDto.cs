namespace DriveOps.Shared.Dtos.VehicleOwnership;

public class VehicleOwnershipDetailsDto
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public string? Notes { get; set; }
}
