using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Barcodes;

namespace UAMS.Infrastructure.Configurations;

public class BarcodeConfiguration
    : IEntityTypeConfiguration<Barcode>
{
    public void Configure(EntityTypeBuilder<Barcode> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Barcodes");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Asset Foreign Key
        // ============================================================

        builder.Property(b => b.AssetId)
            .IsRequired();


        // ============================================================
        // Barcode
        // ============================================================

        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);


        // ============================================================
        // Barcode Value
        // ============================================================

        builder.Property(b => b.EncodedData)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Barcode Type
        // ============================================================

        builder.Property(b => b.Format)
            .IsRequired()
            .HasConversion<int>();


        // ============================================================
        // Image Path
        // ============================================================

        builder.Property(b => b.ImagePath)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Generated Date
        // ============================================================

        builder.Property(b => b.GeneratedAt)
            .IsRequired();


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(b => b.AssetId)
            .IsUnique();

        builder.HasIndex(b => b.Code)
            .IsUnique();


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(b => b.Asset)
            .WithOne(a => a.Barcode)
            .HasForeignKey<Barcode>(b => b.AssetId)
            .OnDelete(DeleteBehavior.Cascade);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(b => b.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}