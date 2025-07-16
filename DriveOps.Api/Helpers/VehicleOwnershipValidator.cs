using DriveOps.Api.Models;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.VehicleOwnership;

namespace DriveOps.Api.Helpers;

public static class VehicleOwnershipValidator
{
    public static ServiceResult<VehicleOwnershipDetailsDto>? ValidateDuplicateCurrentOwnership(bool hasCurrentOwner)
    {
        if (hasCurrentOwner)
            return ServiceResult<VehicleOwnershipDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Current ownership conflict.",
                errorMessage: "This vehicle is already marked as currently owned. End the previous ownership before assigning a new owner."
            );

        return null;
    }

    public static ServiceResult<VehicleOwnershipDetailsDto>? ValidateExistingOwnership(VehicleOwnership? ownership)
    {
        if (ownership is null)
            return ServiceResult<VehicleOwnershipDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "No ownership to transfer.",
                errorMessage: "Cannot transfer ownership. The vehicle currently has no registered owner."
            );

        return null;
    }

    public static ServiceResult<VehicleOwnershipDetailsDto>? ValidateExistingOwnership(VehicleOwnership? ownership, int id)
    {
        if (ownership is null)
            return ServiceResult<VehicleOwnershipDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Ownership record not found.",
                errorMessage: $"No vehicle ownership record found with ID [{id}]."
            );

        return null;
    }
}
