using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace OnlineShop.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        public HomeController(IProductService productService, IProductGroupService productGroupService)
        {
            _productService = productService;
            _productGroupService = productGroupService;
        }
        public async Task<IActionResult> Index()
        {
            var group = await _productGroupService.GetProductGroupsAsync();
            ViewData["GroupId"] = group.Where(u => u.isDelete == false);
            var product = (await _productService.GetAllProductCardItemAsync()).OrderByDescending(u=>u.ProductId).Take(8);

            return View(product);
        }

    }
}
