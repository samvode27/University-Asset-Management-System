using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AssetCategories;

namespace UAMS.Infrastructure.Configurations;

public class AssetCategoryConfiguration
    : IEntityTypeConfiguration<AssetCategory>
{
    public void Configure(EntityTypeBuilder<AssetCategory> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AssetCategories");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(ac => ac.Id);

        builder.Property(ac => ac.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Properties
        // ============================================================

        builder.Property(ac => ac.Name)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(true);

        builder.Property(ac => ac.Code)
            .IsRequired()
            .HasMaxLength(30)
            .IsUnicode(false);

        builder.Property(ac => ac.Description)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Status
        // ============================================================

        builder.Property(ac => ac.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(ac => ac.Name)
            .IsUnique();

        builder.HasIndex(ac => ac.Code)
            .IsUnique();


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasMany(ac => ac.Assets)
            .WithOne(a => a.AssetCategory)
            .HasForeignKey(a => a.AssetCategoryId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(ac => ac.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(ac => !ac.IsDeleted);
    }
}