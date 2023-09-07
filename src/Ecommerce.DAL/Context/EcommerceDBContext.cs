using Ecommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DAL.Context
{
    public class EcommerceDBContext  : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }


        public EcommerceDBContext(DbContextOptions options):base(options){
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcommerceDBContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
