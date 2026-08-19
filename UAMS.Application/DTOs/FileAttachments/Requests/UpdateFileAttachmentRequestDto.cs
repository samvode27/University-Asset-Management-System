using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.FileAttachments.Requests;

public class UpdateFileAttachmentRequestDto
{
    // ============================================================
    // Description
    // ============================================================

    [MaxLength(1000)]
    public string? Description { get; set; }


    // ============================================================
    // File Type
    // ============================================================

    [Required]
    public FileAttachmentType FileType { get; set; }
}