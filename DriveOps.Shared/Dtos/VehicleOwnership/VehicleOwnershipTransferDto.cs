using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.VehicleOwnership;

public class VehicleOwnershipTransferDto
{
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int NewOwnerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public string? Notes { get; set; }
}
