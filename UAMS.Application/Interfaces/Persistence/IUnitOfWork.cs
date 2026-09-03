using UAMS.Application.Interfaces.Repositories;

namespace UAMS.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    IPermissionRepository Permissions { get; }
    IRoleRepository Roles { get; }

    IUserRoleRepository UserRoles { get; }
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IDepartmentRepository Departments { get; }

    IAssetCategoryRepository AssetCategories { get; }

    ISupplierRepository Suppliers { get; }

    IPurchaseRepository Purchases { get; }

    IAssetRepository Assets { get; }

    IQRCodeRepository QRCodes { get; }

    IBarcodeRepository Barcodes { get; }

    IAssetRequestRepository AssetRequests { get; }

    IAssetAssignmentRepository AssetAssignments { get; }

    IAssetTransferRepository AssetTransfers { get; }

    IAssetReturnRepository AssetReturns { get; }

    IDamageReportRepository DamageReports { get; }

    IMaintenanceRequestRepository MaintenanceRequests { get; }

    IAssetDisposalRepository AssetDisposals { get; }

    INotificationRepository Notifications { get; }

    IFileAttachmentRepository FileAttachments { get; }

    IAuditLogRepository AuditLogs { get; }

    IRolePermissionRepository RolePermissions { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}