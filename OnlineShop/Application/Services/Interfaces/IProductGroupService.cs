using Application.Enums.ProductGroup;
using Domain.Model;
using Domain.ViewModel.ProductGroupViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductGroupService
    {
        Task<List<ProductGroupViewModel>> GetProductGroupsAsync();
        Task<ProductGroupViewModel> GetGroupByIdAsync(int id);
        Task<GroupResult> AddGroupAsync(CreateProductGroupViewModel model);
        Task<GroupResult> UpdateGroupAsync(ProductGroupViewModel model);
        Task<bool> DeleteGroupAsync(int id);
    }
}

