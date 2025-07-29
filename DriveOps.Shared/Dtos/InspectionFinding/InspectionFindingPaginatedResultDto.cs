namespace DriveOps.Shared.Dtos.InspectionFinding;

public class InspectionFindingPaginatedResultDto<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; } = [];
}