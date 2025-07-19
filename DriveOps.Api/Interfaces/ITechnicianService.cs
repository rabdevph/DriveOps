using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Technician;

namespace DriveOps.Api.Interfaces;

public interface ITechnicianService
{
    Task<TechnicianPaginatedResultDto<TechnicianDetailsDto>> GetAllAsync(int page, int pageSize);
    Task<ServiceResult<TechnicianDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<TechnicianDetailsDto>> CreateAsync(TechnicianCreateDto dto);
    Task<ServiceResult<TechnicianDetailsDto>> UpdateDetailsAsync(int id, TechnicianDetailsUpdateDto dto);
    Task<ServiceResult<TechnicianDetailsDto>> UpdateStatusAsync(int id, TechnicianStatusUpdateDto dto);
}
