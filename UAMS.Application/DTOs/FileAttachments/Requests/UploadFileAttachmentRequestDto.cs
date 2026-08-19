using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.FileAttachments.Requests;

public class UploadFileAttachmentRequestDto
{
    // ============================================================
    // Related Entity
    // ============================================================

    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = null!;


    [Required]
    public Guid EntityId { get; set; }


    // ============================================================
    // File
    // ============================================================

    [Required]
    public IFormFile File { get; set; } = null!;


    // ============================================================
    // File Classification
    // ============================================================

    [Required]
    public FileAttachmentType FileType { get; set; }


    // ============================================================
    // Description
    // ============================================================

    [MaxLength(1000)]
    public string? Description { get; set; }
}