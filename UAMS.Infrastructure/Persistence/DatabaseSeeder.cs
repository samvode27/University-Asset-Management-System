using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Permissions;
using UAMS.Domain.Entities.Roles;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        UAMSDbContext context,
        IPasswordService passwordService,
        CancellationToken cancellationToken = default)
    {
        // ============================================================
        // 1. Ensure database exists / migrations are applied
        // ============================================================

        await context.Database.MigrateAsync(
            cancellationToken);


        // ============================================================
        // 2. Seed Department
        // ============================================================

        var department =
            await context.Departments
                .FirstOrDefaultAsync(
                    x => x.Code == "ICT",
                    cancellationToken);

        if (department is null)
        {
            department =
                Department.Create(
                    code: "ICT",
                    name: "Information and Communication Technology",
                    description: "University information and communication technology department.",
                    officeLocation: "Main Campus",
                    establishedDate: DateOnly.FromDateTime(DateTime.UtcNow),
                    departmentHeadId: null);

            await context.Departments.AddAsync(
                department,
                cancellationToken);

            await context.SaveChangesAsync(
                cancellationToken);
        }


        // ============================================================
        // 3. Seed Administrator User
        // ============================================================

        var adminUser =
            await context.Users
                .FirstOrDefaultAsync(
                    x => x.Username == "admin",
                    cancellationToken);

        if (adminUser is null)
        {
            var passwordHash =
                passwordService.HashPassword(
                    "Admin@12345");

            adminUser =
                User.Create(
                    employeeId: "EMP-0001",
                    fullName: "System Administrator",
                    email: "admin@uams.local",
                    phoneNumber: "0900000000",
                    departmentId: department.Id,
                    username: "admin",
                    passwordHash: passwordHash);

            adminUser.VerifyEmail(
                DateTime.UtcNow);

            adminUser.Activate();

            await context.Users.AddAsync(
                adminUser,
                cancellationToken);

            await context.SaveChangesAsync(
                cancellationToken);
        }
        else
        {
            // DEVELOPMENT ONLY:
            // Reset the seeded administrator password.
            var passwordHash =
                passwordService.HashPassword(
                    "Admin@12345");

            adminUser.ChangePassword(
                passwordHash);

            adminUser.VerifyEmail(
                DateTime.UtcNow);

            adminUser.Activate();

            await context.SaveChangesAsync(
                cancellationToken);
        }


        // ============================================================
        // 4. Seed Administrator Role
        // ============================================================

        var adminRole =
            await context.Roles
                .FirstOrDefaultAsync(
                    x => x.Code == "SYS_ADMIN",
                    cancellationToken);

        if (adminRole is null)
        {
            adminRole =
                Role.Create(
                    name: "System Administrator",
                    code: "SYS_ADMIN",
                    description: "Full system administrator role.",
                    isSystemRole: true);

            await context.Roles.AddAsync(
                adminRole,
                cancellationToken);

            await context.SaveChangesAsync(
                cancellationToken);
        }


        // ============================================================
        // 5. Assign Administrator Role to Admin User
        // ============================================================

        var existingUserRole =
            await context.UserRoles
                .FirstOrDefaultAsync(
                    x =>
                        x.UserId == adminUser.Id &&
                        x.RoleId == adminRole.Id,
                    cancellationToken);

        if (existingUserRole is null)
        {
            var userRole =
                UserRole.Create(
                    userId: adminUser.Id,
                    roleId: adminRole.Id,
                    assignedBy: adminUser.Id);

            await context.UserRoles.AddAsync(
                userRole,
                cancellationToken);

            await context.SaveChangesAsync(
                cancellationToken);
        }


        // ============================================================
        // 6. Seed Permissions
        // ============================================================

        var permissionDefinitions = new[]
        {
    // --------------------------------------------------------
    // Users
    // --------------------------------------------------------

    new
    {
        Name = "View Users",
        Code = "USERS_VIEW",
        Description = "Allows viewing users.",
        Module = "Users"
    },

    new
    {
        Name = "Create Users",
        Code = "USERS_CREATE",
        Description = "Allows creating users.",
        Module = "Users"
    },

    new
    {
        Name = "Update Users",
        Code = "USERS_UPDATE",
        Description = "Allows updating users.",
        Module = "Users"
    },

    new
    {
        Name = "Delete Users",
        Code = "USERS_DELETE",
        Description = "Allows deleting users.",
        Module = "Users"
    },


    // --------------------------------------------------------
    // Roles
    // --------------------------------------------------------

    new
    {
        Name = "View Roles",
        Code = "ROLES_VIEW",
        Description = "Allows viewing roles.",
        Module = "Roles"
    },

    new
    {
        Name = "Create Roles",
        Code = "ROLES_CREATE",
        Description = "Allows creating roles.",
        Module = "Roles"
    },

    new
    {
        Name = "Update Roles",
        Code = "ROLES_UPDATE",
        Description = "Allows updating roles.",
        Module = "Roles"
    },

    new
    {
        Name = "Delete Roles",
        Code = "ROLES_DELETE",
        Description = "Allows deleting roles.",
        Module = "Roles"
    },


    // --------------------------------------------------------
    // Permissions
    // --------------------------------------------------------

    new
    {
        Name = "View Permissions",
        Code = "PERMISSIONS_VIEW",
        Description = "Allows viewing permissions.",
        Module = "Permissions"
    },

    new
    {
        Name = "Create Permissions",
        Code = "PERMISSIONS_CREATE",
        Description = "Allows creating permissions.",
        Module = "Permissions"
    },

    new
    {
        Name = "Update Permissions",
        Code = "PERMISSIONS_UPDATE",
        Description = "Allows updating permissions.",
        Module = "Permissions"
    },

    new
    {
        Name = "Delete Permissions",
        Code = "PERMISSIONS_DELETE",
        Description = "Allows deleting permissions.",
        Module = "Permissions"
    },


    // --------------------------------------------------------
    // Departments
    // --------------------------------------------------------

    new
    {
        Name = "View Departments",
        Code = "DEPARTMENTS_VIEW",
        Description = "Allows viewing departments.",
        Module = "Departments"
    },

    new
    {
        Name = "Create Departments",
        Code = "DEPARTMENTS_CREATE",
        Description = "Allows creating departments.",
        Module = "Departments"
    },

    new
    {
        Name = "Update Departments",
        Code = "DEPARTMENTS_UPDATE",
        Description = "Allows updating departments.",
        Module = "Departments"
    },

    new
    {
        Name = "Delete Departments",
        Code = "DEPARTMENTS_DELETE",
        Description = "Allows deleting departments.",
        Module = "Departments"
    },


    // --------------------------------------------------------
    // Asset Categories
    // --------------------------------------------------------

    new
    {
        Name = "View Asset Categories",
        Code = "ASSET_CATEGORIES_VIEW",
        Description = "Allows viewing asset categories.",
        Module = "Asset Categories"
    },

    new
    {
        Name = "Create Asset Categories",
        Code = "ASSET_CATEGORIES_CREATE",
        Description = "Allows creating asset categories.",
        Module = "Asset Categories"
    },

    new
    {
        Name = "Update Asset Categories",
        Code = "ASSET_CATEGORIES_UPDATE",
        Description = "Allows updating asset categories.",
        Module = "Asset Categories"
    },

    new
    {
        Name = "Delete Asset Categories",
        Code = "ASSET_CATEGORIES_DELETE",
        Description = "Allows deleting asset categories.",
        Module = "Asset Categories"
    },


    // --------------------------------------------------------
    // Suppliers
    // --------------------------------------------------------

    new
    {
        Name = "View Suppliers",
        Code = "SUPPLIERS_VIEW",
        Description = "Allows viewing suppliers.",
        Module = "Suppliers"
    },

    new
    {
        Name = "Create Suppliers",
        Code = "SUPPLIERS_CREATE",
        Description = "Allows creating suppliers.",
        Module = "Suppliers"
    },

    new
    {
        Name = "Update Suppliers",
        Code = "SUPPLIERS_UPDATE",
        Description = "Allows updating suppliers.",
        Module = "Suppliers"
    },

    new
    {
        Name = "Delete Suppliers",
        Code = "SUPPLIERS_DELETE",
        Description = "Allows deleting suppliers.",
        Module = "Suppliers"
    },


    // --------------------------------------------------------
    // Purchases
    // --------------------------------------------------------

    new
    {
        Name = "View Purchases",
        Code = "PURCHASES_VIEW",
        Description = "Allows viewing purchases.",
        Module = "Purchases"
    },

    new
    {
        Name = "Create Purchases",
        Code = "PURCHASES_CREATE",
        Description = "Allows creating purchases.",
        Module = "Purchases"
    },

    new
    {
        Name = "Update Purchases",
        Code = "PURCHASES_UPDATE",
        Description = "Allows updating purchases.",
        Module = "Purchases"
    },

    new
    {
        Name = "Delete Purchases",
        Code = "PURCHASES_DELETE",
        Description = "Allows deleting purchases.",
        Module = "Purchases"
    },


    // --------------------------------------------------------
    // Assets
    // --------------------------------------------------------

    new
    {
        Name = "View Assets",
        Code = "ASSETS_VIEW",
        Description = "Allows viewing assets.",
        Module = "Assets"
    },

    new
    {
        Name = "Create Assets",
        Code = "ASSETS_CREATE",
        Description = "Allows creating assets.",
        Module = "Assets"
    },

    new
    {
        Name = "Update Assets",
        Code = "ASSETS_UPDATE",
        Description = "Allows updating assets.",
        Module = "Assets"
    },

    new
    {
        Name = "Delete Assets",
        Code = "ASSETS_DELETE",
        Description = "Allows deleting assets.",
        Module = "Assets"
    },


    // --------------------------------------------------------
    // Asset Requests
    // --------------------------------------------------------

    new
    {
        Name = "View Asset Requests",
        Code = "ASSET_REQUESTS_VIEW",
        Description = "Allows viewing asset requests.",
        Module = "Asset Requests"
    },

    new
    {
        Name = "Create Asset Requests",
        Code = "ASSET_REQUESTS_CREATE",
        Description = "Allows creating asset requests.",
        Module = "Asset Requests"
    },

    new
    {
        Name = "Approve Asset Requests",
        Code = "ASSET_REQUESTS_APPROVE",
        Description = "Allows approving asset requests.",
        Module = "Asset Requests"
    },

    new
    {
        Name = "Reject Asset Requests",
        Code = "ASSET_REQUESTS_REJECT",
        Description = "Allows rejecting asset requests.",
        Module = "Asset Requests"
    },


    // --------------------------------------------------------
    // Asset Assignments
    // --------------------------------------------------------

    new
    {
        Name = "View Asset Assignments",
        Code = "ASSET_ASSIGNMENTS_VIEW",
        Description = "Allows viewing asset assignments.",
        Module = "Asset Assignments"
    },

    new
    {
        Name = "Create Asset Assignments",
        Code = "ASSET_ASSIGNMENTS_CREATE",
        Description = "Allows assigning assets.",
        Module = "Asset Assignments"
    },

    new
    {
        Name = "Update Asset Assignments",
        Code = "ASSET_ASSIGNMENTS_UPDATE",
        Description = "Allows updating asset assignments.",
        Module = "Asset Assignments"
    },


    // --------------------------------------------------------
    // Asset Transfers
    // --------------------------------------------------------

    new
    {
        Name = "View Asset Transfers",
        Code = "ASSET_TRANSFERS_VIEW",
        Description = "Allows viewing asset transfers.",
        Module = "Asset Transfers"
    },

    new
    {
        Name = "Create Asset Transfers",
        Code = "ASSET_TRANSFERS_CREATE",
        Description = "Allows creating asset transfers.",
        Module = "Asset Transfers"
    },

    new
    {
        Name = "Approve Asset Transfers",
        Code = "ASSET_TRANSFERS_APPROVE",
        Description = "Allows approving asset transfers.",
        Module = "Asset Transfers"
    },


    // --------------------------------------------------------
    // Asset Returns
    // --------------------------------------------------------

    new
    {
        Name = "View Asset Returns",
        Code = "ASSET_RETURNS_VIEW",
        Description = "Allows viewing asset returns.",
        Module = "Asset Returns"
    },

    new
    {
        Name = "Create Asset Returns",
        Code = "ASSET_RETURNS_CREATE",
        Description = "Allows creating asset returns.",
        Module = "Asset Returns"
    },

    new
    {
        Name = "Approve Asset Returns",
        Code = "ASSET_RETURNS_APPROVE",
        Description = "Allows approving asset returns.",
        Module = "Asset Returns"
    },


    // --------------------------------------------------------
    // Damage Reports
    // --------------------------------------------------------

    new
    {
        Name = "View Damage Reports",
        Code = "DAMAGE_REPORTS_VIEW",
        Description = "Allows viewing damage reports.",
        Module = "Damage Reports"
    },

    new
    {
        Name = "Create Damage Reports",
        Code = "DAMAGE_REPORTS_CREATE",
        Description = "Allows reporting asset damage.",
        Module = "Damage Reports"
    },

    new
    {
        Name = "Review Damage Reports",
        Code = "DAMAGE_REPORTS_REVIEW",
        Description = "Allows reviewing damage reports.",
        Module = "Damage Reports"
    },


    // --------------------------------------------------------
    // Maintenance
    // --------------------------------------------------------

    new
    {
        Name = "View Maintenance",
        Code = "MAINTENANCE_VIEW",
        Description = "Allows viewing maintenance records.",
        Module = "Maintenance"
    },

    new
    {
        Name = "Create Maintenance",
        Code = "MAINTENANCE_CREATE",
        Description = "Allows creating maintenance records.",
        Module = "Maintenance"
    },

    new
    {
        Name = "Update Maintenance",
        Code = "MAINTENANCE_UPDATE",
        Description = "Allows updating maintenance records.",
        Module = "Maintenance"
    },


    // --------------------------------------------------------
    // Asset Disposal
    // --------------------------------------------------------

    new
    {
        Name = "View Asset Disposals",
        Code = "ASSET_DISPOSALS_VIEW",
        Description = "Allows viewing asset disposal records.",
        Module = "Asset Disposals"
    },

    new
    {
        Name = "Create Asset Disposals",
        Code = "ASSET_DISPOSALS_CREATE",
        Description = "Allows creating asset disposal requests.",
        Module = "Asset Disposals"
    },

    new
    {
        Name = "Approve Asset Disposals",
        Code = "ASSET_DISPOSALS_APPROVE",
        Description = "Allows approving asset disposals.",
        Module = "Asset Disposals"
    },


    // --------------------------------------------------------
    // Notifications
    // --------------------------------------------------------

    new
    {
        Name = "View Notifications",
        Code = "NOTIFICATIONS_VIEW",
        Description = "Allows viewing notifications.",
        Module = "Notifications"
    },

    new
    {
        Name = "Manage Notifications",
        Code = "NOTIFICATIONS_MANAGE",
        Description = "Allows managing notifications.",
        Module = "Notifications"
    },


    // --------------------------------------------------------
    // Files
    // --------------------------------------------------------

    new
    {
        Name = "View Files",
        Code = "FILES_VIEW",
        Description = "Allows viewing file attachments.",
        Module = "Files"
    },

    new
    {
        Name = "Upload Files",
        Code = "FILES_UPLOAD",
        Description = "Allows uploading file attachments.",
        Module = "Files"
    },

    new
    {
        Name = "Delete Files",
        Code = "FILES_DELETE",
        Description = "Allows deleting file attachments.",
        Module = "Files"
    },


    // --------------------------------------------------------
    // Audit Logs
    // --------------------------------------------------------

    new
    {
        Name = "View Audit Logs",
        Code = "AUDIT_LOGS_VIEW",
        Description = "Allows viewing audit logs.",
        Module = "Audit Logs"
    },


    // --------------------------------------------------------
    // Dashboard
    // --------------------------------------------------------

    new
    {
        Name = "View Dashboard",
        Code = "DASHBOARD_VIEW",
        Description = "Allows viewing the system dashboard.",
        Module = "Dashboard"
    }
};


        foreach (var definition in permissionDefinitions)
        {
            var permission =
                await context.Permissions
                    .FirstOrDefaultAsync(
                        x => x.Code == definition.Code,
                        cancellationToken);

            if (permission is null)
            {
                permission =
                    Permission.Create(
                        name: definition.Name,
                        code: definition.Code,
                        description: definition.Description,
                        module: definition.Module,
                        createdBy: adminUser.Id);

                await context.Permissions.AddAsync(
                    permission,
                    cancellationToken);
            }
        }

        await context.SaveChangesAsync(
            cancellationToken);


        // ============================================================
        // 7. Assign Permissions to Administrator Role
        // ============================================================

        var permissions =
            await context.Permissions
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(cancellationToken);

        foreach (var permission in permissions)
        {
            var rolePermissionExists =
                await context.RolePermissions
                    .AnyAsync(
                        x =>
                            x.RoleId == adminRole.Id &&
                            x.PermissionId == permission.Id,
                        cancellationToken);

            if (!rolePermissionExists)
            {
                var rolePermission =
                    RolePermission.Create(
                        roleId: adminRole.Id,
                        permissionId: permission.Id,
                        assignedBy: adminUser.Id);

                await context.RolePermissions.AddAsync(
                    rolePermission,
                    cancellationToken);
            }
        }

        await context.SaveChangesAsync(
            cancellationToken);


        // ============================================================
        // 8. Final Save
        // ============================================================

        await context.SaveChangesAsync(
            cancellationToken);
    }
}