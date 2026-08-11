using Application.Enums.Product;
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
        Task<EditProductviewModel> GetProductByIdAsync(int id);
        Task DeleteGallaryImgByIdAsync(int id);
        Task<UpdateProductResult> UpdateProductAsync(EditProductviewModel model, IFormFile ImgUpload, IFormFile[] Gimgupload, string TagsText);
    }
}
