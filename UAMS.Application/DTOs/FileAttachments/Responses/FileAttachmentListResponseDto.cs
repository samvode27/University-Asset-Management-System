namespace UAMS.Application.DTOs.FileAttachments.Responses;

public class FileAttachmentListResponseDto
{
    public IReadOnlyCollection<FileAttachmentResponseDto> Items { get; set; }
        = Array.Empty<FileAttachmentResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}