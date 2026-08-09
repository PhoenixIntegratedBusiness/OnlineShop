using Domain.Model;
using Domain.ViewModel.ProductGroupViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductGroupRepository
    {
        Task<List<ProductGroup>> GetProductGroupsAsync();
        Task<ProductGroup?> GetGroupByIdAsync(int id);
        Task<bool> GetGroupByNameAsync(string title);
        Task AddGroupAsync(ProductGroup group);
        Task  SaveChangeAsync();
        void UpdateGroup(ProductGroup group);
        Task<bool> IsGroupTitleExistsAsync(string title, int groupId);

    }
}
