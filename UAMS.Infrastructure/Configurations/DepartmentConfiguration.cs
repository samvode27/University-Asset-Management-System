using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Departments;

namespace UAMS.Infrastructure.Configurations;

public class DepartmentConfiguration
    : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Departments", "Organization");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Properties
        // ============================================================

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(true);

        builder.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(d => d.Description)
            .HasMaxLength(500)
            .IsUnicode(true);

        builder.Property(d => d.OfficeLocation)
            .HasMaxLength(250)
            .IsUnicode(true);


        // ============================================================
        // Status
        // ============================================================

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Established Date
        // ============================================================

        builder.Property(d => d.EstablishedDate);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(d => d.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(d => d.Name)
            .IsUnique();

        builder.HasIndex(d => d.Code)
            .IsUnique();

        builder.HasIndex(d => d.DepartmentHeadId);


        // ============================================================
        // Department → Users
        // ============================================================

        builder.HasMany(d => d.Users)
            .WithOne(u => u.Department)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Department → Assets
        // ============================================================

        builder.HasMany(d => d.Assets)
            .WithOne(a => a.Department)
            .HasForeignKey(a => a.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Department → Asset Requests
        // ============================================================

        builder.HasMany(d => d.AssetRequests)
            .WithOne(ar => ar.Department)
            .HasForeignKey(ar => ar.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Department → Department Head
        // ============================================================

        builder.HasOne(d => d.DepartmentHead)
            .WithMany()
            .HasForeignKey(d => d.DepartmentHeadId)
            .OnDelete(DeleteBehavior.SetNull);


        // ============================================================
        // Global Query Filter
        // ============================================================

        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}