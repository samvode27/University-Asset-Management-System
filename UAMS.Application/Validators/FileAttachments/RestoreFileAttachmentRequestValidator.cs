using FluentValidation;
using UAMS.Application.DTOs.FileAttachments.Requests;

namespace UAMS.Application.Validators.FileAttachments;

public class RestoreFileAttachmentRequestValidator
    : AbstractValidator<RestoreFileAttachmentRequestDto>
{
    public RestoreFileAttachmentRequestValidator()
    {
        // ============================================================
        // File Attachment ID
        // ============================================================

        RuleFor(x => x.FileAttachmentId)
            .NotEmpty()
            .WithMessage("File attachment ID is required.");
    }
}