using DriveOps.Api.Models;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.JobOrder;

namespace DriveOps.Api.Helpers;

public class JobOrderValidator
{
    public static ServiceResult<T>? ValidateJobOrderExists<T>(JobOrder? jobOrder, int id)
    {
        if (jobOrder is null)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Job order not found",
                errorMessage: $"No job order found with ID [{id}]."
            );

        return null;
    }

    public static ServiceResult<JobOrderDetailsDto>? ValidateJobOrderNumberIsUnique(bool isDuplicate)
    {
        if (isDuplicate)
            return ServiceResult<JobOrderDetailsDto>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Job order number already in exists",
                errorMessage: "A job order with the same number already exists."
            );

        return null;
    }
}
