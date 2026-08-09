using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Infrastructure.Configurations;

public class RolePermissionConfiguration
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(
        EntityTypeBuilder<RolePermission> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("RolePermissions");


        // ============================================================
        // Composite Primary Key
        // ============================================================

        builder.HasKey(rp => new
        {
            rp.RoleId,
            rp.PermissionId
        });


        // ============================================================
        // Relationships
        // ============================================================

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}