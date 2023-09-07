using Ecommerce.BLL.Enum;

namespace Ecommerce.BLL.Entities
{
    public class Supplier : Entity
    {
        public string? Name { get; set; }
        public string? Document { get; set; }
        public SupplierType SupplierType { get; set; }
        public Address? Address { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
