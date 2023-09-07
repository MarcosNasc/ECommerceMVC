using Ecommerce.BLL.Entities;

namespace Ecommerce.BLL.Interfaces.Services
{
    public interface ISupplierService : IDisposable
    {
        Task Add(Supplier supplier);
        Task Update(Supplier supplier);
        Task Remove(Guid id);
        Task UpdateAddress(Address address);
    }
}