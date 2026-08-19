using FluentValidation;
using UAMS.Application.DTOs.AssetCategories.Requests;

namespace UAMS.Application.Validators.AssetCategories;

public class UpdateAssetCategoryRequestValidator
    : AbstractValidator<UpdateAssetCategoryRequestDto>
{
    public UpdateAssetCategoryRequestValidator()
    {
        // ============================================================
        // Name
        // ============================================================

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Asset category name is required.")
            .MaximumLength(150)
            .WithMessage("Asset category name must not exceed 150 characters.");


        // ============================================================
        // Code
        // ============================================================

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Asset category code is required.")
            .MaximumLength(50)
            .WithMessage("Asset category code must not exceed 50 characters.");


        // ============================================================
        // Description
        // ============================================================

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}