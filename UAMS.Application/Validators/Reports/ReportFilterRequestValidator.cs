using FluentValidation;
using UAMS.Application.DTOs.Reports.Requests;

namespace UAMS.Application.Validators.Reports;

public class ReportFilterRequestValidator
    : AbstractValidator<ReportFilterRequestDto>
{
    public ReportFilterRequestValidator()
    {
        // ============================================================
        // Date Range
        // ============================================================

        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("ToDate must be greater than or equal to FromDate.");


        // ============================================================
        // Department
        // ============================================================

        RuleFor(x => x.DepartmentId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("DepartmentId must be a valid identifier.");


        // ============================================================
        // Asset Category
        // ============================================================

        RuleFor(x => x.AssetCategoryId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("AssetCategoryId must be a valid identifier.");


        // ============================================================
        // User
        // ============================================================

        RuleFor(x => x.UserId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("UserId must be a valid identifier.");
    }
}