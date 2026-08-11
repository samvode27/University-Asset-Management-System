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
        // Primary Key
        // ============================================================

        builder.HasKey(rp => rp.Id);

        // ============================================================
        // Unique Role-Permission Assignment
        // ============================================================

        builder.HasIndex(rp => new
        {
            rp.RoleId,
            rp.PermissionId
        })
        .IsUnique();


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