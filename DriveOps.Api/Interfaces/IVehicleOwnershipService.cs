using DriveOps.Shared.Dtos.VehicleOwnership;

namespace DriveOps.Api.Interfaces;

public interface IVehicleOwnershipService
{
    Task<VehicleOwnershipDetailsDto?> GetByIdAsync(int id);
    Task<VehicleOwnershipDetailsDto> CreateAsync(VehicleOwnershipCreateDto dto);
    Task<VehicleOwnershipDetailsDto> TransferAsync(VehicleOwnershipTransferDto dto);
}
