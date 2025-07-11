namespace DriveOps.Api.Results;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public int ResponseStatusCode { get; set; }
    public string? ErrorTitle { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }

    public static ServiceResult<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static ServiceResult<T> Fail(int responseStatusCode, string errorTitle, string errorMessage) =>
        new() { Success = false, ResponseStatusCode = responseStatusCode, ErrorTitle = errorTitle, ErrorMessage = errorMessage };
}
