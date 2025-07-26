namespace DriveOps.Shared.Dtos.JobOrder;

public class JobOrderDetailsPatchDto
{
    public int? CustomerId { get; set; }
    public int? VehicleId { get; set; }
    public int? TechnicianId { get; set; }
}
