namespace UAMS.Application.DTOs.Reports.Requests;

public class ReportFilterRequestDto
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? AssetCategoryId { get; set; }

    public Guid? UserId { get; set; }
}