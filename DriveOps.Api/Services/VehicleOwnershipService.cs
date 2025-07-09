using DriveOps.Api.Data;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Shared.Dtos.VehicleOwnership;

namespace DriveOps.Api.Services;

public class VehicleOwnershipService(DriveOpsContext dbContext) : IVehicleOwnershipService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<VehicleOwnershipDetailsDto?> GetByIdAsync(int id)
    {
        var ownership = await _dbContext.VehicleOwnerships.FindAsync(id);

        if (ownership is null)
            return null;

        return ownership?.ToVehicleOwnershipDetailsDto();
    }

    public async Task<VehicleOwnershipDetailsDto> CreateAsync(VehicleOwnershipCreateDto dto)
    {
        var ownership = dto.ToEntity();
        await _dbContext.VehicleOwnerships.AddAsync(ownership);
        await _dbContext.SaveChangesAsync();

        return ownership.ToVehicleOwnershipDetailsDto();
    }

    public async Task<VehicleOwnershipDetailsDto> TransferAsync(VehicleOwnershipTransferDto dto)
    {
        var ownership = dto.ToEntity();
        await _dbContext.VehicleOwnerships.AddAsync(ownership);
        await _dbContext.SaveChangesAsync();

        return ownership.ToVehicleOwnershipDetailsDto();
    }
}
