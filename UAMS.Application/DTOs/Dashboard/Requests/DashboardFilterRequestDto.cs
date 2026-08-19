namespace UAMS.Application.DTOs.Dashboard.Requests;

public class DashboardFilterRequestDto
{
    public Guid? DepartmentId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}