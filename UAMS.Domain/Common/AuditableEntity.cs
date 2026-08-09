namespace UAMS.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    public Guid? CreatedBy { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    public bool IsDeleted { get; protected set; }

     public bool IsActive { get; protected set; }

    public DateTime? DeletedAt { get; protected set; }

    public Guid? DeletedBy { get; protected set; }
}