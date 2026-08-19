using FluentValidation;
using UAMS.Application.DTOs.FileAttachments.Requests;

namespace UAMS.Application.Validators.FileAttachments;

public class UploadFileAttachmentRequestValidator
    : AbstractValidator<UploadFileAttachmentRequestDto>
{
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    private static readonly string[] AllowedExtensions =
    {
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".ppt",
        ".pptx",
        ".jpg",
        ".jpeg",
        ".png"
    };

    public UploadFileAttachmentRequestValidator()
    {
        // ============================================================
        // Entity Name
        // ============================================================

        RuleFor(x => x.EntityName)
            .NotEmpty()
            .WithMessage("Entity name is required.")
            .MaximumLength(100)
            .WithMessage("Entity name cannot exceed 100 characters.");


        // ============================================================
        // Entity ID
        // ============================================================

        RuleFor(x => x.EntityId)
            .NotEmpty()
            .WithMessage("Entity ID is required.");


        // ============================================================
        // File
        // ============================================================

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .DependentRules(() =>
            {
                RuleFor(x => x.File.Length)
                    .GreaterThan(0)
                    .WithMessage("Uploaded file cannot be empty.");

                RuleFor(x => x.File.Length)
                    .LessThanOrEqualTo(MaxFileSize)
                    .WithMessage("File size cannot exceed 10 MB.");

                RuleFor(x => x.File.FileName)
                    .NotEmpty()
                    .WithMessage("File name is required.")
                    .MaximumLength(255)
                    .WithMessage("File name cannot exceed 255 characters.");

                RuleFor(x => x.File.FileName)
                    .Must(HasAllowedExtension)
                    .WithMessage(
                        "Unsupported file type. Allowed types are PDF, Word, Excel, PowerPoint, JPG, JPEG, and PNG.");
            });


        // ============================================================
        // File Type
        // ============================================================

        RuleFor(x => x.FileType)
            .IsInEnum()
            .WithMessage("Invalid file attachment type.");


        // ============================================================
        // Description
        // ============================================================

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName);

        return !string.IsNullOrWhiteSpace(extension)
            && AllowedExtensions.Contains(
                extension,
                StringComparer.OrdinalIgnoreCase);
    }
}