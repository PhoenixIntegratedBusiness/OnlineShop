using Application.Services.Interfaces;
using Domain.ViewModel.ProductGroupViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.web.Web.Extentions;
using System.Runtime.InteropServices;

namespace OnlineShop.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductGroupController : Controller
    {
        private readonly IProductGroupService productGroupService;
        public ProductGroupController(IProductGroupService productGroupService)
        {
            this.productGroupService = productGroupService;
        }

        #region ProductGroupList
        [Route("/ProductGroupList")]
        public async Task<IActionResult> ProductGroupList()
        {
            return View(await productGroupService.GetProductGroupsAsync());
        }
        #endregion

        #region ProductGroupDetails
        [Route("/ProductGroupDetails")]
        public async Task<IActionResult> ProductGroupDetails(int groupId)
        {
            return View(await productGroupService.GetGroupByIdAsync(groupId));
        }
        #endregion

        #region CreateProductGroup
        [Route("/CreateProductGroup")]
        public IActionResult CreateProductGroup()
        {
            return View();
        }

        [HttpPost("/CreateProductGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductGroup(CreateProductGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await productGroupService.AddGroupAsync(model);
            switch (res)
            {
                case Application.Enums.ProductGroup.GroupResult.Success:

                    TempData["AlertType"] = SwalExtentions.Success;
                    TempData["AlertMessage"] = "Product group created successfully.";
                    return RedirectToAction(nameof(ProductGroupList));

                case Application.Enums.ProductGroup.GroupResult.Failure:
                    TempData["AlertType"] = SwalExtentions.Error;
                    TempData["AlertMessage"] = "Operation Failed.";
                    break;
                case Application.Enums.ProductGroup.GroupResult.DuplicateTitle:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Product group title already exists.";
                    break;
            }
            return View();
        }

        #endregion

        #region EditProductGroup

        [Route("/EditProductGroup")]
        public async Task<IActionResult> EditProductGroup(int groupId)
        {
            return View(await productGroupService.GetGroupByIdAsync(groupId));
        }


        [HttpPost("/EditProductGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductGroup(ProductGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var res = await productGroupService.UpdateGroupAsync(model);

            switch (res)
            {
                case Application.Enums.ProductGroup.GroupResult.Success:

                    TempData["AlertType"] = SwalExtentions.Success;
                    TempData["AlertMessage"] = "Product group Edited successfully.";
                    return RedirectToAction(nameof(ProductGroupList));

                case Application.Enums.ProductGroup.GroupResult.Failure:
                    TempData["AlertType"] = SwalExtentions.Error;
                    TempData["AlertMessage"] = "Operation Failed.";
                    break;
            }
            return View(model);
        }
        #endregion

        #region DeleteGroup
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var res = await productGroupService.DeleteGroupAsync(groupId);
            if (res == true)
            {
                TempData["AlertType"] = SwalExtentions.Error;
                TempData["AlertMessage"] = "Operation failed.";
            }
            TempData["AlertType"] = SwalExtentions.Success;
            TempData["AlertMessage"] = "Product group deleted successfully";
            return RedirectToAction(nameof(ProductGroupList));
        }

        #endregion

    }
}
