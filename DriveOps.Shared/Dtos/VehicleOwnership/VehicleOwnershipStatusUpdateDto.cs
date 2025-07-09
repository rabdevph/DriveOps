using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.VehicleOwnership;

public class VehicleOwnershipStatusUpdateDto
{
    [Required]
    public bool IsCurrentOwner { get; set; }
}
