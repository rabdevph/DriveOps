using DriveOps.Shared.Dtos.Vehicle;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.Customer;

public class CustomerDetailsDto
{
    public int Id { get; set; }
    public CustomerType Type { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public CustomerStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public CustomerIndividualDto? Individual { get; set; }
    public CustomerCompanyDto? Company { get; set; }

    public List<VehicleSummaryDto> OwnedVehicles { get; set; } = [];
}
