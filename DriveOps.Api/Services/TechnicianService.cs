using DriveOps.Api.Data;
using DriveOps.Api.Helpers;
using DriveOps.Api.Interfaces;
using DriveOps.Api.Mappers;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Technician;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Services;

public class TechnicianService(DriveOpsContext dbContext) : ITechnicianService
{
    private readonly DriveOpsContext _dbContext = dbContext;

    public async Task<TechnicianPaginatedResultDto<TechnicianDetailsDto>> GetAllAsync(int page, int pageSize)
    {
        // Ensure page and pageSize are within valid bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Get total count and calculate pagination metadata
        var totalCount = await _dbContext.Technicians.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Apply pagination and project to DTOs
        var technicians = await _dbContext.Technicians
            .OrderBy(m => m.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => m.ToTechnicianDetailsDto())
            .ToListAsync();

        return new TechnicianPaginatedResultDto<TechnicianDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = technicians
        };
    }

    public async Task<ServiceResult<TechnicianDetailsDto>> GetByIdAsync(int id)
    {
        // Retrieve technician
        var technician = await _dbContext.Technicians.FirstOrDefaultAsync(m => m.Id == id);

        // Validate technician exists
        var validationResult = TechnicianValidator.ValidateTechnicianExists<TechnicianDetailsDto>(technician, id);
        if (validationResult is not null)
            return validationResult;

        // Return technician details
        var result = technician!.ToTechnicianDetailsDto();
        return ServiceResult<TechnicianDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<TechnicianDetailsDto>> CreateAsync(TechnicianCreateDto dto)
    {
        // Check for duplicate full name
        var isDuplicateFullName = await _dbContext.Technicians
            .AnyAsync(m => m.FullName == dto.FullName);

        var validationResult = TechnicianValidator.ValidateTechnicianIsUnique<TechnicianDetailsDto>(isDuplicateFullName, dto.FullName);
        if (validationResult is not null)
            return validationResult;

        // Check for duplicate phone number
        var phoneNumberExists = await _dbContext.Technicians
            .AnyAsync(m => m.PhoneNumber == dto.PhoneNumber);

        validationResult = TechnicianValidator.ValidatePhoneNumberIsUnique(phoneNumberExists);
        if (validationResult is not null)
            return validationResult;

        // Save new technician
        var technician = dto.ToEntity();
        await _dbContext.Technicians.AddAsync(technician);
        await _dbContext.SaveChangesAsync();

        var result = technician.ToTechnicianDetailsDto();
        return ServiceResult<TechnicianDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<TechnicianDetailsDto>> UpdateDetailsAsync(int id, TechnicianDetailsUpdateDto dto)
    {
        // Fetch technician
        var technician = await _dbContext.Technicians.FindAsync(id);

        // Validate technician exists
        var validationResult = TechnicianValidator.ValidateTechnicianExists<TechnicianDetailsDto>(technician, id);
        if (validationResult is not null)
            return validationResult;

        // Check for duplicate full name
        var isDuplicateFullName = await _dbContext.Technicians
            .AnyAsync(m => m.Id != id && m.FullName == dto.FullName);

        validationResult = TechnicianValidator.ValidateTechnicianIsUnique<TechnicianDetailsDto>(isDuplicateFullName, dto.FullName);
        if (validationResult is not null)
            return validationResult;

        // Check for duplicate phone number
        var phoneNumberExists = await _dbContext.Technicians
            .AnyAsync(m => m.Id != id && m.PhoneNumber == dto.PhoneNumber);

        validationResult = TechnicianValidator.ValidatePhoneNumberIsUnique(phoneNumberExists);
        if (validationResult is not null)
            return validationResult;

        // Update fields
        technician!.FullName = dto.FullName;
        technician.PhoneNumber = dto.PhoneNumber;
        technician.Specialization = dto.Specialization;

        technician.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        var result = technician.ToTechnicianDetailsDto();
        return ServiceResult<TechnicianDetailsDto>.Ok(result);
    }

    public async Task<ServiceResult<TechnicianDetailsDto>> UpdateStatusAsync(int id, TechnicianStatusUpdateDto dto)
    {
        // Fetch technician
        var technician = await _dbContext.Technicians.FindAsync(id);

        // Validate technician exists
        var validationResult = TechnicianValidator.ValidateTechnicianExists<TechnicianDetailsDto>(technician, id);
        if (validationResult is not null)
            return validationResult;

        // Update status
        technician!.Status = dto.Status;

        technician.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        var result = technician.ToTechnicianDetailsDto();
        return ServiceResult<TechnicianDetailsDto>.Ok(result);
    }
}
