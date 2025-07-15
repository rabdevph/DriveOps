using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class VehicleService(DriveOpsContext dbContext) : IVehicleService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<VehiclePaginatedResultDto<VehicleDetailsDto>> GetAllAsync(int page, int pageSize)
    {
        // Ensure page and pageSize within valid bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Get total count and calculate pagination metadata
        var totalCount = await _dbContext.Vehicles.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Apply pagination and projec to DTOs
        var vehicles = await _dbContext.Vehicles
            .OrderBy(v => v.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(v => v.VehicleOwnerships)
                .ThenInclude(vo => vo.Customer)
                    .ThenInclude(c => c.IndividualCustomer)
            .Include(v => v.VehicleOwnerships)
                .ThenInclude(vo => vo.Customer)
                    .ThenInclude(c => c.CompanyCustomer)
            .Select(v => v.ToVehicleDetailsDto())
            .ToListAsync();

        return new VehiclePaginatedResultDto<VehicleDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = vehicles
        };
    }

    public async Task<ServiceResult<VehicleDetailsDto>> GetByIdAsync(int id)
    {
        // Retrieve vehicle with all related data
        var vehicle = await _dbContext.Vehicles
            .Include(v => v.VehicleOwnerships)
                .ThenInclude(vo => vo.Customer)
                    .ThenInclude(c => c.IndividualCustomer)
            .Include(v => v.VehicleOwnerships)
                .ThenInclude(vo => vo.Customer)
                    .ThenInclude(c => c.CompanyCustomer)
            .FirstOrDefaultAsync(v => v.Id == id);

        // Validate vehicle exists
        var validationResult = VehicleValidator.ValidateExistingVehicle(vehicle, id);
        if (validationResult is not null)
            return validationResult;

        // Return vehicle details
        var result = vehicle!.ToVehicleDetailsDto();
        return ServiceResult<VehicleDetailsDto>.Ok(result);

    }

    public async Task<ServiceResult<VehicleDetailsDto>> CreateAsync(VehicleCreateDto dto)
    {
        // Check for duplicate plate number or vin
        var vehicleDetailsExists = await _dbContext.Vehicles
            .AnyAsync(v => v.PlateNumber == dto.PlateNumber || v.Vin == dto.Vin);

        var validationResult = VehicleValidator.ValidateExistingDetails(vehicleDetailsExists);
        if (validationResult is not null)
            return validationResult;

        // Save new vehicle
        var vehicle = dto.ToEntity();
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var result = vehicle.ToVehicleDetailsDto();
        return ServiceResult<VehicleDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<VehicleDetailsDto>> UpdateDetailsAsync(int id, VehicleDetailsUpdateDto dto)
    {
        // Fetch vehicle
        var existingVehicle = await _dbContext.Vehicles.FindAsync(id);

        // Validate vehicle exists
        var validationResult = VehicleValidator.ValidateExistingVehicle(existingVehicle, id);
        if (validationResult is not null)
            return validationResult;

        // Check for detail conflicts with other vehicles
        var vehicleDetailsExists = await _dbContext.Vehicles
            .AnyAsync(v => v.Id != id && (v.PlateNumber == dto.PlateNumber || v.Vin == dto.Vin));

        validationResult = VehicleValidator.ValidateExistingDetails(vehicleDetailsExists);
        if (validationResult is not null)
            return validationResult;

        // Update fields
        existingVehicle!.PlateNumber = dto.PlateNumber;
        existingVehicle.Make = dto.Make;
        existingVehicle.Model = dto.Model;
        existingVehicle.Year = dto.Year;
        existingVehicle.Color = dto.Color;
        existingVehicle.Vin = dto.Vin;
        existingVehicle.Notes = dto.Notes;
        existingVehicle.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        var result = existingVehicle.ToVehicleDetailsDto();
        return ServiceResult<VehicleDetailsDto>.Ok(result);
    }
}
