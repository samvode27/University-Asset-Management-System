using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.FileAttachments;

public class FileAttachment : AuditableEntity
{
    private FileAttachment()
    {
    }

    public FileAttachment(
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
        string? description,
        string? checksum)
    {
        UploadedById = uploadedById;
        EntityName = entityName;
        EntityId = entityId;
        FileName = fileName;
        StoredFileName = storedFileName;
        FilePath = filePath;
        ContentType = contentType;
        FileExtension = fileExtension;
        FileSize = fileSize;
        FileType = fileType;
        Description = description;
        Checksum = checksum;

        Status = FileAttachmentStatus.Active;
        UploadedAt = DateTime.UtcNow;
        IsActive = true;
    }


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


    public User UploadedBy { get; private set; } = null!;


    public void Update(
        string? description,
        FileAttachmentType fileType)
    {
        Description = description;
        FileType = fileType;
    }


    public void UpdateStorageInformation(
        string storedFileName,
        string filePath,
        string? checksum)
    {
        StoredFileName = storedFileName;
        FilePath = filePath;
        Checksum = checksum;
    }


    public void Archive()
    {
        Status = FileAttachmentStatus.Archived;
        IsActive = false;
    }


    public void Restore()
    {
        Status = FileAttachmentStatus.Active;
        IsActive = true;
    }


    public void MarkDeleted(Guid deletedBy)
    {
        Status = FileAttachmentStatus.Deleted;
        IsActive = false;

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }


    public void Activate()
    {
        Status = FileAttachmentStatus.Active;
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }
}