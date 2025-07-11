using System.ComponentModel.DataAnnotations;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.Customer;

public class CustomerDetailsUpdateDto : ICustomerTypeDto
{
    [Required]
    public CustomerType Type { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(512)]
    public string? Address { get; set; }

    [MaxLength(1024)]
    public string? Notes { get; set; }

    public CustomerIndividualDto? Individual { get; set; }
    public CustomerCompanyDto? Company { get; set; }
}
