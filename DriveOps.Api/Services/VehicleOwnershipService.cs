using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.VehicleOwnership;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class VehicleOwnershipService(DriveOpsContext dbContext) : IVehicleOwnershipService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<ServiceResult<VehicleOwnershipDetailsDto>> GetByIdAsync(int id)
    {
        // Retrieve vehicle ownership
        var ownership = await _dbContext.VehicleOwnerships.FindAsync(id);

        // Validate ownership exists
        var validationResult = VehicleOwnershipValidator.ValidateExistingOwnership(ownership, id);
        if (validationResult is not null)
            return validationResult;

        // Return ownership details
        var result = ownership!.ToVehicleOwnershipDetailsDto();
        return ServiceResult<VehicleOwnershipDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<VehicleOwnershipDetailsDto>> CreateAsync(VehicleOwnershipCreateDto dto)
    {
        // Validate customer exists
        var customer = await _dbContext.Customers.FindAsync(dto.CustomerId);

        var validationResult = CustomerValidator.ValidateExistingCustomer<VehicleOwnershipDetailsDto>(customer, dto.CustomerId);
        if (validationResult is not null)
            return validationResult;

        // Validate vehicle exists
        var vehicle = await _dbContext.Vehicles.FindAsync(dto.VehicleId);

        validationResult = VehicleValidator.ValidateExistingVehicle<VehicleOwnershipDetailsDto>(vehicle, dto.VehicleId);
        if (validationResult is not null)
            return validationResult;

        // Check for existing ownership
        var hasCurrentOwner = await _dbContext.VehicleOwnerships
            .AnyAsync(vo => vo.VehicleId == dto.VehicleId && vo.IsCurrentOwner);

        validationResult = VehicleOwnershipValidator.ValidateDuplicateCurrentOwnership(hasCurrentOwner);
        if (validationResult is not null)
            return validationResult;

        // Save new ownership
        var ownership = dto.ToEntity();
        await _dbContext.VehicleOwnerships.AddAsync(ownership);
        await _dbContext.SaveChangesAsync();

        var result = ownership.ToVehicleOwnershipDetailsDto();
        return ServiceResult<VehicleOwnershipDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<VehicleOwnershipDetailsDto>> TransferAsync(VehicleOwnershipTransferDto dto)
    {
        // Validate vehicle have ownership
        var vehicle = await _dbContext.Vehicles
            .Include(v => v.VehicleOwnerships)
            .FirstOrDefaultAsync(v => v.Id == dto.VehicleId);

        var validationResult = VehicleValidator.ValidateExistingVehicle<VehicleOwnershipDetailsDto>(vehicle, dto.VehicleId);
        if (validationResult is not null)
            return validationResult;

        // Validate customer(new owner) exists
        var customer = await _dbContext.Customers.FindAsync(dto.NewOwnerId);

        validationResult = CustomerValidator.ValidateExistingCustomer<VehicleOwnershipDetailsDto>(customer, dto.NewOwnerId);
        if (validationResult is not null)
            return validationResult;

        // Get current ownership
        var currentOwnership = vehicle!.VehicleOwnerships.FirstOrDefault(vo => vo.IsCurrentOwner);

        // Validate current ownership exists
        validationResult = VehicleOwnershipValidator.ValidateExistingOwnership(currentOwnership);
        if (validationResult is not null)
            return validationResult;

        // Set current ownership to false
        currentOwnership!.IsCurrentOwner = false;

        // Transfer ownership
        var ownership = dto.ToEntity();
        await _dbContext.VehicleOwnerships.AddAsync(ownership);
        await _dbContext.SaveChangesAsync();

        var result = ownership.ToVehicleOwnershipDetailsDto();
        return ServiceResult<VehicleOwnershipDetailsDto>.Ok(result);
    }
}
