using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;

namespace UAMS.Domain.Entities.AssetCategories;

public class AssetCategory : AuditableEntity
{
    private AssetCategory()
    {
    }

    public AssetCategory(
        string code,
        string name,
        string? description)
    {
        Code = code;
        Name = name;
        Description = description;
        IsActive = true;
    }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public ICollection<Asset> Assets { get; private set; }
        = new List<Asset>();

    public void Update(
        string code,
        string name,
        string? description)
    {
        Code = code;
        Name = name;
        Description = description;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}