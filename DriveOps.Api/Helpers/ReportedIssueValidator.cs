using DriveOps.Api.Models;
using DriveOps.Api.Results;

namespace DriveOps.Api.Helpers;

public class ReportedIssueValidator
{
    public static ServiceResult<T>? ValidateReportedIssueExists<T>(ReportedIssue? reportedIssue, int id)
    {
        if (reportedIssue is null)
            return ServiceResult<T>.Fail(
                responseStatusCode: StatusCodes.Status404NotFound,
                errorTitle: "Reported issue not found",
                errorMessage: $"No reported issue found with ID [{id}]."
            );

        return null;
    }
}
