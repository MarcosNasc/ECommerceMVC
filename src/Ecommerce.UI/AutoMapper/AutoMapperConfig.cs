using AutoMapper;
using Ecommerce.BLL.Entities;
using Ecommerce.UI.Models;

namespace Ecommerce.UI.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region Entities to ViewModel
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Supplier, SupplierViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            #endregion
        }
    }
}
