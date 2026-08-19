namespace UAMS.Application.DTOs.Notifications.Responses;

public class NotificationListResponseDto
{
    public IReadOnlyCollection<NotificationResponseDto> Items { get; set; }
        = Array.Empty<NotificationResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}