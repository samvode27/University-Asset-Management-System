using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AuditLogs;

namespace UAMS.Infrastructure.Configurations;

public class AuditLogConfiguration
    : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AuditLogs");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(al => al.Id);

        builder.Property(al => al.Id)
            .ValueGeneratedNever();


        // ============================================================
        // User
        // ============================================================

        builder.Property(al => al.UserId);


        // ============================================================
        // Action
        // ============================================================

        builder.Property(al => al.Action)
            .IsRequired()
            .HasConversion<int>();


        // ============================================================
        // Entity Information
        // ============================================================

        builder.Property(al => al.EntityName)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(al => al.EntityId);


        // ============================================================
        // Change Information
        // ============================================================

        builder.Property(al => al.OldValues);

        builder.Property(al => al.NewValues);


        // ============================================================
        // Request Information
        // ============================================================

        builder.Property(al => al.IpAddress)
            .HasMaxLength(45)
            .IsUnicode(false);

        builder.Property(al => al.UserAgent)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Timestamp
        // ============================================================

        builder.Property(al => al.CreatedAt)
            .IsRequired();


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(al => al.UserId);

        builder.HasIndex(al => al.Action);

        builder.HasIndex(al => al.EntityName);

        builder.HasIndex(al => al.EntityId);

        builder.HasIndex(al => al.CreatedAt);

        builder.HasIndex(al => new
        {
            al.EntityName,
            al.EntityId
        });

        builder.HasIndex(al => new
        {
            al.UserId,
            al.CreatedAt
        });


        // ============================================================
        // User Relationship
        // ============================================================

        builder.HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(al => al.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(al => !al.IsDeleted);
    }
}
