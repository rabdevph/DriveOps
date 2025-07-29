using DriveOps.Api.Models;
using DriveOps.Api.Results;

namespace DriveOps.Api.Helpers;

public static class InspectionFindingValidator
{
    public static ServiceResult<T>? ValidateInspectionFindingExists<T>(InspectionFinding? inspectionFinding, int id)
    {
        if (inspectionFinding is null)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Inspection finding not found",
                errorMessage: $"No inspection finding found with ID [{id}]."
            );

        return null;
    }

    public static ServiceResult<T>? ValidateInspectionFindingBelongsToJobOrder<T>(InspectionFinding? inspectionFinding, int jobOrderId, int findingId)
    {
        if (inspectionFinding is null || inspectionFinding.JobOrderId != jobOrderId)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Inspection finding not found",
                errorMessage: $"No inspection finding found with ID [{findingId}] for the specified job order."
            );

        return null;
    }

    public static ServiceResult<T>? ValidateInspectionFindingNotResolved<T>(InspectionFinding inspectionFinding, int id, string operation)
    {
        if (inspectionFinding.IsResolved)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status409Conflict,
                errorTitle: "Inspection finding already resolved",
                errorMessage: $"Cannot {operation} inspection finding with ID [{id}] because it has already been resolved."
            );

        return null;
    }

    public static ServiceResult<T>? ValidateInspectionFindingCanBeModified<T>(InspectionFinding inspectionFinding, int id)
    {
        // Business rule: Only unresolved findings can be modified
        return ValidateInspectionFindingNotResolved<T>(inspectionFinding, id, "modify");
    }

    public static ServiceResult<T>? ValidateInspectionFindingCanBeDeleted<T>(InspectionFinding inspectionFinding, int id)
    {
        // Business rule: Only unresolved findings can be deleted
        return ValidateInspectionFindingNotResolved<T>(inspectionFinding, id, "delete");
    }

    public static ServiceResult<T>? ValidateSeverityDowngrade<T>(InspectionFinding existingFinding, Shared.Enums.FindingSeverity newSeverity)
    {
        // Business rule: Cannot downgrade severity from Critical to lower levels
        if (existingFinding.Severity == Shared.Enums.FindingSeverity.Critical &&
            newSeverity != Shared.Enums.FindingSeverity.Critical)
        {
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status400BadRequest,
                errorTitle: "Invalid severity change",
                errorMessage: "Cannot downgrade severity from Critical to a lower level. Critical findings must remain critical."
            );
        }

        return null;
    }
}
