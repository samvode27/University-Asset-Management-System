using UAMS.Application.DTOs.Dashboard.Requests;
using UAMS.Application.DTOs.Dashboard.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;

using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.AssetCategories;
using UAMS.Domain.Entities.AssetDisposals;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Entities.AssetReturns;
using UAMS.Domain.Entities.AssetTransfers;
using UAMS.Domain.Entities.AuditLogs;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Entities.Notifications;
using UAMS.Domain.Entities.Users;

using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ============================================================
    // Get Dashboard
    // ============================================================

    public async Task<DashboardResponseDto> GetDashboardAsync(
        DashboardFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // --------------------------------------------------------
        // Load Source Data
        // --------------------------------------------------------

        var assets = await _unitOfWork.Assets
            .GetAllAsync(cancellationToken);

        var assetRequests = await _unitOfWork.AssetRequests
            .GetAllAsync(cancellationToken);

        var transfers = await _unitOfWork.AssetTransfers
            .GetAllAsync(cancellationToken);

        var returns = await _unitOfWork.AssetReturns
            .GetAllAsync(cancellationToken);

        var damageReports = await _unitOfWork.DamageReports
            .GetAllAsync(cancellationToken);

        var maintenances = await _unitOfWork.MaintenanceRequests
            .GetAllAsync(cancellationToken);

        var disposals = await _unitOfWork.AssetDisposals
            .GetAllAsync(cancellationToken);

        var users = await _unitOfWork.Users
            .GetAllAsync(cancellationToken);

        var departments = await _unitOfWork.Departments
            .GetAllAsync(cancellationToken);

        var notifications = await _unitOfWork.Notifications
            .GetAllAsync(cancellationToken);

        var auditLogs = await _unitOfWork.AuditLogs
            .GetAllAsync(cancellationToken);

        var categories = await _unitOfWork.AssetCategories
            .GetAllAsync(cancellationToken);


        // --------------------------------------------------------
        // Apply Department Filter
        // --------------------------------------------------------

        var filteredAssets = assets;

        if (request.DepartmentId.HasValue)
        {
            filteredAssets = assets
                .Where(x =>
                    x.DepartmentId ==
                    request.DepartmentId.Value)
                .ToList();
        }


        // --------------------------------------------------------
        // Apply Date Filters
        // --------------------------------------------------------

        var filteredAuditLogs = auditLogs;

        var filteredNotifications = notifications;

        if (request.FromDate.HasValue)
        {
            var fromDate = request.FromDate.Value;

            filteredAuditLogs = filteredAuditLogs
                .Where(x =>
                    x.Timestamp >= fromDate)
                .ToList();

            filteredNotifications = filteredNotifications
                .Where(x =>
                    x.CreatedAt >= fromDate)
                .ToList();
        }

        if (request.ToDate.HasValue)
        {
            var toDate = request.ToDate.Value;

            filteredAuditLogs = filteredAuditLogs
                .Where(x =>
                    x.Timestamp <= toDate)
                .ToList();

            filteredNotifications = filteredNotifications
                .Where(x =>
                    x.CreatedAt <= toDate)
                .ToList();
        }


        // --------------------------------------------------------
        // Build Response
        // --------------------------------------------------------

        return new DashboardResponseDto
        {
            Summary = BuildSummary(
                filteredAssets,
                assetRequests,
                transfers,
                returns,
                damageReports,
                maintenances,
                disposals,
                users,
                departments,
                filteredNotifications,
                filteredAuditLogs),

            AssetStatus = BuildAssetStatusSummary(
                filteredAssets),

            AssetCategories = BuildAssetCategorySummary(
                filteredAssets,
                categories),

            DepartmentAssets = BuildDepartmentAssetSummary(
                filteredAssets,
                departments),

            RecentActivities = BuildRecentActivities(
                filteredAuditLogs)
        };
    }


    // ============================================================
    // Dashboard Summary
    // ============================================================

    private static DashboardSummaryResponseDto BuildSummary(
        IReadOnlyCollection<Asset> assets,
        IReadOnlyCollection<AssetRequest> assetRequests,
        IReadOnlyCollection<AssetTransfer> transfers,
        IReadOnlyCollection<AssetReturn> returns,
        IReadOnlyCollection<DamageReport> damageReports,
        IReadOnlyCollection<Maintenance> maintenances,
        IReadOnlyCollection<AssetDisposal> disposals,
        IReadOnlyCollection<User> users,
        IReadOnlyCollection<Department> departments,
        IReadOnlyCollection<Notification> notifications,
        IReadOnlyCollection<AuditLog> auditLogs)
    {
        return new DashboardSummaryResponseDto
        {
            // ----------------------------------------------------
            // Assets
            // ----------------------------------------------------

            TotalAssets = assets.Count,

            AvailableAssets = assets.Count(
                x => x.Status == AssetStatus.Available),

            AssignedAssets = assets.Count(
                x => x.Status == AssetStatus.Assigned),

            UnderMaintenanceAssets = assets.Count(
                x => x.Status == AssetStatus.UnderMaintenance),

            DamagedAssets = assets.Count(
                x => x.Status == AssetStatus.Damaged),

            DisposedAssets = assets.Count(
                x => x.Status == AssetStatus.Disposed),


            // ----------------------------------------------------
            // Asset Requests
            // ----------------------------------------------------

            PendingAssetRequests = assetRequests.Count(
                x =>
                    x.Status ==
                    AssetRequestStatus.PendingDepartmentHeadApproval
                    ||
                    x.Status ==
                    AssetRequestStatus.PendingAssetManagerApproval),

            ApprovedAssetRequests = assetRequests.Count(
                x =>
                    x.Status ==
                    AssetRequestStatus.DepartmentHeadApproved
                    ||
                    x.Status ==
                    AssetRequestStatus.AssetManagerApproved),

            RejectedAssetRequests = assetRequests.Count(
                x =>
                    x.Status ==
                    AssetRequestStatus.DepartmentHeadRejected
                    ||
                    x.Status ==
                    AssetRequestStatus.AssetManagerRejected),


            // ----------------------------------------------------
            // Transfers
            // ----------------------------------------------------

            PendingTransfers = transfers.Count(
                x =>
                    x.Status ==
                    AssetTransferStatus.PendingApproval),

            CompletedTransfers = transfers.Count(
                x =>
                    x.Status ==
                    AssetTransferStatus.Completed),


            // ----------------------------------------------------
            // Returns
            // ----------------------------------------------------

            PendingReturns = returns.Count(
                x =>
                    x.Status ==
                    AssetReturnStatus.Requested
                    ||
                    x.Status ==
                    AssetReturnStatus.Approved
                    ||
                    x.Status ==
                    AssetReturnStatus.PendingInspection),

            CompletedReturns = returns.Count(
                x =>
                    x.Status ==
                    AssetReturnStatus.Completed),


            // ----------------------------------------------------
            // Damage Reports
            // ----------------------------------------------------

            PendingDamageReports = damageReports.Count(
                x =>
                    x.Status ==
                    DamageReportStatus.Submitted
                    ||
                    x.Status ==
                    DamageReportStatus.UnderReview),


            // ----------------------------------------------------
            // Maintenance
            // ----------------------------------------------------

            ActiveMaintenanceRequests = maintenances.Count(
                x =>
                    x.Status ==
                    MaintenanceStatus.Pending
                    ||
                    x.Status ==
                    MaintenanceStatus.Approved
                    ||
                    x.Status ==
                    MaintenanceStatus.InProgress),


            // ----------------------------------------------------
            // Disposal
            // ----------------------------------------------------

            PendingDisposals = disposals.Count(
                x =>
                    x.Status ==
                    AssetDisposalStatus.Requested
                    ||
                    x.Status ==
                    AssetDisposalStatus.UnderReview
                    ||
                    x.Status ==
                    AssetDisposalStatus.Approved),

            CompletedDisposals = disposals.Count(
                x =>
                    x.Status ==
                    AssetDisposalStatus.Completed),


            // ----------------------------------------------------
            // Users
            // ----------------------------------------------------

            TotalUsers = users.Count,

            ActiveUsers = users.Count(
                x =>
                    x.IsActive &&
                    !x.IsDeleted),


            // ----------------------------------------------------
            // Departments
            // ----------------------------------------------------

            TotalDepartments = departments.Count,

            ActiveDepartments = departments.Count(
                x =>
                    x.IsActive &&
                    !x.IsDeleted),


            // ----------------------------------------------------
            // Notifications
            // ----------------------------------------------------

            TotalNotifications = notifications.Count,

            UnreadNotifications = notifications.Count(
                x =>
                    x.Status ==
                    NotificationStatus.Unread),

            ReadNotifications = notifications.Count(
                x =>
                    x.Status ==
                    NotificationStatus.Read),


            // ----------------------------------------------------
            // Audit Logs
            // ----------------------------------------------------

            TotalAuditLogs = auditLogs.Count,

            SuccessfulAuditLogs = auditLogs.Count(
                x => x.IsSuccessful),

            FailedAuditLogs = auditLogs.Count(
                x => !x.IsSuccessful),

            CriticalAuditLogs = auditLogs.Count(
                x =>
                    x.Severity ==
                    AuditSeverity.Critical)
        };
    }


    // ============================================================
    // Asset Status Summary
    // ============================================================

    private static List<AssetStatusSummaryDto>
        BuildAssetStatusSummary(
            IReadOnlyCollection<Asset> assets)
    {
        if (assets.Count == 0)
        {
            return new List<AssetStatusSummaryDto>();
        }

        return assets
            .GroupBy(x => x.Status)
            .Select(group =>
                new AssetStatusSummaryDto
                {
                    Status = group.Key.ToString(),

                    Count = group.Count(),

                    Percentage = Math.Round(
                        group.Count() * 100m /
                        assets.Count,
                        2)
                })
            .OrderByDescending(x => x.Count)
            .ToList();
    }


    // ============================================================
    // Asset Category Summary
    // ============================================================

    private static List<AssetCategorySummaryDto>
        BuildAssetCategorySummary(
            IReadOnlyCollection<Asset> assets,
            IReadOnlyCollection<AssetCategory> categories)
    {
        var categoryNames = categories
            .ToDictionary(
                x => x.Id,
                x => x.Name);

        return assets
            .GroupBy(x => x.AssetCategoryId)
            .Select(group =>
            {
                categoryNames.TryGetValue(
                    group.Key,
                    out var categoryName);

                return new AssetCategorySummaryDto
                {
                    CategoryId = group.Key,

                    CategoryName =
                        categoryName ?? "Unknown",

                    AssetCount = group.Count(),

                    TotalValue = group.Sum(
                        x => x.PurchaseCost)
                };
            })
            .OrderByDescending(x => x.AssetCount)
            .ToList();
    }


    // ============================================================
    // Department Asset Summary
    // ============================================================

    private static List<DepartmentAssetSummaryDto>
        BuildDepartmentAssetSummary(
            IReadOnlyCollection<Asset> assets,
            IReadOnlyCollection<Department> departments)
    {
        var departmentDictionary = departments
            .ToDictionary(
                x => x.Id,
                x => new DepartmentInfo(
                    x.Code,
                    x.Name));

        return assets
            .Where(x =>
                x.DepartmentId.HasValue)
            .GroupBy(x =>
                x.DepartmentId!.Value)
            .Select(group =>
            {
                departmentDictionary.TryGetValue(
                    group.Key,
                    out var department);

                return new DepartmentAssetSummaryDto
                {
                    DepartmentId = group.Key,

                    DepartmentCode =
                        department?.Code ?? "N/A",

                    DepartmentName =
                        department?.Name ?? "Unknown",

                    TotalAssets = group.Count(),

                    AssignedAssets = group.Count(
                        x =>
                            x.Status ==
                            AssetStatus.Assigned),

                    AvailableAssets = group.Count(
                        x =>
                            x.Status ==
                            AssetStatus.Available),

                    TotalAssetValue = group.Sum(
                        x => x.PurchaseCost)
                };
            })
            .OrderByDescending(
                x => x.TotalAssets)
            .ToList();
    }


    // ============================================================
    // Recent Activities
    // ============================================================

    private static List<RecentActivityDto>
        BuildRecentActivities(
            IReadOnlyCollection<AuditLog> auditLogs)
    {
        return auditLogs
            .OrderByDescending(
                x => x.Timestamp)
            .Take(10)
            .Select(x =>
                new RecentActivityDto
                {
                    Id = x.Id,

                    Action =
                        x.Action.ToString(),

                    EntityName =
                        x.EntityName,

                    EntityId =
                        x.EntityId,

                    Description =
                        x.Description,

                    UserName =
                        x.User?.FullName,

                    Timestamp =
                        x.Timestamp,

                    IsSuccessful =
                        x.IsSuccessful
                })
            .ToList();
    }


    // ============================================================
    // Internal Helper
    // ============================================================

    private sealed record DepartmentInfo(
        string Code,
        string Name);
}