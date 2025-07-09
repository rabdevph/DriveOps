using DriveOps.Api.Data;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Shared.Dtos.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class VehicleService(DriveOpsContext dbContext) : IVehicleService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<VehiclePaginatedResultDto<VehicleDetailsDto>> GetAllAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var totalCount = await _dbContext.Vehicles.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

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

    public async Task<VehicleDetailsDto?> GetByIdAsync(int id)
    {
        var vehicle = await _dbContext.Vehicles
            .Include(v => v.VehicleOwnerships)
                .ThenInclude(vo => vo.Customer)
                    .ThenInclude(c => c.IndividualCustomer)
            .Include(v => v.VehicleOwnerships)
                .ThenInclude(vo => vo.Customer)
                    .ThenInclude(c => c.CompanyCustomer)
            .FirstOrDefaultAsync(v => v.Id == id);

        return vehicle?.ToVehicleDetailsDto();
    }

    public async Task<VehicleDetailsDto> CreateAsync(VehicleCreateDto dto)
    {
        var vehicle = dto.ToEntity();
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        return vehicle.ToVehicleDetailsDto();
    }

    public async Task<VehicleDetailsDto?> UpdateDetailsAsync(int id, VehicleDetailsUpdateDto dto)
    {
        var existingVehicle = await _dbContext.Vehicles.FindAsync(id);

        if (existingVehicle is null)
            return null;

        existingVehicle.PlateNumber = dto.PlateNumber;
        existingVehicle.Make = dto.Make;
        existingVehicle.Model = dto.Model;
        existingVehicle.Year = dto.Year;
        existingVehicle.Color = dto.Color;
        existingVehicle.Vin = dto.Vin;
        existingVehicle.Notes = dto.Notes;
        existingVehicle.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return existingVehicle.ToVehicleDetailsDto();
    }
}
