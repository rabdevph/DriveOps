using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Enums;

namespace DriveOps.Api.Helpers;

public static class CustomerValidator
{
    public static ServiceResult<CustomerDetailsDto>? ValidateSubtypeDetails<T>(T dto)
    where T : ICustomerTypeDto
    {
        if (dto.Type == CustomerType.Individual &&
            (
                dto.Individual is null ||
                string.IsNullOrEmpty(dto.Individual.FirstName) ||
                string.IsNullOrEmpty(dto.Individual.LastName)
            ))
        {
            return ServiceResult<CustomerDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Missing required customer subtype details.",
                errorMessage: "Individual customer details are required."
            );
        }

        if (dto.Type == CustomerType.Company &&
            (
                dto.Company is null ||
                string.IsNullOrEmpty(dto.Company.CompanyName)
            ))
        {
            return ServiceResult<CustomerDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Missing required customer subtype details.",
                errorMessage: "Company customer details are required."
            );
        }

        return null;
    }

    public static ServiceResult<CustomerDetailsDto>? ValidateExistingContacts(bool isDuplicate)
    {
        if (isDuplicate)
            return ServiceResult<CustomerDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Duplicate contact details.",
                errorMessage: "Customer with the same contact details already exists.");

        return null;
    }

    public static ServiceResult<CustomerDetailsDto>? ValidateExistingCustomer(object? resource, int id)
    {
        if (resource is null)
            return ServiceResult<CustomerDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Customer not found.",
                errorMessage: $"Customer with ID[{id}] does not exists."
            );

        return null;
    }
}
