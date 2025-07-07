using DriveOps.Shared.Enums;

namespace DriveOps.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public CustomerType Type { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public IndividualCustomer? IndividualCustomer { get; set; }
    public CompanyCustomer? CompanyCustomer { get; set; }
    public List<VehicleOwnership> VehicleOwnerships { get; set; } = [];
}
