using FluentValidation;
using UAMS.Application.DTOs.Profile.Requests;

namespace UAMS.Application.Validators.Profile;

public class UpdateProfilePictureRequestValidator
    : AbstractValidator<UpdateProfilePictureRequestDto>
{
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public UpdateProfilePictureRequestValidator()
    {
        // ============================================================
        // File Name
        // ============================================================

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required.")
            .MaximumLength(255)
            .WithMessage("File name cannot exceed 255 characters.")
            .Must(HasAllowedExtension)
            .WithMessage(
                "Only JPG, JPEG, PNG, and WEBP profile pictures are allowed.");


        // ============================================================
        // Content Type
        // ============================================================

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("Content type is required.")
            .Must(HasAllowedContentType)
            .WithMessage(
                "Only JPEG, PNG, and WEBP image content types are allowed.");


        // ============================================================
        // File Size
        // ============================================================

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("Profile picture file cannot be empty.")
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage("Profile picture cannot exceed 5 MB.");


        // ============================================================
        // File Path
        // ============================================================

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("File path is required.")
            .MaximumLength(1000)
            .WithMessage("File path cannot exceed 1000 characters.");
    }

    private static bool HasAllowedExtension(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        var extension = Path.GetExtension(fileName);

        return AllowedExtensions.Contains(
            extension,
            StringComparer.OrdinalIgnoreCase);
    }

    private static bool HasAllowedContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        return AllowedContentTypes.Contains(
            contentType,
            StringComparer.OrdinalIgnoreCase);
    }
}