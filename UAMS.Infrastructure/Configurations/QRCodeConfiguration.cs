using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.QRCodes;

namespace UAMS.Infrastructure.Configurations;

public class QRCodeConfiguration
    : IEntityTypeConfiguration<QRCode>
{
    public void Configure(EntityTypeBuilder<QRCode> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("QRCodes", "AssetManagement");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Asset Foreign Key
        // ============================================================

        builder.Property(q => q.AssetId)
            .IsRequired();


        // ============================================================
        // QR Code
        // ============================================================

        builder.Property(q => q.Code)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);


        // ============================================================
        // QR Code Value
        // ============================================================

        builder.Property(q => q.EncodedData)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Image Path
        // ============================================================

        builder.Property(q => q.ImagePath)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Generated Date
        // ============================================================

        builder.Property(q => q.GeneratedAt)
            .IsRequired();


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(q => q.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(q => q.AssetId)
            .IsUnique();

        builder.HasIndex(q => q.Code)
            .IsUnique();


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(q => q.Asset)
            .WithOne(a => a.QRCode)
            .HasForeignKey<QRCode>(q => q.AssetId)
            .OnDelete(DeleteBehavior.Cascade);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(q => q.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(q => !q.IsDeleted);
    }
}