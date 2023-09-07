using Ecommerce.BLL.Entities;
using Ecommerce.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BLL.Interfaces.Repositories;

namespace Ecommerce.DAL.Repository
{
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        public ProductRepository(EcommerceDBContext context) : base(context){}

        public async Task<Product?> GetProductSupplier(Guid id)
        {
            var product = await _table.AsNoTracking()
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id) ?? null;
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsSuppliers()
        {
            var products = await _table.AsNoTracking()
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplier(Guid supplierId)
        {
            return await Search(p => p.SupplierId == supplierId);
        }

    }
}
