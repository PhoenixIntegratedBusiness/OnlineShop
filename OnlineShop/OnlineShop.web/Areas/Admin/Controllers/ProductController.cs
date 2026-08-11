using Application.Enums.Product;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.ViewModel.ProductViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.web.Web.Extentions;
using System.Security.Cryptography;

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
            return View(await productService.GetAllProductsAsync() ?? new List<ProductViewModel>());
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

        #region EditProduct
        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct(int PId)
        {
            var product = await productService.GetProductByIdAsync(PId);
            ViewData["GroupId"] = new SelectList(await productGroupservice.GetProductGroupsAsync(), "GroupId", "GroupTitle", product.GroupId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost("EditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductviewModel model, IFormFile? ImgUpload, IFormFile[]? Gimgupload, string TagsText)
        {
            if (!ModelState.IsValid) 
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"FIELD: {item.Key} - ERROR: {error.ErrorMessage}");
                    }
                }

                ViewData["GroupId"] = new SelectList(await productGroupservice.GetProductGroupsAsync(), "GroupId", "GroupTitle", model.GroupId);
                return View(model);
            }
            var res = await productService.UpdateProductAsync(model, ImgUpload, Gimgupload, TagsText);
            switch (res)
            {
                case UpdateProductResult.Success:
                    TempData["AlertType"] = SwalExtentions.Success;
                    TempData["AlertMessage"] = "Update Product Operation has been done successfully";
                    return RedirectToAction(nameof(ProductList));
                    
                case UpdateProductResult.Failure:
                    TempData["AlertType"] = SwalExtentions.Error;
                    TempData["AlertMessage"] = "Operation faild";
                    break;

                case UpdateProductResult.DuplicateTitle:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Duplicate Product Title";
                    break;
            }

            ViewData["GroupId"] = new SelectList(await productGroupservice.GetProductGroupsAsync(), "GroupId", "GroupTitle", model.GroupId);
            return View(model);
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> DeleteProductGallary(int id)
        {
            await productService.DeleteGallaryImgByIdAsync(id);

            return Ok();
        }
    }
}
