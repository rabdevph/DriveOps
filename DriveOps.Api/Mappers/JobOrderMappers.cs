using DriveOps.Api.Models;
using DriveOps.Shared.Dtos.JobOrder;

namespace DriveOps.Api.Mappers;

public static class JobOrderMappers
{
    public static JobOrder ToEntity(this JobOrderCreateDto dto)
    {
        var jobOrder = new JobOrder
        {
            JobOrderNumber = dto.JobOrderNumber,
            CustomerId = dto.CustomerId,
            VehicleId = dto.VehicleId,
            TechnicianId = dto.TechnicianId
        };

        return jobOrder;
    }

    public static JobOrderDetailsDto ToJobOrderDetailsDto(this JobOrder jobOrder)
    {
        return new JobOrderDetailsDto
        {
            Id = jobOrder.Id,
            JobOrderNumber = jobOrder.JobOrderNumber,
            Status = jobOrder.Status,
            CustomerId = jobOrder.CustomerId,
            VehicleId = jobOrder.VehicleId,
            TechnicianId = jobOrder.TechnicianId,
            CreatedAt = jobOrder.CreatedAt,
            UpdatedAt = jobOrder.UpdatedAt,
            Customer = jobOrder.Customer == null ? null : new JobOrderCustomerDto
            {
                Type = jobOrder.Customer.Type,
                FirstName = jobOrder.Customer.IndividualCustomer?.FirstName,
                LastName = jobOrder.Customer.IndividualCustomer?.LastName,
                CompanyName = jobOrder.Customer.CompanyCustomer?.CompanyName,
            },
            Vehicle = jobOrder.Vehicle == null ? null : new JobOrderVehicleDto
            {
                PlateNumber = jobOrder.Vehicle.PlateNumber,
                Make = jobOrder.Vehicle.Make,
                Model = jobOrder.Vehicle.Model
            },
            Technician = jobOrder.Technician == null ? null : new JobOrderTechnicianDto
            {
                Name = jobOrder.Technician.FullName
            }
        };
    }
}
