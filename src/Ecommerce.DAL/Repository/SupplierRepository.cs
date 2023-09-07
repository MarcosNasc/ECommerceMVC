using Ecommerce.BLL.Entities;
using Ecommerce.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BLL.Interfaces.Repositories;

namespace Ecommerce.DAL.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(EcommerceDBContext context) : base(context){}

        public async Task<Supplier?> GetSupplierAdress(Guid id)
        {
            return await _table.AsNoTracking()
                               .Include(s => s.Address)
                               .FirstOrDefaultAsync(s => s.Id == id) ?? null;
                        
                   
        }

        public async Task<Supplier?> GetSupplierProductsAddress(Guid id)
        {
            return await _table.AsNoTracking()
                               .Include(s => s.Products)
                               .Include(s => s.Address)
                               .FirstOrDefaultAsync(p => p.Id == id) ?? null;
        }
    }
}
