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


        #region GetProductsByGruoupIdAsync
        public async Task<List<Product>> GetProductsByGruoupIdAsync(int id)
        {
            return await _context.Products.Include(u=>u.ProductGroup).Include(u=>u.Tags).Where(u=>u.GroupId == id && u.isDelete==false).ToListAsync();
        }

        #endregion


        #region IsTitleExistAsync
        public async Task<bool> IsTitleExistAsync(string title, int productId)
        {
            return await _context.Products
                .AnyAsync(x => x.Title == title && x.ProductId != productId);
        }
        #endregion

        #region UpdateProduct
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
        #endregion

        #region GetAllTagsByIdAsync
        public async Task GetAllTagsByIdAsync(int id)
        {
            var res = await _context.Tags.Where(u => u.ProductId == id).ToListAsync();
            _context.Tags.RemoveRange(res);

        }
        #endregion

        #region listproductGallery
        public async Task<List<ProductGallery?>> GetAllProuctGalleriesAsync(int id)
        {
            return await _context.ProductGallery.Where(t => t.ProductId == id).ToListAsync();
        }
        #endregion

        #region FindImgGallaryAsync
        public async Task<string?> FindImgGallaryAsync(int id)
        {
            var gallary = await _context.ProductGallery.FindAsync(id);
            return gallary?.ImageName;

        }
        #endregion

        #region DeleteGallaryImgByIdAsync
        public async Task<bool?> DeleteGallaryImgByIdAsync(int id)
        {
            var gid = await _context.ProductGallery.FindAsync(id);
            if (gid != null)
            {
                _context.ProductGallery.Remove(gid);
                return true;
            }
            else return false;
        }
        #endregion

        #region GetProductByIdAsync
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                                         .Include(p => p.Tags)
                                         .Include(p => p.ProductGallery)
                                         .Include(p => p.ProductGroup)
                                         .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        #endregion

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
