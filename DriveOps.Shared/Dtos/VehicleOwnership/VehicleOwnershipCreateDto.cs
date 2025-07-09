using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.VehicleOwnership;

public class VehicleOwnershipCreateDto
{
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public string? Notes { get; set; }
}
