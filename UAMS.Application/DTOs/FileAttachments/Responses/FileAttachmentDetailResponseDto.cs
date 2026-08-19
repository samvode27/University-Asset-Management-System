using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.FileAttachments.Responses;

public class FileAttachmentDetailResponseDto
{
    // ============================================================
    // Identity
    // ============================================================

    public Guid Id { get; set; }


    // ============================================================
    // Related Entity
    // ============================================================

    public string EntityName { get; set; } = null!;

    public Guid EntityId { get; set; }


    // ============================================================
    // File Information
    // ============================================================

    public string FileName { get; set; } = null!;

    public string StoredFileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string FileExtension { get; set; } = null!;

    public long FileSize { get; set; }


    // ============================================================
    // Classification
    // ============================================================

    public FileAttachmentType FileType { get; set; }

    public FileAttachmentStatus Status { get; set; }


    // ============================================================
    // Description
    // ============================================================

    public string? Description { get; set; }


    // ============================================================
    // Uploaded By
    // ============================================================

    public Guid UploadedById { get; set; }

    public string? UploadedByName { get; set; }


    // ============================================================
    // Upload Information
    // ============================================================

    public DateTime UploadedAt { get; set; }


    // ============================================================
    // Integrity
    // ============================================================

    public string? Checksum { get; set; }


    // ============================================================
    // State
    // ============================================================

    public bool IsActive { get; set; }


    // ============================================================
    // Audit Information
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}