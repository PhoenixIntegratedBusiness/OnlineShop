using Application.Enums.Product;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.ViewModel.ProductViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.web.Web.Extentions;

namespace OnlineShop.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductGroupService productGroupservice;
        public ProductController(IProductService productService, IProductGroupService productGroupService)
        {
            this.productService = productService;
            this.productGroupservice = productGroupService;
        }

        #region ProductList
        [Route("/ProductList")]
        public async Task<IActionResult> ProductList()
        {
            return View(await productService.GetAllProductsAsync());
        }
        #endregion 

        #region CreateProduct
        [Route("/CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewData["GroupId"] = new SelectList(await productGroupservice.GetProductGroupsAsync(), "GroupId", "GroupTitle");
            return View();
        }

        [HttpPost("/CreateProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model, IFormFile ImgUpload, IFormFile[] Gimgupload, string Tags)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GroupId"] = new SelectList(await productGroupservice.GetProductGroupsAsync(), "GroupId", "GroupTitle");
                return View(model);
            }

            var res = await productService.CreateProductAsync(model, ImgUpload, Gimgupload, Tags);
            switch (res)
            {
                case CreateProductResult.Success:
                    TempData["AlertType"] = SwalExtentions.Success;
                    TempData["AlertMessage"] = "Product has been created successfully";
                    return RedirectToAction(nameof(ProductList));


                case CreateProductResult.Failure:
                    TempData["AlertType"] = SwalExtentions.Error;
                    TempData["AlertMessage"] = "Operation faild";
                    break;

                case CreateProductResult.DuplicateTitle:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Duplicate Product Title";
                    break;


                case CreateProductResult.imageformatnotvalid:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Image format is not valid";
                    break;

            }
            ViewData["GroupId"] = new SelectList(await productGroupservice.GetProductGroupsAsync(), "GroupId", "GroupTitle");
            return View(model);
        }

        #endregion

    }
}
