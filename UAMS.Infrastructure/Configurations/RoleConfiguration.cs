using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Roles");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(r => r.Id);


        // ============================================================
        // Properties
        // ============================================================

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(500);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.HasIndex(r => r.Code)
            .IsUnique();


        // ============================================================
        // Relationships
        // ============================================================

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Query Filter
        // ============================================================

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}