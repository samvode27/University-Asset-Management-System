using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.FileAttachments;

public class FileAttachment : AuditableEntity
{
    private FileAttachment()
    {
    }


    // ================================================================
    // Properties
    // ================================================================

    public Guid UploadedById { get; private set; }

    public string EntityName { get; private set; } = null!;

    public Guid EntityId { get; private set; }

    public string FileName { get; private set; } = null!;

    public string StoredFileName { get; private set; } = null!;

    public string FilePath { get; private set; } = null!;

    public string ContentType { get; private set; } = null!;

    public string FileExtension { get; private set; } = null!;

    public long FileSize { get; private set; }

    public FileAttachmentType FileType { get; private set; }

    public string? Description { get; private set; }

    public FileAttachmentStatus Status { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public string? Checksum { get; private set; }


    // ================================================================
    // Navigation Properties
    // ================================================================

    public User UploadedBy { get; private set; } = null!;


    // ================================================================
    // Factory
    // ================================================================

    public static FileAttachment Create(
        Guid uploadedById,
        string entityName,
        Guid entityId,
        string fileName,
        string storedFileName,
        string filePath,
        string contentType,
        string fileExtension,
        long fileSize,
        FileAttachmentType fileType,
        string? description = null,
        string? checksum = null)
    {
        if (uploadedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Uploaded by user ID is required.",
                nameof(uploadedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            entityName,
            nameof(entityName));

        if (entityId == Guid.Empty)
        {
            throw new ArgumentException(
                "Entity ID is required.",
                nameof(entityId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            fileName,
            nameof(fileName));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            storedFileName,
            nameof(storedFileName));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            filePath,
            nameof(filePath));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            contentType,
            nameof(contentType));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            fileExtension,
            nameof(fileExtension));

        if (fileSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(fileSize),
                "File size must be greater than zero.");
        }

        return new FileAttachment
        {
            Id = Guid.NewGuid(),
            UploadedById = uploadedById,
            EntityName = entityName.Trim(),
            EntityId = entityId,
            FileName = fileName.Trim(),
            StoredFileName = storedFileName.Trim(),
            FilePath = filePath.Trim(),
            ContentType = contentType.Trim(),
            FileExtension = fileExtension.Trim(),
            FileSize = fileSize,
            FileType = fileType,
            Description = Normalize(description),
            Status = FileAttachmentStatus.Active,
            UploadedAt = DateTime.UtcNow,
            Checksum = Normalize(checksum),
            IsActive = true,
            IsDeleted = false
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        string? description,
        FileAttachmentType fileType)
    {
        Description = Normalize(description);
        FileType = fileType;
    }


    // ================================================================
    // Update Storage Information
    // ================================================================

    public void UpdateStorageInformation(
        string storedFileName,
        string filePath,
        string? checksum)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            storedFileName,
            nameof(storedFileName));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            filePath,
            nameof(filePath));

        StoredFileName = storedFileName.Trim();
        FilePath = filePath.Trim();
        Checksum = Normalize(checksum);
    }


    // ================================================================
    // Archive
    // ================================================================

    public void Archive()
    {
        Status = FileAttachmentStatus.Archived;
        IsActive = false;
    }


    // ================================================================
    // Restore
    // ================================================================

    public void Restore()
    {
        Status = FileAttachmentStatus.Active;
        IsActive = true;
        IsDeleted = false;
    }


    // ================================================================
    // Soft Delete
    // ================================================================

    public void MarkDeleted(Guid deletedBy)
    {
        if (deletedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Deleted by user ID is required.",
                nameof(deletedBy));
        }

        Status = FileAttachmentStatus.Deleted;
        IsActive = false;

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }


    // ================================================================
    // Activate
    // ================================================================

    public void Activate()
    {
        Status = FileAttachmentStatus.Active;
        IsActive = true;
    }


    // ================================================================
    // Deactivate
    // ================================================================

    public void Deactivate()
    {
        IsActive = false;
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}