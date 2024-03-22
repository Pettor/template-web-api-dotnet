using Backend.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Infrastructure.Auditing;

public class AuditTrail(EntityEntry entry, ISerializerService serializer)
{
    public EntityEntry Entry { get; } = entry;
    public Guid UserId { get; set; }
    public string? TableName { get; set; }
    public Dictionary<string, object?> KeyValues { get; } = new();
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();
    public TrailType TrailType { get; set; }
    public List<string> ChangedColumns { get; } = new();
    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

    public Trail ToAuditTrail() =>
        new()
        {
            UserId = UserId,
            Type = TrailType.ToString(),
            TableName = TableName,
            DateTime = DateTime.UtcNow,
            PrimaryKey = serializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : serializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : serializer.Serialize(NewValues),
            AffectedColumns = ChangedColumns.Count == 0 ? null : serializer.Serialize(ChangedColumns)
        };
}
