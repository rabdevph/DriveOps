namespace DriveOps.Shared.Dtos.VehicleOwnership;

public class VehicleOwnershipCreateDto
{
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public string? Notes { get; set; }
}
