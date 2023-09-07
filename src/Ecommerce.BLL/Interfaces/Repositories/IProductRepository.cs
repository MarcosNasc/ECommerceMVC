using Ecommerce.BLL.Entities;

namespace Ecommerce.BLL.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsBySupplier(Guid id);
        Task<IEnumerable<Product>> GetProductsSuppliers();
        Task<Product?> GetProductSupplier(Guid supplierId);

    }
}
