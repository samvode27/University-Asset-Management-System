using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Purchases;

namespace UAMS.Infrastructure.Configurations;

public class PurchaseConfiguration
    : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Purchases", "Procurement");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Purchase Number
        // ============================================================

        builder.Property(p => p.PurchaseNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Supplier
        // ============================================================

        builder.Property(p => p.SupplierId)
            .IsRequired();


        // ============================================================
        // Purchase Date
        // ============================================================

        builder.Property(p => p.PurchaseDate)
            .IsRequired();


        // ============================================================
        // Invoice Number
        // ============================================================

        builder.Property(p => p.InvoiceNumber)
            .HasMaxLength(100)
            .IsUnicode(false);


        // ============================================================
        // Purchase Order Number
        // ============================================================

        builder.Property(p => p.PurchaseOrderNumber)
            .HasMaxLength(100)
            .IsUnicode(false);


        // ============================================================
        // Total Amount
        // ============================================================

        builder.Property(p => p.TotalAmount)
            .HasPrecision(18, 2);


        // ============================================================
        // Currency
        // ============================================================

        builder.Property(p => p.Currency)
            .HasMaxLength(10)
            .IsUnicode(false);


        // ============================================================
        // Description
        // ============================================================

        builder.Property(p => p.Description)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Status
        // ============================================================

        builder.Property(p => p.Status)
            .IsRequired();


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(p => p.PurchaseNumber)
            .IsUnique();

        builder.HasIndex(p => p.SupplierId);

        builder.HasIndex(p => p.PurchaseDate);

        builder.HasIndex(p => p.InvoiceNumber);


        // ============================================================
        // Supplier Relationship
        // ============================================================

        builder.HasOne(p => p.Supplier)
            .WithMany(s => s.Purchases)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Purchase → Assets
        // ============================================================

        builder.HasMany(p => p.Assets)
            .WithOne(a => a.Purchase)
            .HasForeignKey(a => a.PurchaseId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}