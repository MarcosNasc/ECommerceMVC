using Ecommerce.BLL.Entities;
using Ecommerce.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BLL.Interfaces.Repositories;

namespace Ecommerce.DAL.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(EcommerceDBContext context) : base(context){}

        public async Task<Address?> GetAddressBySupplier(Guid supplierId)
        {
            return await _table.AsNoTracking().FirstOrDefaultAsync(a => a.SupplierId == supplierId) ?? null;
        }
    }
}
