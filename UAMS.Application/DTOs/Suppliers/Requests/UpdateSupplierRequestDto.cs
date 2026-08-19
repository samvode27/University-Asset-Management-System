namespace UAMS.Application.DTOs.Suppliers.Requests;

public class UpdateSupplierRequestDto
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? TaxIdentificationNumber { get; set; }

    public bool IsActive { get; set; }
}