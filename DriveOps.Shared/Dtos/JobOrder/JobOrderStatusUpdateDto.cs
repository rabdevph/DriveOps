using System.ComponentModel.DataAnnotations;
using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.JobOrder;

public class JobOrderStatusUpdateDto
{
    [Required]
    public JobOrderStatus Status { get; set; }
}
