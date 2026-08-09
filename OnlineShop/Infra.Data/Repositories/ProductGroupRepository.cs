using Domain.Interfaces;
using Domain.Model;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly MyContext _context;
        public ProductGroupRepository(MyContext context)
        {
            _context = context;
        }

        #region AddGroupAsync
        public async Task AddGroupAsync(ProductGroup group)
        {
            await _context.ProductGroups.AddAsync(group);
        }
        #endregion

        #region GetGroupByIdAsync
        public async Task<ProductGroup?> GetGroupByIdAsync(int id)
        {
            return await _context.ProductGroups.FindAsync(id);
        }
        #endregion

        #region GetGroupByNameAsync
        public async Task<bool> GetGroupByNameAsync(string title)
        {
            if (await _context.ProductGroups.FirstOrDefaultAsync(u => u.GroupTitle == title) != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region GetProductGroupsAsync
        public async Task<List<ProductGroup>> GetProductGroupsAsync()
        {
            return await _context.ProductGroups.ToListAsync();
        }
        #endregion

        #region SaveChangeAsync
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdateGroup
        public void UpdateGroup(ProductGroup group)
        {
            _context.ProductGroups.Update(group);
        }
        #endregion

        #region IsGroupTitleExistsAsync
        public async Task<bool> IsGroupTitleExistsAsync(string title, int groupId)
        {
            return await _context.ProductGroups.AnyAsync(x =>x.GroupTitle == title &&x.GroupId != groupId);
        }
        #endregion

    }
}
