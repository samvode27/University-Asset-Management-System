using System.Text;

using UAMS.Application.Options;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using UAMS.Application;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Application.Services;

using UAMS.Infrastructure.Persistence;
using UAMS.Infrastructure.Repositories;
using UAMS.Infrastructure.Services;
using UAMS.Infrastructure.UnitOfWork;
using UAMS.Application.Services.AssetReturns;
using UAMS.Application.Services.AssetDisposals;
using Npgsql;
using UAMS.Application.Services.Notifications;
using UAMS.Application.Services.FileAttachments;
using UAMS.Application.Services.AuditLogs;


var builder = WebApplication.CreateBuilder(args);


// ================================================================
// JWT Configuration
// ================================================================

var jwtSection = builder.Configuration.GetSection("Jwt");

var jwtKey =
    jwtSection["Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is not configured.");

var jwtIssuer =
    jwtSection["Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer is not configured.");

var jwtAudience =
    jwtSection["Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience is not configured.");


if (Encoding.UTF8.GetBytes(jwtKey).Length < 32)
{
    throw new InvalidOperationException(
        "JWT Key must be at least 32 bytes long.");
}


// ================================================================
// Authentication Options
// ================================================================

builder.Services
    .AddOptions<AuthenticationOptions>()
    .Bind(
        builder.Configuration.GetSection(
            AuthenticationOptions.SectionName))
    .Validate(
        options => options.MaxFailedLoginAttempts > 0,
        "Maximum failed login attempts must be greater than zero.")
    .ValidateOnStart();


// ================================================================
// Authentication
// ================================================================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ValidateIssuer = true,

                ValidIssuer = jwtIssuer,

                ValidateAudience = true,

                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });


// ================================================================
// Authorization
// ================================================================

builder.Services.AddAuthorization();


// ================================================================
// Database
// ================================================================

builder.Services.AddDbContext<UAMSDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "UAMSConnection"));
});


// ================================================================
// Controllers
// ================================================================

builder.Services.AddControllers();


// ================================================================
// API Documentation
// ================================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// ================================================================
// FluentValidation
// ================================================================

builder.Services.AddValidatorsFromAssembly(
    typeof(AssemblyReference).Assembly);


// ================================================================
// HTTP Context
// ================================================================

builder.Services.AddHttpContextAccessor();


// ================================================================
// Repositories
// ================================================================

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();


// ================================================================
// Unit Of Work
// ================================================================

builder.Services.AddScoped<
    IUnitOfWork,
    UnitOfWork>();


// ================================================================
// Authentication Services
// ================================================================

builder.Services.AddScoped<
    IAuthenticationService,
    AuthenticationService>();

builder.Services.AddScoped<
    IPasswordService,
    PasswordService>();

builder.Services.AddScoped<
    ITokenService,
    TokenService>();

builder.Services.AddScoped<
    ICurrentUserService,
    CurrentUserService>();

// ================================================================
// User Services
// ================================================================

builder.Services.AddScoped<
    IUserService,
    UserService>();

// ================================================================
// Role Services
// ================================================================

builder.Services.AddScoped<
    IRoleService,
    RoleService>();

// ================================================================
// Department Services
// ================================================================

builder.Services.AddScoped<
    IDepartmentService,
    DepartmentService>();

// ================================================================
// Asset Category Services
// ================================================================

builder.Services.AddScoped<
    IAssetCategoryService,
    AssetCategoryService>();


// ================================================================
// Supplier Services
// ================================================================

builder.Services.AddScoped<
    ISupplierService,
    SupplierService>();

// ================================================================
// Purchase Services
// ================================================================

builder.Services.AddScoped<
    IPurchaseService,
    PurchaseService>();

// ================================================================
// Asset Services
// ================================================================

builder.Services.AddScoped<
    IAssetService,
    AssetService>();

// ================================================================
// QrCode Services
// ================================================================

builder.Services.AddScoped<
    IQRCodeService, 
    QRCodeService>();

// ================================================================
// Barcode Services
// ================================================================

builder.Services.AddScoped<
    IBarcodeService, 
    BarcodeService>();

// ================================================================
// AssetAssignment Services
// ================================================================

builder.Services.AddScoped<
    IAssetAssignmentService, 
    AssetAssignmentService>();


// ================================================================
// Asset Transfer Services
// ================================================================

builder.Services.AddScoped<
    IAssetTransferService, 
    AssetTransferService>();


// ================================================================
// Asset Return Services
// ================================================================

builder.Services.AddScoped<
    IAssetReturnService, 
    AssetReturnService>();


// ================================================================
// Asset Disposal Services
// ================================================================

builder.Services.AddScoped<
    IAssetDisposalService, 
    AssetDisposalService>();

// ================================================================
// Maintainance Services
// ================================================================

builder.Services.AddScoped<
    IMaintenanceService, 
    MaintenanceService>();

// ================================================================
// Notification Services
// ================================================================

builder.Services.AddScoped<
    INotificationService, 
    NotificationService>();

// ================================================================
// File Attachment Services
// ================================================================

builder.Services.AddScoped<
    IFileAttachmentService, 
    FileAttachmentService>();

// ================================================================
// AuditLog Services
// ================================================================

builder.Services.AddScoped<
    IAuditLogService, 
    AuditLogService>();


// ================================================================
// Dashboard Services
// ================================================================
builder.Services.AddScoped<
    IDashboardService, 
    DashboardService>();

// ================================================================
// Profile Services
// ================================================================
builder.Services.AddScoped<
    IProfileService, 
    ProfileService>();

// ================================================================
// Damage Report Services
// ================================================================

builder.Services.AddScoped<
    IDamageReportService, 
    DamageReportService>();

// ================================================================
// Permission Services
// ================================================================

builder.Services.AddScoped<
    IPermissionService, 
    PermissionService>();

// ================================================================
// Asset Request Services
// ================================================================

builder.Services.AddScoped<
    IAssetRequestService, 
    AssetRequestService>();


// ================================================================
// Build Application
// ================================================================

var app = builder.Build();


// ================================================================
// Swagger
// ================================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// ================================================================
// HTTPS
// ================================================================

app.UseHttpsRedirection();


// ================================================================
// Authentication / Authorization
// ================================================================

app.UseAuthentication();

app.UseAuthorization();


// ================================================================
// Controllers
// ================================================================

app.MapControllers();


// ================================================================
// Run
// ================================================================

await app.RunAsync();