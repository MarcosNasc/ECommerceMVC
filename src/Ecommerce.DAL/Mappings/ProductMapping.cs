using Ecommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("VARCHAR(1000)");

            builder.Property(p => p.Image)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(p => p.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.ToTable("Products");
        }
    }
}
