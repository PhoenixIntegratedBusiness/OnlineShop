using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> ShowProductbyGroup(int id, string title)
        {
            var group = await _productGroupService.GetProductGroupsAsync();
            ViewData["GroupId"] = group.Where(u => u.isDelete == false);
            if (id == -1)
            {
                ViewData["Productlist"] = await _productService.GetAllProductCardItemAsync();
            }
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

      
    }
}