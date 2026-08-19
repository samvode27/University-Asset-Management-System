namespace UAMS.Application.DTOs.Suppliers.Responses;

public class SupplierResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public string? ContactPerson { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? TaxIdentificationNumber { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}