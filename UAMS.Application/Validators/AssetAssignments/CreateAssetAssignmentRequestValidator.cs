using FluentValidation;
using UAMS.Application.DTOs.AssetAssignments.Requests;

namespace UAMS.Application.Validators.AssetAssignments;

public class CreateAssetAssignmentRequestValidator
    : AbstractValidator<CreateAssetAssignmentRequestDto>
{
    public CreateAssetAssignmentRequestValidator()
    {
        // ============================================================
        // Asset
        // ============================================================

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset is required.");


        // ============================================================
        // Asset Request
        // ============================================================

        RuleFor(x => x.AssetRequestId)
            .NotEmpty()
            .WithMessage("Asset request is required.");


        // ============================================================
        // Employee
        // ============================================================

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");


        // ============================================================
        // Expected Return Date
        // ============================================================

        RuleFor(x => x.ExpectedReturnDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.ExpectedReturnDate.HasValue)
            .WithMessage(
                "Expected return date cannot be in the past.");


        // ============================================================
        // Assignment Location
        // ============================================================

        RuleFor(x => x.AssignmentLocation)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.AssignmentLocation))
            .WithMessage(
                "Assignment location must not exceed 500 characters.");


        // ============================================================
        // Remarks
        // ============================================================

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage(
                "Remarks must not exceed 1000 characters.");
    }
}