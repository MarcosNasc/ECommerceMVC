namespace Ecommerce.BLL.Entities
{
    public class Product : Entity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool  IsActive { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

    }
}
