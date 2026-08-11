using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Notifications;

namespace UAMS.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Notifications");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Foreign Key
        // ============================================================

        builder.Property(n => n.UserId)
            .IsRequired();


        // ============================================================
        // Notification Content
        // ============================================================

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(250)
            .IsUnicode(true);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(2000)
            .IsUnicode(true);


        // ============================================================
        // Notification Type
        // ============================================================

        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(n => n.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(n => n.Status)
            .IsRequired()
            .HasConversion<int>();


        // ============================================================
        // Reference Information
        // ============================================================

        builder.Property(n => n.ReferenceId);

        builder.Property(n => n.ReferenceType)
            .HasMaxLength(100)
            .IsUnicode(false);


        // ============================================================
        // Action
        // ============================================================

        builder.Property(n => n.ActionUrl)
            .HasMaxLength(500)
            .IsUnicode(false);


        // ============================================================
        // Read Information
        // ============================================================

        builder.Property(n => n.ReadAt);


        // ============================================================
        // Expiration
        // ============================================================

        builder.Property(n => n.ExpiresAt);


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(n => n.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(n => n.UserId);

        builder.HasIndex(n => n.Status);

        builder.HasIndex(n => n.Type);

        builder.HasIndex(n => n.Priority);

        builder.HasIndex(n => n.CreatedAt);

        builder.HasIndex(n => n.ExpiresAt);

        builder.HasIndex(n => new
        {
            n.UserId,
            n.Status
        });

        builder.HasIndex(n => new
        {
            n.ReferenceType,
            n.ReferenceId
        });


        // ============================================================
        // User Relationship
        // ============================================================

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(n => n.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}