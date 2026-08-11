using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.FileAttachments;

public class FileAttachment : AuditableEntity
{
    private FileAttachment()
    {
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