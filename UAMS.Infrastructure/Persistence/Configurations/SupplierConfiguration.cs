using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Suppliers;

namespace UAMS.Infrastructure.Configurations;

public class SupplierConfiguration
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Suppliers");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Supplier Code
        // ============================================================

        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(30)
            .IsUnicode(false);


        // ============================================================
        // Supplier Name
        // ============================================================

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(true);


        // ============================================================
        // Contact Person
        // ============================================================

        builder.Property(s => s.ContactPerson)
            .HasMaxLength(150)
            .IsUnicode(true);


        // ============================================================
        // Contact Information
        // ============================================================

        builder.Property(s => s.Email)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(30)
            .IsUnicode(false);


        // ============================================================
        // Address
        // ============================================================

        builder.Property(s => s.Address)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Tax Identification Number
        // ============================================================

        builder.Property(s => s.TaxIdentificationNumber)
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Status
        // ============================================================

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(s => s.Code)
            .IsUnique();

        builder.HasIndex(s => s.Name);


        // ============================================================
        // Purchase Relationship
        // ============================================================

        builder.HasMany(s => s.Purchases)
            .WithOne(p => p.Supplier)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(s => s.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}