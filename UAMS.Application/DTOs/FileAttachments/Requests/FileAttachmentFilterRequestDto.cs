using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.FileAttachments.Requests;

public class FileAttachmentFilterRequestDto
{
    // ============================================================
    // Search
    // ============================================================

    public string? SearchTerm { get; set; }


    // ============================================================
    // Related Entity
    // ============================================================

    public string? EntityName { get; set; }

    public Guid? EntityId { get; set; }


    // ============================================================
    // Uploaded By
    // ============================================================

    public Guid? UploadedById { get; set; }


    // ============================================================
    // File Classification
    // ============================================================

    public FileAttachmentType? FileType { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public FileAttachmentStatus? Status { get; set; }


    // ============================================================
    // Date Range
    // ============================================================

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }


    // ============================================================
    // File Size
    // ============================================================

    public long? MinimumFileSize { get; set; }

    public long? MaximumFileSize { get; set; }


    // ============================================================
    // Pagination
    // ============================================================

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}