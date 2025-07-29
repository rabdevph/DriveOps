using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.InspectionFinding;

namespace DriveOps.Api.Interfaces;

public interface IInspectionFindingService
{
    Task<ServiceResult<InspectionFindingPaginatedResultDto<InspectionFindingDetailsDto>>> GetAllAsync(string jobOrderNumber, int page, int pageSize);
    Task<ServiceResult<InspectionFindingDetailsDto>> GetByIdAsync(string jobOrderNumber, int id);
    Task<ServiceResult<InspectionFindingDetailsDto>> CreateAsync(string jobOrderNumber, InspectionFindingCreateDto dto);
    Task<ServiceResult<InspectionFindingDetailsDto>> UpdateAsync(string jobOrderNumber, int id, InspectionFindingUpdateDto dto);
    Task<ServiceResult<InspectionFindingDetailsDto>> UpdateStatusAsync(string jobOrderNumber, int id, InspectionFindingStatusUpdateDto dto);
    Task<ServiceResult<bool>> DeleteAsync(string jobOrderNumber, int id);
}
