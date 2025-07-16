using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.VehicleOwnership;

namespace DriveOps.Api.Interfaces;

public interface IVehicleOwnershipService
{
    Task<ServiceResult<VehicleOwnershipDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<VehicleOwnershipDetailsDto>> CreateAsync(VehicleOwnershipCreateDto dto);
    Task<ServiceResult<VehicleOwnershipDetailsDto>> TransferAsync(VehicleOwnershipTransferDto dto);
}
