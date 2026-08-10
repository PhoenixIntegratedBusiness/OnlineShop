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
    }
}
