using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Users");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(u => u.Id);


        // ============================================================
        // Properties
        // ============================================================

        builder.Property(u => u.EmployeeId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(u => u.EmployeeId)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Username)
            .IsUnique();


        // ============================================================
        // Department Relationship
        // ============================================================

        builder.HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Query Filter
        // ============================================================

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}