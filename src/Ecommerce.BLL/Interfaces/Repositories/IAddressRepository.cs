using Ecommerce.BLL.Entities;

namespace Ecommerce.BLL.Interfaces.Repositories
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address?> GetAddressBySupplier(Guid supplierId);
    }
}
