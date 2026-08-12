using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> ProductExistsAsync(int id, string title);
        Task<bool> AddProductAsync(Product product);
        Task SavechangeAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool?> DeleteGallaryImgByIdAsync(int id);
        Task<string?> FindImgGallaryAsync(int id);
        Task<List<ProductGallery?>> GetAllProuctGalleriesAsync(int id);
        Task GetAllTagsByIdAsync(int id);
        void UpdateProduct(Product product);
        Task<bool> IsTitleExistAsync(string title, int productId);
        Task<List<Product>> GetProductsByGruoupIdAsync(int id);

    }
}
