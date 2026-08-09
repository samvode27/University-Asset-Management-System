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
        // Composite Primary Key
        // ============================================================

        builder.HasKey(ur => new
        {
            ur.UserId,
            ur.RoleId
        });


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