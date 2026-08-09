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

    public Department(
        string code,
        string name,
        string? description,
        string? officeLocation = null,
        DateTime? establishedDate = null)
    {
        Code = code;
        Name = name;
        Description = description;
        OfficeLocation = officeLocation;
        EstablishedDate = establishedDate;
        IsActive = true;
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


    public void Update(
        string code,
        string name,
        string? description,
        string? officeLocation,
        DateTime? establishedDate)
    {
        Code = code;
        Name = name;
        Description = description;
        OfficeLocation = officeLocation;
        EstablishedDate = establishedDate;
    }

    public void AssignDepartmentHead(Guid departmentHeadId)
    {
        DepartmentHeadId = departmentHeadId;
    }

    public void RemoveDepartmentHead()
    {
        DepartmentHeadId = null;
        DepartmentHead = null;
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