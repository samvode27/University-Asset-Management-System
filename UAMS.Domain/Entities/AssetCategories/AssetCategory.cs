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


}