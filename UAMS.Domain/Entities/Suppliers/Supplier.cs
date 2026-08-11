using UAMS.Domain.Common;
using UAMS.Domain.Entities.Purchases;

namespace UAMS.Domain.Entities.Suppliers;

public class Supplier : AuditableEntity
{
    private Supplier()
    {
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

}