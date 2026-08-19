using FluentValidation;
using UAMS.Application.DTOs.AssetRequests.Requests;

namespace UAMS.Application.Validators.AssetRequests;

public class UpdateAssetRequestValidator
    : AbstractValidator<UpdateAssetRequestDto>
{
    public UpdateAssetRequestValidator()
    {
        // ============================================================
        // Asset
        // ============================================================

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset is required.");


        // ============================================================
        // Department
        // ============================================================

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department is required.");


        // ============================================================
        // Purpose
        // ============================================================

        RuleFor(x => x.Purpose)
            .NotEmpty()
            .WithMessage("Purpose is required.")
            .MaximumLength(2000)
            .WithMessage("Purpose must not exceed 2000 characters.");


        // ============================================================
        // Required From Date
        // ============================================================

        RuleFor(x => x.RequiredFromDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.RequiredFromDate.HasValue)
            .WithMessage(
                "Required from date cannot be in the past.");


        // ============================================================
        // Required Date Range
        // ============================================================

        RuleFor(x => x.RequiredToDate)
            .GreaterThanOrEqualTo(x => x.RequiredFromDate)
            .When(x =>
                x.RequiredFromDate.HasValue &&
                x.RequiredToDate.HasValue)
            .WithMessage(
                "Required to date cannot be earlier than required from date.");
    }
}