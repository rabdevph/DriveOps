using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.JobOrder;
using DriveOps.Shared.Enums;

namespace DriveOps.Api.Interfaces;

public interface IJobOrderService
{
    Task<JobOrderPaginatedResultDto<JobOrderDetailsDto>> GetAllAsync(JobOrderStatus? status, int? customerId, int page, int pageSize);
    Task<ServiceResult<JobOrderDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<JobOrderDetailsDto>> CreateAsync(JobOrderCreateDto dto);
    Task<ServiceResult<JobOrderDetailsDto>> PatchDetailsAsync(int id, JobOrderDetailsPatchDto dto);
    Task<ServiceResult<JobOrderDetailsDto>> UpdateStatusAsync(int id, JobOrderStatusUpdateDto dto);
}
