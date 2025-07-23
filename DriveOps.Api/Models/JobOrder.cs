using DriveOps.Shared.Enums;

namespace DriveOps.Api.Models;

public class JobOrder
{
    public int Id { get; set; }
    public string JobOrderNumber { get; set; } = string.Empty;
    public JobOrderStatus Status { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public int TechnicianId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Customer? Customer { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Technician? Technician { get; set; }

    public List<ReportedIssue> Issues { get; set; } = [];
    public List<InspectionFinding> Findings { get; set; } = [];
}
