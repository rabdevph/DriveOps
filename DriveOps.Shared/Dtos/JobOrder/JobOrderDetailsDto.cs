using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.JobOrder;

public class JobOrderDetailsDto
{
    public int Id { get; set; }
    public string JobOrderNumber { get; set; } = string.Empty;
    public JobOrderStatus Status { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public int TechnicianId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public JobOrderCustomerDto? Customer { get; set; }
    public JobOrderVehicleDto? Vehicle { get; set; }
    public JobOrderTechnicianDto? Technician { get; set; }
}
