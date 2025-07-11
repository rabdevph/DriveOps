using DriveOps.Api.Results;
using Microsoft.AspNetCore.Mvc;

namespace DriveOps.Api.Controllers;

[ApiController]
public class ApiBaseController : ControllerBase
{
    protected IActionResult HandleServiceError<T>(ServiceResult<T> result)
    {
        var errorResponse = new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title = result.ErrorTitle,
            status = result.ResponseStatusCode,
            error = result.ErrorMessage
        };

        return result.ResponseStatusCode switch
        {
            StatusCodes.Status404NotFound => NotFound(errorResponse),
            StatusCodes.Status400BadRequest => BadRequest(errorResponse),
            _ => StatusCode(StatusCodes.Status500InternalServerError, errorResponse)
        };
    }
}
