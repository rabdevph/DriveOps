using System.ComponentModel.DataAnnotations;

namespace DriveOps.Shared.Dtos.Vehicle;

public class VehicleDetailsUpdateDto
{
    [Required]
    [MaxLength(20)]
    public string PlateNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? Color { get; set; }

    [Required]
    [MaxLength(50)]
    public string Vin { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
