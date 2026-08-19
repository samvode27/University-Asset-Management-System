using FluentValidation;
using UAMS.Application.DTOs.AuditLogs.Requests;

namespace UAMS.Application.Validators.AuditLogs;

public class AuditLogFilterRequestValidator
    : AbstractValidator<AuditLogFilterRequestDto>
{
    public AuditLogFilterRequestValidator()
    {
        // ============================================================
        // Action
        // ============================================================

        RuleFor(x => x.Action)
            .IsInEnum()
            .When(x => x.Action.HasValue)
            .WithMessage("Invalid audit action.");


        // ============================================================
        // Entity Name
        // ============================================================

        RuleFor(x => x.EntityName)
            .MaximumLength(150)
            .WithMessage("Entity name cannot exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.EntityName));


        // ============================================================
        // Severity
        // ============================================================

        RuleFor(x => x.Severity)
            .IsInEnum()
            .When(x => x.Severity.HasValue)
            .WithMessage("Invalid audit severity.");


        // ============================================================
        // Request ID
        // ============================================================

        RuleFor(x => x.RequestId)
            .MaximumLength(100)
            .WithMessage("Request ID cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestId));


        // ============================================================
        // Date Range
        // ============================================================

        RuleFor(x => x)
            .Must(x =>
                !x.FromDate.HasValue ||
                !x.ToDate.HasValue ||
                x.FromDate.Value <= x.ToDate.Value)
            .WithMessage(
                "From date must be earlier than or equal to To date.");


        // ============================================================
        // Search
        // ============================================================

        RuleFor(x => x.SearchTerm)
            .MaximumLength(500)
            .WithMessage("Search term cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));


        // ============================================================
        // Pagination
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}