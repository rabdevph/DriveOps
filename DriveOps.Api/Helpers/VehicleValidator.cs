using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.Vehicle;

namespace DriveOps.Api.Helpers;

public static class VehicleValidator
{
    public static ServiceResult<VehicleDetailsDto>? ValidateExistingDetails(bool isDuplicate)
    {
        if (isDuplicate)
            return ServiceResult<VehicleDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Duplicate vehicle details",
                errorMessage: "Vehicle with the same plate number or VIN already exists."
            );

        return null;
    }

    public static ServiceResult<VehicleDetailsDto>? ValidateExistingVehicle(object? resource, int id)
    {
        if (resource is null)
            return ServiceResult<VehicleDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Vehicle not found.",
                errorMessage: $"Vehicle with ID[{id}] does not exists."
            );

        return null;
    }
}
