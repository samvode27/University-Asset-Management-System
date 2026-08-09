using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AuditLogs;

public class AuditLog : AuditableEntity
{
    private AuditLog()
    {
    }

    public AuditLog(
        Guid? userId,
        AuditAction action,
        string entityName,
        Guid? entityId,
        string description,
        AuditSeverity severity,
        string? oldValues,
        string? newValues,
        string? changedProperties,
        string? ipAddress,
        string? userAgent,
        string? requestId,
        bool isSuccessful,
        string? errorMessage)
    {
        UserId = userId;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        Description = description;
        Severity = severity;
        OldValues = oldValues;
        NewValues = newValues;
        ChangedProperties = changedProperties;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        RequestId = requestId;
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;

        Timestamp = DateTime.UtcNow;
        IsActive = true;
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


    public void UpdateDescription(string description)
    {
        Description = description;
    }


    public void UpdateSeverity(AuditSeverity severity)
    {
        Severity = severity;
    }


    public void MarkSuccessful()
    {
        IsSuccessful = true;
        ErrorMessage = null;
    }


    public void MarkFailed(string errorMessage)
    {
        IsSuccessful = false;
        ErrorMessage = errorMessage;
    }


    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}