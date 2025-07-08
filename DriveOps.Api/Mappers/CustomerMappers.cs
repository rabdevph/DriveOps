using DriveOps.Api.Models;
using DriveOps.Shared.Dtos;
using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Dtos.Vehicle;
using DriveOps.Shared.Enums;

namespace DriveOps.Api.Mappers;

public static class CustomerMappers
{
    public static Customer ToEntity(this CustomerCreateDto dto)
    {
        var customer = new Customer
        {
            Type = dto.Type,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Notes = dto.Notes,
            IndividualCustomer = dto.Type == CustomerType.Individual && dto.Individual is not null
                ? new IndividualCustomer
                {
                    FirstName = dto.Individual.FirstName,
                    LastName = dto.Individual.LastName
                }
                :
                null,
            CompanyCustomer = dto.Type == CustomerType.Company && dto.Company is not null
                ? new CompanyCustomer
                {
                    CompanyName = dto.Company.CompanyName,
                    ContactPerson = dto.Company.ContactPerson,
                    Position = dto.Company.Position
                }
                :
                null
        };

        return customer;
    }

    public static CustomerDetailsDto ToCustomerDetailsDto(this Customer customer)
    {
        return new CustomerDetailsDto
        {
            Id = customer.Id,
            Type = customer.Type,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            Notes = customer.Notes,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            Individual = customer.IndividualCustomer is not null
                ? new CustomerIndividualDto
                {
                    FirstName = customer.IndividualCustomer.FirstName,
                    LastName = customer.IndividualCustomer.LastName
                }
                :
                null,
            Company = customer.CompanyCustomer is not null
                ? new CustomerCompanyDto
                {
                    CompanyName = customer.CompanyCustomer.CompanyName,
                    ContactPerson = customer.CompanyCustomer.ContactPerson,
                    Position = customer.CompanyCustomer.Position
                }
                :
                null,
            OwnedVehicles = []
        };
    }

    public static CustomerDetailsDto ToCustomerDetailsDto(this Customer customer, bool onlyCurrent = false)
    {
        var ownerships = onlyCurrent
            ? customer.VehicleOwnerships.Where(vo => vo.IsCurrentOwner)
            : customer.VehicleOwnerships;

        return new CustomerDetailsDto
        {
            Id = customer.Id,
            Type = customer.Type,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            Notes = customer.Notes,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            Individual = customer.IndividualCustomer is not null
                ? new CustomerIndividualDto
                {
                    FirstName = customer.IndividualCustomer.FirstName,
                    LastName = customer.IndividualCustomer.LastName
                }
                :
                null,
            Company = customer.CompanyCustomer is not null
                ? new CustomerCompanyDto
                {
                    CompanyName = customer.CompanyCustomer.CompanyName,
                    ContactPerson = customer.CompanyCustomer.ContactPerson,
                    Position = customer.CompanyCustomer.Position
                }
                :
                null,
            OwnedVehicles = [.. ownerships
                .Select(vo => new VehicleSummaryDto
                {
                    Id = vo.Vehicle.Id,
                    PlateNumber = vo.Vehicle.PlateNumber,
                    Make = vo.Vehicle.Make,
                    Model = vo.Vehicle.Model,
                    Year = vo.Vehicle.Year,
                    Color = vo.Vehicle.Color,
                    IsCurrentOwner = vo.IsCurrentOwner
                })]
        };
    }
}
