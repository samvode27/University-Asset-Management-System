using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;

namespace UAMS.Domain.Entities.AssetCategories;

public class AssetCategory : AuditableEntity
{
    private AssetCategory()
    {
    }


    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }


    public ICollection<Asset> Assets { get; private set; }
        = new List<Asset>();


    // ================================================================
    // Factory
    // ================================================================

    public static AssetCategory Create(
        string code,
        string name,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AssetCategory
        {
            Code = code.Trim(),
            Name = name.Trim(),
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim(),
            IsActive = true
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        string code,
        string name,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Code = code.Trim();
        Name = name.Trim();

        Description = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();
    }


    // ================================================================
    // Status
    // ================================================================

    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }
}

