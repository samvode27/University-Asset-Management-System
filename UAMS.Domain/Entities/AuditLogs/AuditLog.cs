using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AuditLogs;

public class AuditLog : AuditableEntity
{
    private AuditLog()
    {
    }


    public Guid? UserId { get; private set; }

    public AuditAction Action { get; private set; }

    public string EntityName { get; private set; } = null!;

    public Guid? EntityId { get; private set; }

    public string Description { get; private set; } = null!;

    public string? OldValues { get; private set; }

    public string? NewValues { get; private set; }

    public string? ChangedProperties { get; private set; }

    public string? IpAddress { get; private set; }

    public string? UserAgent { get; private set; }

    public string? RequestId { get; private set; }

    public AuditSeverity Severity { get; private set; }

    public DateTime Timestamp { get; private set; }
    public new bool IsActive { get; }
    public bool IsSuccessful { get; private set; }

    public string? ErrorMessage { get; private set; }


    public User? User { get; private set; }

}