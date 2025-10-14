using webcore_backend.Features.Guilds.AuditLogs.Models;

namespace webcore_backend.Features.Guilds.AuditLogs.Entities;

public class GuildModerationActionEntity
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public Guid TargetUserId { get; set; }
    public Guid ModeratorUserId { get; set; }

    public ModerationActionType ActionType { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
}