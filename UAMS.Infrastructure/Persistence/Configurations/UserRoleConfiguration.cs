using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Configurations;

public class UserRoleConfiguration
    : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("UserRoles");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(ur => ur.Id);

        // ============================================================
        // Unique User-Role Assignment
        // ============================================================

        builder.HasIndex(ur => new
        {
            ur.UserId,
            ur.RoleId
        })
        .IsUnique();


        // ============================================================
        // User Relationship
        // ============================================================

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Role Relationship
        // ============================================================

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}