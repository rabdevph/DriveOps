namespace DriveOps.Shared.Dtos.VehicleOwnership;

public class VehicleOwnershipTransferDto
{
    public int VehicleId { get; set; }
    public int NewCustomerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public string? Notes { get; set; }
}
