using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Infrastructure.Persistence;
using UAMS.Infrastructure.Repositories;
using UAMS.Infrastructure.UnitOfWork;

namespace UAMS.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================================
        // Database
        // ============================================================

        services.AddDbContext<UAMSDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("UAMSConnection"));
        });

        // ============================================================
        // Repositories
        // ============================================================

        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();

        services.AddScoped<ISupplierRepository, SupplierRepository>();

        services.AddScoped<IPurchaseRepository, PurchaseRepository>();

        services.AddScoped<IAssetRepository, AssetRepository>();

        services.AddScoped<IQRCodeRepository, QRCodeRepository>();

        services.AddScoped<IBarcodeRepository, BarcodeRepository>();

        services.AddScoped<IAssetRequestRepository, AssetRequestRepository>();

        services.AddScoped<IAssetAssignmentRepository, AssetAssignmentRepository>();

        services.AddScoped<IAssetTransferRepository, AssetTransferRepository>();

        services.AddScoped<IAssetReturnRepository, AssetReturnRepository>();

        services.AddScoped<IDamageReportRepository, DamageReportRepository>();

        services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();

        services.AddScoped<IAssetDisposalRepository, AssetDisposalRepository>();

        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped<IFileAttachmentRepository, FileAttachmentRepository>();

        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // ============================================================
        // Unit of Work
        // ============================================================

        services.AddScoped<
    IUnitOfWork,
    UnitOfWork.UnitOfWork>();

        return services;
    }
}