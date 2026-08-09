using Application.Enums.ProductGroup;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.ProductGroupViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly IProductGroupRepository _productGroupRepository;
        public ProductGroupService(IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        #region DeleteGroupAsync
        public async Task<bool> DeleteGroupAsync(int id)
        {
            var res = await _productGroupRepository.GetGroupByIdAsync(id);
            if (res == null)
            {
                return false;
            }
            res.isDelete = true;
            await _productGroupRepository.SaveChangeAsync();
            return true;
        }

        #endregion

        #region UpdateGroupAsync
        public async Task<GroupResult> UpdateGroupAsync(ProductGroupViewModel model)
        {
            var exists = await _productGroupRepository.IsGroupTitleExistsAsync(model.GroupTitle.ToLower().Trim(), model.GroupId);
            if (exists)
            {
                return GroupResult.DuplicateTitle;
            }
            var group = new ProductGroup()
            {
                CreateDate = model.CreateDate,
                GroupTitle = model.GroupTitle.ToLower().Trim(),
                isDelete = model.isDelete,
                GroupId = model.GroupId,
            };
            _productGroupRepository.UpdateGroup(group);
            await _productGroupRepository.SaveChangeAsync();
            return GroupResult.Success;
        }
        #endregion

        #region AddGroupAsync
        public async Task<GroupResult> AddGroupAsync(CreateProductGroupViewModel model)
        {
            var res = await _productGroupRepository.GetGroupByNameAsync(model.GroupTitle.ToLower().Trim());
            if (res == false)
            {
                var group = new ProductGroup()
                {
                    CreateDate = DateTime.Now,
                    GroupTitle = model.GroupTitle.ToLower().Trim(),
                    isDelete = false,
                };
                await _productGroupRepository.AddGroupAsync(group);
                await _productGroupRepository.SaveChangeAsync();
                return GroupResult.Success;
            }

            return GroupResult.DuplicateTitle;
        }
        #endregion

        #region GetGroupByIdAsync
        public async Task<ProductGroupViewModel?> GetGroupByIdAsync(int id)
        {
            var group = await _productGroupRepository.GetGroupByIdAsync(id);
            if (group == null)
                return null;
            return new ProductGroupViewModel()
            {
                CreateDate = group.CreateDate,
                GroupId = group.GroupId,
                GroupTitle = group.GroupTitle,
                isDelete = group.isDelete
            };
        }

        #endregion

        #region GetProductGroupsAsync
        public async Task<List<ProductGroupViewModel>> GetProductGroupsAsync()
        {
            var group = await _productGroupRepository.GetProductGroupsAsync();

            return group.Select(item => new ProductGroupViewModel()
            {
                CreateDate = item.CreateDate,
                GroupId = item.GroupId,
                GroupTitle = item.GroupTitle,
                isDelete = item.isDelete
            }).ToList();
        }
        #endregion
    }
}
