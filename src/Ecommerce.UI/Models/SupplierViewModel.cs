using Ecommerce.UI.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.UI.Models
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 2)]
        public string? Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(15, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 2)]
        public string? Document { get; set; }
        [DisplayName("Tipo")]
        public int SupplierType { get; set; }
        public AddressViewModel? Address { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        public IEnumerable<ProductViewModel>? Products { get; set; }
    }
}
