using Ecommerce.BLL.Entities;

namespace Ecommerce.BLL.Interfaces.Services
{
    public interface IProductService : IDisposable
    {
        Task Add(Product product);
        Task Update(Product product);
        Task Remove(Guid id);
    }
}