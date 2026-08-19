using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Infrastructure.Persistence;
using UAMS.Infrastructure.Repositories;

namespace UAMS.Infrastructure.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly UAMSDbContext _context;

    private IPermissionRepository? _permissions;
    private IRoleRepository? _roles;
    private IUserRepository? _users;
    private IDepartmentRepository? _departments;
    private IAssetCategoryRepository? _assetCategories;
    private ISupplierRepository? _suppliers;
    private IPurchaseRepository? _purchases;
    private IAssetRepository? _assets;
    private IQRCodeRepository? _qrCodes;
    private IBarcodeRepository? _barcodes;
    private IAssetRequestRepository? _assetRequests;
    private IAssetAssignmentRepository? _assetAssignments;
    private IAssetTransferRepository? _assetTransfers;
    private IAssetReturnRepository? _assetReturns;
    private IDamageReportRepository? _damageReports;
    private IMaintenanceRequestRepository? _maintenanceRequests;
    private IAssetDisposalRepository? _assetDisposals;
    private INotificationRepository? _notifications;
    private IFileAttachmentRepository? _fileAttachments;
    private IAuditLogRepository? _auditLogs;

    public UnitOfWork(UAMSDbContext context)
    {
        _context = context;
    }

    public IPermissionRepository Permissions =>
        _permissions ??= new PermissionRepository(_context);

    public IRoleRepository Roles =>
        _roles ??= new RoleRepository(_context);

    public IUserRepository Users =>
        _users ??= new UserRepository(_context);

    public IDepartmentRepository Departments =>
        _departments ??= new DepartmentRepository(_context);

    public IAssetCategoryRepository AssetCategories =>
        _assetCategories ??= new AssetCategoryRepository(_context);

    public ISupplierRepository Suppliers =>
        _suppliers ??= new SupplierRepository(_context);

    public IPurchaseRepository Purchases =>
        _purchases ??= new PurchaseRepository(_context);

    public IAssetRepository Assets =>
        _assets ??= new AssetRepository(_context);

    public IQRCodeRepository QRCodes =>
        _qrCodes ??= new QRCodeRepository(_context);

    public IBarcodeRepository Barcodes =>
        _barcodes ??= new BarcodeRepository(_context);

    public IAssetRequestRepository AssetRequests =>
        _assetRequests ??= new AssetRequestRepository(_context);

    public IAssetAssignmentRepository AssetAssignments =>
        _assetAssignments ??= new AssetAssignmentRepository(_context);

    public IAssetTransferRepository AssetTransfers =>
        _assetTransfers ??= new AssetTransferRepository(_context);

    public IAssetReturnRepository AssetReturns =>
        _assetReturns ??= new AssetReturnRepository(_context);

    public IDamageReportRepository DamageReports =>
        _damageReports ??= new DamageReportRepository(_context);

    public IMaintenanceRequestRepository MaintenanceRequests =>
        _maintenanceRequests ??= new MaintenanceRequestRepository(_context);

    public IAssetDisposalRepository AssetDisposals =>
        _assetDisposals ??= new AssetDisposalRepository(_context);

    public INotificationRepository Notifications =>
        _notifications ??= new NotificationRepository(_context);

    public IFileAttachmentRepository FileAttachments =>
        _fileAttachments ??= new FileAttachmentRepository(_context);

    public IAuditLogRepository AuditLogs =>
        _auditLogs ??= new AuditLogRepository(_context);

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}