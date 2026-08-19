using FluentValidation;
using UAMS.Application.DTOs.FileAttachments.Requests;

namespace UAMS.Application.Validators.FileAttachments;

public class UpdateFileAttachmentRequestValidator
    : AbstractValidator<UpdateFileAttachmentRequestDto>
{
    public UpdateFileAttachmentRequestValidator()
    {
        // ============================================================
        // Description
        // ============================================================

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));


        // ============================================================
        // File Type
        // ============================================================

        RuleFor(x => x.FileType)
            .IsInEnum()
            .WithMessage("Invalid file attachment type.");
    }
}