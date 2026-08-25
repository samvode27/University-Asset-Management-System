using UAMS.Domain.Common;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Permissions;

public class Permission : AuditableEntity
{
    private Permission()
    {
    }

    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public string? Description { get; private set; }

    public string Module { get; private set; } = null!;

    public ICollection<RolePermission> RolePermissions { get; private set; }
        = new List<RolePermission>();


    // ================================================================
    // Factory
    // ================================================================

    public static Permission Create(
        string name,
        string code,
        string? description,
        string module,
        Guid? createdBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            name,
            nameof(name));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            code,
            nameof(code));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            module,
            nameof(module));

        return new Permission
        {
            Name = name.Trim(),

            Code = code.Trim(),

            Description =
                string.IsNullOrWhiteSpace(description)
                    ? null
                    : description.Trim(),

            Module = module.Trim(),

            CreatedAt = DateTime.UtcNow,

            CreatedBy = createdBy,

            IsActive = true,

            IsDeleted = false
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        string name,
        string code,
        string? description,
        string module,
        Guid? updatedBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            name,
            nameof(name));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            code,
            nameof(code));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            module,
            nameof(module));

        if (IsDeleted)
        {
            throw new InvalidOperationException(
                "A deleted permission cannot be updated.");
        }

        Name = name.Trim();

        Code = code.Trim();

        Description =
            string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();

        Module = module.Trim();

        UpdatedAt = DateTime.UtcNow;

        UpdatedBy = updatedBy;
    }


    // ================================================================
    // Activation
    // ================================================================

    public void Activate(Guid? updatedBy = null)
    {
        if (IsDeleted)
        {
            throw new InvalidOperationException(
                "A deleted permission cannot be activated.");
        }

        IsActive = true;

        UpdatedAt = DateTime.UtcNow;

        UpdatedBy = updatedBy;
    }


    // ================================================================
    // Deactivation
    // ================================================================

    public void Deactivate(Guid? updatedBy = null)
    {
        if (IsDeleted)
        {
            throw new InvalidOperationException(
                "A deleted permission cannot be deactivated.");
        }

        IsActive = false;

        UpdatedAt = DateTime.UtcNow;

        UpdatedBy = updatedBy;
    }


    // ================================================================
    // Soft Delete
    // ================================================================

    public void MarkDeleted(Guid deletedBy)
    {
        if (deletedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Deleted by user ID is required.",
                nameof(deletedBy));
        }

        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;

        IsActive = false;

        DeletedAt = DateTime.UtcNow;

        DeletedBy = deletedBy;

        UpdatedAt = DateTime.UtcNow;

        UpdatedBy = deletedBy;
    }
}

