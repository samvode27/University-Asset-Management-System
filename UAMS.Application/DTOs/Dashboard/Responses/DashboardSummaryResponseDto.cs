namespace UAMS.Application.DTOs.Dashboard.Responses;

public class DashboardSummaryResponseDto
{
    // ============================================================
    // Assets
    // ============================================================

    public int TotalAssets { get; set; }

    public int AvailableAssets { get; set; }

    public int AssignedAssets { get; set; }

    public int UnderMaintenanceAssets { get; set; }

    public int DamagedAssets { get; set; }

    public int DisposedAssets { get; set; }


    // ============================================================
    // Asset Requests
    // ============================================================

    public int PendingAssetRequests { get; set; }

    public int ApprovedAssetRequests { get; set; }

    public int RejectedAssetRequests { get; set; }


    // ============================================================
    // Transfers
    // ============================================================

    public int PendingTransfers { get; set; }

    public int CompletedTransfers { get; set; }


    // ============================================================
    // Returns
    // ============================================================

    public int PendingReturns { get; set; }

    public int CompletedReturns { get; set; }


    // ============================================================
    // Damage & Maintenance
    // ============================================================

    public int PendingDamageReports { get; set; }

    public int ActiveMaintenanceRequests { get; set; }


    // ============================================================
    // Disposal
    // ============================================================

    public int PendingDisposals { get; set; }

    public int CompletedDisposals { get; set; }


    // ============================================================
    // Users / Departments
    // ============================================================

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public int TotalDepartments { get; set; }

    public int ActiveDepartments { get; set; }


    // ============================================================
    // Notifications
    // ============================================================

    public int TotalNotifications { get; set; }

    public int UnreadNotifications { get; set; }

    public int ReadNotifications { get; set; }


    // ============================================================
    // Audit Activity
    // ============================================================

    public int TotalAuditLogs { get; set; }

    public int SuccessfulAuditLogs { get; set; }

    public int FailedAuditLogs { get; set; }

    public int CriticalAuditLogs { get; set; }

}