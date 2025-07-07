namespace DriveOps.Api.Models;

public class VehicleOwnership
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public bool IsCurrentOwner { get; set; }
    public string? Notes { get; set; }

    public Vehicle Vechile { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
