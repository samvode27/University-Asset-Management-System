namespace UAMS.Application.DTOs.Reports.Responses;

public class AssetAssignmentReportResponseDto
{
    public Guid AssignmentId { get; set; }

    public string AssignmentNumber { get; set; } = null!;

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public DateTime AssignedDate { get; set; }

    public DateTime? ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    public string ConditionAtAssignment { get; set; } = null!;

    public string Status { get; set; } = null!;
}