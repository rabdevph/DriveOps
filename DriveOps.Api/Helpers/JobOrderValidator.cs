using DriveOps.Api.Models;
using DriveOps.Api.Results;
using DriveOps.Shared.Dtos.JobOrder;
using DriveOps.Shared.Enums;

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

    public static ServiceResult<T>? ValidateJobOrderExists<T>(JobOrder? jobOrder, string jobOrderNumber)
    {
        if (jobOrder is null)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Job order not found",
                errorMessage: $"No job order found with number [{jobOrderNumber}]."
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

    public static ServiceResult<T>? ValidateJobOrderStatusForOperation<T>(JobOrder jobOrder, string operation, string entityType)
    {
        if (jobOrder.Status == JobOrderStatus.Completed || jobOrder.Status == JobOrderStatus.Cancelled)
        {
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status409Conflict,
                errorTitle: $"Cannot {operation} {entityType}",
                errorMessage: $"Job order is {jobOrder.Status}, cannot {operation} {entityType}."
            );
        }

        return null;
    }
}
