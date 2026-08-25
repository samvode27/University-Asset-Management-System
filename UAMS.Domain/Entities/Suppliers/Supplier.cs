using UAMS.Domain.Common;
using UAMS.Domain.Entities.Purchases;

namespace UAMS.Domain.Entities.Suppliers;

public class Supplier : AuditableEntity
{
    private Supplier()
    {
    }


    // ================================================================
    // Properties
    // ================================================================

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? ContactPerson { get; private set; }

    public string? Email { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string? Address { get; private set; }

    public string? TaxIdentificationNumber { get; private set; }


    // ================================================================
    // Navigation Properties
    // ================================================================

    public ICollection<Purchase> Purchases { get; private set; }
        = new List<Purchase>();


    // ================================================================
    // Factory
    // ================================================================

    public static Supplier Create(
        string code,
        string name,
        string? contactPerson,
        string? phoneNumber,
        string? email,
        string? address,
        string? taxIdentificationNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Supplier
        {
            Code = code.Trim().ToUpperInvariant(),
            Name = name.Trim(),
            ContactPerson = NormalizeOptional(contactPerson),
            PhoneNumber = NormalizeOptional(phoneNumber),
            Email = NormalizeEmail(email),
            Address = NormalizeOptional(address),
            TaxIdentificationNumber =
                NormalizeOptional(taxIdentificationNumber),

            IsActive = true
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        string code,
        string name,
        string? contactPerson,
        string? phoneNumber,
        string? email,
        string? address,
        string? taxIdentificationNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        ContactPerson = NormalizeOptional(contactPerson);
        PhoneNumber = NormalizeOptional(phoneNumber);
        Email = NormalizeEmail(email);
        Address = NormalizeOptional(address);
        TaxIdentificationNumber =
            NormalizeOptional(taxIdentificationNumber);
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

    // ================================================================
    // Soft Delete
    // ================================================================

    public void SoftDelete(Guid deletedBy)
    {
        if (deletedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Deleted by user ID is required.",
                nameof(deletedBy));
        }

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        IsActive = false;
    }


    // ================================================================
    // Restore
    // ================================================================

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        IsActive = true;
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }


    private static string? NormalizeEmail(string? email)
    {
        return string.IsNullOrWhiteSpace(email)
            ? null
            : email.Trim().ToLowerInvariant();
    }
}

