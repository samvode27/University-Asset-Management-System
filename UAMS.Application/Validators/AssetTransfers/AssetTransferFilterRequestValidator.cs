using FluentValidation;
using UAMS.Application.DTOs.AssetTransfers.Requests;

namespace UAMS.Application.Validators.AssetTransfers;

public class AssetTransferFilterRequestValidator
    : AbstractValidator<AssetTransferFilterRequestDto>
{
    public AssetTransferFilterRequestValidator()
    {
        // ============================================================
        // Transfer Number
        // ============================================================

        RuleFor(x => x.TransferNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.TransferNumber))
            .WithMessage(
                "Transfer number must not exceed 100 characters.");


        // ============================================================
        // Status
        // ============================================================

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage(
                "Invalid asset transfer status.");


        // ============================================================
        // Requested Date Range
        // ============================================================

        RuleFor(x => x.RequestedDateTo)
            .GreaterThanOrEqualTo(x => x.RequestedDateFrom)
            .When(x =>
                x.RequestedDateFrom.HasValue &&
                x.RequestedDateTo.HasValue)
            .WithMessage(
                "Requested date to cannot be earlier than requested date from.");


        // ============================================================
        // Completed Date Range
        // ============================================================

        RuleFor(x => x.CompletedDateTo)
            .GreaterThanOrEqualTo(x => x.CompletedDateFrom)
            .When(x =>
                x.CompletedDateFrom.HasValue &&
                x.CompletedDateTo.HasValue)
            .WithMessage(
                "Completed date to cannot be earlier than completed date from.");


        // ============================================================
        // Page Number
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage(
                "Page number must be greater than or equal to 1.");


        // ============================================================
        // Page Size
        // ============================================================

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(
                "Page size must be between 1 and 100.");
    }
}