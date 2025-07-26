using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.JobOrder;

public class JobOrderCustomerDto
{
    public CustomerType Type { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string DisplayName => Type switch
    {
        CustomerType.Individual => $"{FirstName} {LastName}".Trim(),
        CustomerType.Company => CompanyName ?? string.Empty,
        _ => string.Empty
    };
}
