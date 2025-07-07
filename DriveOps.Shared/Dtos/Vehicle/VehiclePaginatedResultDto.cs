namespace DriveOps.Shared.Dtos.Vehicle;

public class VehiclePaginatedResultDto<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = [];
}
