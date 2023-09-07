using Ecommerce.BLL.Entities;

namespace Ecommerce.BLL.Interfaces.Repositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier?> GetSupplierAdress(Guid id);
        Task<Supplier?> GetSupplierProductsAddress(Guid id);
    }
}
