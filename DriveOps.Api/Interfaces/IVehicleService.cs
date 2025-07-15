using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Vehicle;

namespace DriveOps.Api.Interfaces;

public interface IVehicleService
{
    Task<VehiclePaginatedResultDto<VehicleDetailsDto>> GetAllAsync(int page, int pageSize);
    Task<ServiceResult<VehicleDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<VehicleDetailsDto>> CreateAsync(VehicleCreateDto dto);
    Task<ServiceResult<VehicleDetailsDto>> UpdateDetailsAsync(int id, VehicleDetailsUpdateDto dto);
}
