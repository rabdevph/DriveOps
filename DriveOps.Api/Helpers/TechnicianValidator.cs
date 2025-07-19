using DriveOps.Api.Models;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Technician;

namespace DriveOps.Api.Helpers;

public static class TechnicianValidator
{
    public static ServiceResult<TechnicianDetailsDto>? ValidatePhoneNumberIsUnique(bool isDuplicate)
    {
        if (isDuplicate)
            return ServiceResult<TechnicianDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Phone number already in use",
                errorMessage: "Another technician is already registered with this phone number."
            );

        return null;
    }

    public static ServiceResult<T>? ValidateTechnicianExists<T>(Technician? Technician, int id)
    {
        if (Technician is null)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Technician not found",
                errorMessage: $"No technician found with ID [{id}]."
            );

        return null;
    }

    public static ServiceResult<T>? ValidateTechnicianIsUnique<T>(bool isDuplicate, string fullName)
    {
        if (isDuplicate)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status409Conflict,
                errorTitle: "Duplicate Technician record",
                errorMessage: $"A technician named {fullName} is already registered."
            );

        return null;
    }
}
