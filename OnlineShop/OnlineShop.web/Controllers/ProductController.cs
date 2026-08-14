using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineShop.web.Web.Extentions;

namespace OnlineShop.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        public ProductController(IProductService productService, IProductGroupService productGroupService)
        {
            _productService = productService;
            _productGroupService = productGroupService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region GetProById
        public async Task<IActionResult> GetProById(int id)
        {
            var res = (await _productService.GetProductsByGruoupIdAsync(id)).OrderByDescending(u => u.ProductId).Take(8);
            return PartialView("_ProductList", res);
        }
        #endregion



        #region ShowProductbyGroup
        [Route("Group/{id}/{title}")]
        public async Task<IActionResult> ShowProductbyGroup(int id,string title,int pageid = 1)
        {
            int take = 6;
            int skip = (pageid - 1) * take;

            var group = await _productGroupService.GetProductGroupsAsync();
            ViewData["GroupId"] = group.Where(u => u.isDelete == false);

            // All Products
            if (id == -1)
            {
                var products = await _productService.GetAllProductCardItemAsync();
                int pageCount = (int)Math.Ceiling(products.Count() / (double)take);

                ViewBag.PageCount = pageCount;
                ViewBag.PageId = pageid;

                var productList = products.Skip(skip).Take(take).ToList();
                return View(productList);
            }

            // Products by Group
            var res = await _productService.GetProductsByGruoupIdAsync(id);

            return View(res);
        }

        #endregion




        #region ProductDetails
        [Route("ProductDetails/{id}")]
        public async Task<IActionResult> ProductDetails(int id)
        {
            return View(await _productService.GetProductByIdAsync(id));
        }
        #endregion


        #region SearchProductKeyAsync
        //[Route("SearchProductKey /{q}")]
        public async Task<IActionResult> SearchProductKey(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                TempData["AlertType"] = SwalExtentions.Warning;
                TempData["AlertMessage"] = "Please enter a keyword.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _productService.SearchProductKeyAsync(q);
            if (!result.Any())
            {
                TempData["AlertType"] = SwalExtentions.Warning;
                TempData["AlertMessage"] = "No product was found for your keyword.";
                return RedirectToAction("Index", "Home");
            }

            return View(result);
        }
        #endregion

    }
}