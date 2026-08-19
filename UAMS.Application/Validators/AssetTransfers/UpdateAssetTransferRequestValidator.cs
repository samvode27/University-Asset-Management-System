using FluentValidation;
using UAMS.Application.DTOs.AssetTransfers.Requests;

namespace UAMS.Application.Validators.AssetTransfers;

public class UpdateAssetTransferRequestValidator
    : AbstractValidator<UpdateAssetTransferRequestDto>
{
    public UpdateAssetTransferRequestValidator()
    {
        // ============================================================
        // Destination Employee
        // ============================================================

        RuleFor(x => x.ToEmployeeId)
            .NotEmpty()
            .WithMessage("Destination employee is required.");


        // ============================================================
        // Destination Department
        // ============================================================

        RuleFor(x => x.ToDepartmentId)
            .NotEmpty()
            .WithMessage("Destination department is required.");


        // ============================================================
        // Destination Location
        // ============================================================

        RuleFor(x => x.ToLocation)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.ToLocation))
            .WithMessage(
                "Destination location must not exceed 500 characters.");


        // ============================================================
        // Reason
        // ============================================================

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Transfer reason is required.")
            .MaximumLength(1000)
            .WithMessage(
                "Transfer reason must not exceed 1000 characters.");


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