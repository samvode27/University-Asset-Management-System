using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Permissions;

namespace UAMS.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Permissions");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(p => p.Id);


        // ============================================================
        // Properties
        // ============================================================

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Description)
            .HasMaxLength(500);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasIndex(p => p.Code)
            .IsUnique();


        // ============================================================
        // Relationships
        // ============================================================

        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Query Filter
        // ============================================================

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}