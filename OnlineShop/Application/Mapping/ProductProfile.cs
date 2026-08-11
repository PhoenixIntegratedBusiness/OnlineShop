using AutoMapper;
using Domain.Model;
using Domain.ViewModel.ProductGroupViewModel;
using Domain.ViewModel.ProductViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, DeleteProductViewModel>();

            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();

            CreateMap<Product, EditProductviewModel>();
            CreateMap<EditProductviewModel, Product>();

            CreateMap<CreateProductViewModel, Product>()
                     .ForMember(x => x.ImageName, opt => opt.Ignore())
                     .ForMember(x => x.CreateDate, opt => opt.Ignore())
                     .ForMember(x => x.Tags, opt => opt.Ignore())
                     .ForMember(x => x.ProductGallery, opt => opt.Ignore());


            CreateMap<DeleteProductViewModel, Product>();
            CreateMap<Product, DeleteProductViewModel>();
        }
    }
}
