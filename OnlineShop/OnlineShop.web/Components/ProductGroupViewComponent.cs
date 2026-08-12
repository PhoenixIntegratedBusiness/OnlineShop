using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.web.Components
{
    public class ProductGroupViewComponent:ViewComponent
    {
        private readonly IProductGroupService _productGroupService;
        public ProductGroupViewComponent(IProductGroupService productGroupService)
        {
            _productGroupService= productGroupService;
        }
         

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var res= await _productGroupService.GetProductGroupsAsync();
            return View(res.Where(u=>u.isDelete!=true));
        }

    }
}
