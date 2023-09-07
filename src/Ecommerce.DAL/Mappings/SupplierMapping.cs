using Ecommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceMVC.App.Data.Mappings
{
    public class SupplierMapping : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired()
               .HasColumnType("VARCHAR(200)");

            builder.Property(f => f.Document)
                .IsRequired()
                .HasColumnType("VARCHAR(14)");

            builder.HasOne(f => f.Address)
                .WithOne(e => e.Supplier)
                .HasForeignKey<Address>(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(f => f.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Suppliers");
        }
    }
}
