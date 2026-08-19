using FluentValidation;
using UAMS.Application.DTOs.SystemSettings.Requests;

namespace UAMS.Application.Validators.SystemSettings;

public class CreateSystemSettingRequestValidator
    : AbstractValidator<CreateSystemSettingRequestDto>
{
    public CreateSystemSettingRequestValidator()
    {
        // ============================================================
        // Key
        // ============================================================

        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Setting key is required.")
            .MaximumLength(150)
            .WithMessage("Setting key must not exceed 150 characters.");


        // ============================================================
        // Value
        // ============================================================

        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Setting value is required.")
            .MaximumLength(2000)
            .WithMessage("Setting value must not exceed 2000 characters.");


        // ============================================================
        // Description
        // ============================================================

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));


        // ============================================================
        // Category
        // ============================================================

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required.")
            .MaximumLength(100)
            .WithMessage("Category must not exceed 100 characters.");


        // ============================================================
        // Data Type
        // ============================================================

        RuleFor(x => x.DataType)
            .NotEmpty()
            .WithMessage("Data type is required.")
            .MaximumLength(50)
            .WithMessage("Data type must not exceed 50 characters.")
            .Must(BeSupportedDataType)
            .WithMessage(
                "Data type must be one of: String, Integer, Decimal, Boolean, DateTime, Json."
            );
    }


    // ================================================================
    // Supported Data Types
    // ================================================================

    private static bool BeSupportedDataType(string dataType)
    {
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