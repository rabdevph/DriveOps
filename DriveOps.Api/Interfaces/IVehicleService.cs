using DriveOps.Shared.Dtos.Vehicle;

namespace DriveOps.Api.Interfaces;

public interface IVehicleService
{
    Task<VehiclePaginatedResultDto<VehicleDetailsDto>> GetAllAsync(int page, int pageSize);
    Task<VehicleDetailsDto?> GetByIdAsync(int id);
    Task<VehicleDetailsDto> CreateAsync(VehicleCreateDto dto);
    Task<VehicleDetailsDto?> UpdateDetailsAsync(int id, VehicleDetailsUpdateDto dto);
}
