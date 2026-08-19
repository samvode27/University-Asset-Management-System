using FluentValidation;
using UAMS.Application.DTOs.FileAttachments.Requests;

namespace UAMS.Application.Validators.FileAttachments;

public class FileAttachmentFilterRequestValidator
    : AbstractValidator<FileAttachmentFilterRequestDto>
{
    public FileAttachmentFilterRequestValidator()
    {
        // ============================================================
        // Search
        // ============================================================

        RuleFor(x => x.SearchTerm)
            .MaximumLength(500)
            .WithMessage("Search term cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));


        // ============================================================
        // Entity Name
        // ============================================================

        RuleFor(x => x.EntityName)
            .MaximumLength(100)
            .WithMessage("Entity name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.EntityName));


        // ============================================================
        // File Type
        // ============================================================

        RuleFor(x => x.FileType)
            .IsInEnum()
            .When(x => x.FileType.HasValue)
            .WithMessage("Invalid file attachment type.");


        // ============================================================
        // Status
        // ============================================================

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid file attachment status.");


        // ============================================================
        // Date Range
        // ============================================================

        RuleFor(x => x)
            .Must(x =>
                !x.FromDate.HasValue ||
                !x.ToDate.HasValue ||
                x.FromDate.Value <= x.ToDate.Value)
            .WithMessage("From date must be earlier than or equal to To date.");


        // ============================================================
        // File Size Range
        // ============================================================

        RuleFor(x => x.MinimumFileSize)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinimumFileSize.HasValue)
            .WithMessage("Minimum file size cannot be negative.");

        RuleFor(x => x.MaximumFileSize)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaximumFileSize.HasValue)
            .WithMessage("Maximum file size cannot be negative.");

        RuleFor(x => x)
            .Must(x =>
                !x.MinimumFileSize.HasValue ||
                !x.MaximumFileSize.HasValue ||
                x.MinimumFileSize.Value <= x.MaximumFileSize.Value)
            .WithMessage(
                "Minimum file size must be less than or equal to maximum file size.");


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