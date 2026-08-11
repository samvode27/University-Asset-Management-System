using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Entities.Users;

namespace UAMS.Domain.Entities.Departments;

public class Department : AuditableEntity
{
    private Department()
    {
    }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public string? OfficeLocation { get; private set; }

    public DateTime? EstablishedDate { get; private set; }

    // Department Head
    public Guid? DepartmentHeadId { get; private set; }

    public User? DepartmentHead { get; private set; }

    // Users belonging to this department
    public ICollection<User> Users { get; private set; }
        = new List<User>();

    // Assets belonging to this department
    public ICollection<Asset> Assets { get; private set; }
        = new List<Asset>();

    // Asset requests made by this department
    public ICollection<AssetRequest> AssetRequests { get; private set; }
        = new List<AssetRequest>();

}