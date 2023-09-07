using Ecommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceMVC.App.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.StreetName)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");

            builder.Property(e => e.Number)
                .IsRequired()
                .HasColumnType("VARCHAR(50)");

            builder.Property(e => e.ZipCode)
                .IsRequired()
                .HasColumnType("VARCHAR(8)");

            builder.Property(e => e.Complement)
                .HasColumnType("VARCHAR(250)");

            builder.Property(e => e.Neighborhood)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(e => e.City)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(e => e.State)
                .IsRequired()
                .HasColumnType("VARCHAR(50)");

            builder.ToTable("Addresses");
        }
    }
}
