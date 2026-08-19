using FluentValidation;
using UAMS.Application.DTOs.Reservations.Requests;

namespace UAMS.Application.Validators.Reservations;

public class ReservationFilterRequestValidator
    : AbstractValidator<ReservationFilterRequestDto>
{
    public ReservationFilterRequestValidator()
    {
        // ============================================================
        // Asset
        // ============================================================

        RuleFor(x => x.AssetId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("AssetId must be a valid identifier when provided.");


        // ============================================================
        // Employee
        // ============================================================

        RuleFor(x => x.EmployeeId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("EmployeeId must be a valid identifier when provided.");


        // ============================================================
        // Department
        // ============================================================

        RuleFor(x => x.DepartmentId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("DepartmentId must be a valid identifier when provided.");


        // ============================================================
        // Status
        // ============================================================

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Status must be a valid reservation status.");


        // ============================================================
        // Start Date Range
        // ============================================================

        RuleFor(x => x.StartDateTo)
            .GreaterThanOrEqualTo(x => x.StartDateFrom)
            .When(x => x.StartDateFrom.HasValue && x.StartDateTo.HasValue)
            .WithMessage(
                "StartDateTo must be greater than or equal to StartDateFrom.");


        // ============================================================
        // End Date Range
        // ============================================================

        RuleFor(x => x.EndDateTo)
            .GreaterThanOrEqualTo(x => x.EndDateFrom)
            .When(x => x.EndDateFrom.HasValue && x.EndDateTo.HasValue)
            .WithMessage(
                "EndDateTo must be greater than or equal to EndDateFrom.");


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