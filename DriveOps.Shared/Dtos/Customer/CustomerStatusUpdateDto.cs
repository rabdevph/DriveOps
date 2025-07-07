using System.ComponentModel.DataAnnotations;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.Customer;

public class CustomerStatusUpdateDto
{
    [Required]
    public CustomerStatus Status { get; set; }
}
