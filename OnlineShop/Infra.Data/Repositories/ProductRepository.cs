using Domain.Interfaces;
using Domain.Model;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly MyContext _context;
        public ProductRepository(MyContext context)
        {
            _context = context;
        }

        #region AddProductAsync
        public async Task<bool> AddProductAsync(Product product)
        {
            var res = await _context.Products.AddAsync(product);
            return true;
        }
        #endregion

        #region GetAllProductsAsync
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(u => u.ProductGroup).ToListAsync();
        }
        #endregion

        #region ProductExistsAsync
        public async Task<bool> ProductExistsAsync(int id, string title)
        {
            var res = await _context.Products.AnyAsync(u => u.Title == title && u.GroupId == id);
            if (res == null)
            {
                return true;
            }
            return false;
        }


        #endregion

        #region SavechangeAsync
        public async Task SavechangeAsync()
        {
           await _context.SaveChangesAsync();
        }
        #endregion

    }
}
