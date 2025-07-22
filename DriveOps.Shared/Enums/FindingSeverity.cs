using System.Text.Json.Serialization;

namespace DriveOps.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FindingSeverity
{
    Minor,
    Moderate,
    Critical
}
