using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.FileAttachments;

namespace UAMS.Infrastructure.Configurations;

public class FileAttachmentConfiguration : IEntityTypeConfiguration<FileAttachment>
{
    public void Configure(EntityTypeBuilder<FileAttachment> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("FileAttachments");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(fa => fa.Id);

        builder.Property(fa => fa.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Uploaded By
        // ============================================================

        builder.Property(fa => fa.UploadedById)
            .IsRequired();

        builder.Property(fa => fa.UploadedAt)
            .IsRequired();


        // ============================================================
        // Related Entity
        // ============================================================

        builder.Property(fa => fa.EntityName)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(fa => fa.EntityId)
            .IsRequired();


        // ============================================================
        // File Information
        // ============================================================

        builder.Property(fa => fa.FileName)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(true);

        builder.Property(fa => fa.StoredFileName)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(fa => fa.FilePath)
            .IsRequired()
            .HasMaxLength(1000)
            .IsUnicode(false);

        builder.Property(fa => fa.ContentType)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(fa => fa.FileExtension)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(fa => fa.FileSize)
            .IsRequired();

        builder.Property(fa => fa.FileType)
            .IsRequired();

        builder.Property(fa => fa.Description)
            .HasMaxLength(1000)
            .IsUnicode(true);

        builder.Property(fa => fa.Checksum)
            .HasMaxLength(256)
            .IsUnicode(false);


        // ============================================================
        // Status
        // ============================================================

        builder.Property(fa => fa.Status)
            .IsRequired();


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(fa => fa.UploadedById);

        builder.HasIndex(fa => fa.UploadedAt);

        builder.HasIndex(fa => fa.Status);

        builder.HasIndex(fa => new
        {
            fa.EntityName,
            fa.EntityId
        });


        // ============================================================
        // Uploaded By User Relationship
        // ============================================================

        builder.HasOne(fa => fa.UploadedBy)
            .WithMany()
            .HasForeignKey(fa => fa.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(fa => fa.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(fa => !fa.IsDeleted);
    }
}