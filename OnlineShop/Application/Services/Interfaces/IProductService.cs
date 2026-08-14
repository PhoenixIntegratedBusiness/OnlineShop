using Application.Enums.Product;
using Domain.Model;
using Domain.ViewModel.ProductViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetAllProductsAsync();
        Task<CreateProductResult> CreateProductAsync(CreateProductViewModel model, IFormFile ImgUpload, IFormFile[] Gimgupload, string Tags);
        Task<EditProductviewModel> EditProductByIdAsync(int id);
        Task DeleteGallaryImgByIdAsync(int id);
        Task<UpdateProductResult> UpdateProductAsync(EditProductviewModel model, IFormFile ImgUpload, IFormFile[] Gimgupload, string TagsText);
        Task<DeleteProductViewModel> FindDeleteProduct(int id);    
        Task DeleteProductById(int id);
        Task<List<ProductCardViewModel>> GetAllProductCardItemAsync();
        Task<List<ProductCardViewModel>> GetProductsByGruoupIdAsync(int id);
        Task<AllProductDetailviewModel> GetProductByIdAsync(int id);
        Task<List<Product>> SearchProductKeyAsync(string keyword);
    }
}
