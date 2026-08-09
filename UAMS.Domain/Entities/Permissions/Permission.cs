using UAMS.Domain.Common;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Permissions;

public class Permission : AuditableEntity
{
    private Permission()
    {
    }

    public Permission(
        string name,
        string code,
        string? description,
        string module)
    {
        Name = name;
        Code = code;
        Description = description;
        Module = module;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public string? Description { get; private set; }

    public string Module { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public ICollection<RolePermission> RolePermissions { get; private set; }
        = new List<RolePermission>();


    public void Update(
        string name,
        string code,
        string? description,
        string module)
    {
        Name = name;
        Code = code;
        Description = description;
        Module = module;
    }


    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void MarkDeleted(
        Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}