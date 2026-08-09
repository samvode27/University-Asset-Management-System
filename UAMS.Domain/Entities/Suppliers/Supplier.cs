using UAMS.Domain.Common;
using UAMS.Domain.Entities.Purchases;

namespace UAMS.Domain.Entities.Suppliers;

public class Supplier : AuditableEntity
{
    private Supplier()
    {
    }

    public Supplier(
        string code,
        string name,
        string? contactPerson,
        string? email,
        string? phoneNumber,
        string? address,
        string? taxIdentificationNumber)
    {
        Code = code;
        Name = name;
        ContactPerson = contactPerson;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        TaxIdentificationNumber = taxIdentificationNumber;

        IsActive = true;
    }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? ContactPerson { get; private set; }

    public string? Email { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string? Address { get; private set; }

    public string? TaxIdentificationNumber { get; private set; }


    public ICollection<Purchase> Purchases { get; private set; }
        = new List<Purchase>();

    public void Update(
        string code,
        string name,
        string? contactPerson,
        string? email,
        string? phoneNumber,
        string? address,
        string? taxIdentificationNumber)
    {
        Code = code;
        Name = name;
        ContactPerson = contactPerson;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        TaxIdentificationNumber = taxIdentificationNumber;
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