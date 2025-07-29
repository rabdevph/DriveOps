using DriveOps.Api.Models;
using DriveOps.Shared.Dtos.InspectionFinding;

namespace DriveOps.Api.Mappers;

public static class InspectionFindingMappers
{
    public static InspectionFinding ToEntity(this InspectionFindingCreateDto dto)
    {
        var inspectionFinding = new InspectionFinding
        {
            Description = dto.Description,
            Recommendation = dto.Recommendation,
            Severity = dto.Severity
        };

        return inspectionFinding;
    }

    public static InspectionFindingDetailsDto ToInspectionFindingDetailsDto(this InspectionFinding inspectionFinding)
    {
        return new InspectionFindingDetailsDto
        {
            Id = inspectionFinding.Id,
            JobOrderId = inspectionFinding.JobOrderId,
            Description = inspectionFinding.Description,
            Recommendation = inspectionFinding.Recommendation,
            Severity = inspectionFinding.Severity,
            IsResolved = inspectionFinding.IsResolved,
            CreatedAt = inspectionFinding.CreatedAt,
            UpdatedAt = inspectionFinding.UpdatedAt
        };
    }
}
