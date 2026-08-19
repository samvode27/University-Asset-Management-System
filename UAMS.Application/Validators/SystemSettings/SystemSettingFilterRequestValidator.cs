using FluentValidation;
using UAMS.Application.DTOs.SystemSettings.Requests;

namespace UAMS.Application.Validators.SystemSettings;

public class SystemSettingFilterRequestValidator
    : AbstractValidator<SystemSettingFilterRequestDto>
{
    public SystemSettingFilterRequestValidator()
    {
        // ============================================================
        // Search
        // ============================================================

        RuleFor(x => x.SearchTerm)
            .MaximumLength(150)
            .WithMessage("Search term must not exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));


        // ============================================================
        // Category
        // ============================================================

        RuleFor(x => x.Category)
            .MaximumLength(100)
            .WithMessage("Category must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Category));


        // ============================================================
        // Data Type
        // ============================================================

        RuleFor(x => x.DataType)
            .MaximumLength(50)
            .WithMessage("Data type must not exceed 50 characters.")
            .Must(BeSupportedDataType)
            .WithMessage(
                "Data type must be one of: String, Integer, Decimal, Boolean, DateTime, Json."
            )
            .When(x => !string.IsNullOrWhiteSpace(x.DataType));


        // ============================================================
        // Pagination
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");


        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }


    // ================================================================
    // Supported Data Types
    // ================================================================

    private static bool BeSupportedDataType(string? dataType)
    {
        if (string.IsNullOrWhiteSpace(dataType))
            return true;

        return dataType.Trim().ToLowerInvariant() switch
        {
            "string" => true,
            "integer" => true,
            "decimal" => true,
            "boolean" => true,
            "datetime" => true,
            "json" => true,
            _ => false
        };
    }
}