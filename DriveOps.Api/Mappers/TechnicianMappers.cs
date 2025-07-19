using DriveOps.Api.Models;
using DriveOps.Shared.Dtos.Technician;

namespace DriveOps.Api.Mappers;

public static class TechnicianMappers
{
    public static Technician ToEntity(this TechnicianCreateDto dto)
    {
        var Technician = new Technician
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Specialization = dto.Specialization,
        };

        return Technician;
    }

    public static TechnicianDetailsDto ToTechnicianDetailsDto(this Technician Technician)
    {
        return new TechnicianDetailsDto
        {
            Id = Technician.Id,
            FullName = Technician.FullName,
            PhoneNumber = Technician.PhoneNumber,
            Specialization = Technician.Specialization,
            Status = Technician.Status,
            RegisteredAt = Technician.RegisteredAt,
            UpdatedAt = Technician.UpdatedAt
        };
    }
}
