using FluentValidation;
using UAMS.Application.DTOs.Dashboard.Requests;

namespace UAMS.Application.Validators.Dashboard;

public class DashboardFilterRequestValidator
    : AbstractValidator<DashboardFilterRequestDto>
{
    public DashboardFilterRequestValidator()
    {
        // ============================================================
        // Department
        // ============================================================

        RuleFor(x => x.DepartmentId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("DepartmentId must be a valid identifier when provided.");


        // ============================================================
        // Date Range
        // ============================================================

        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("ToDate must be greater than or equal to FromDate.");
    }
}