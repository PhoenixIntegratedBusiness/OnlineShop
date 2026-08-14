using Application.Extentions;
using Application.Services.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineShop.web.Components
{
    public class ShopCartViewComponent:ViewComponent
    {
        private readonly IOrderService _orderService;
        public ShopCartViewComponent( IOrderService orderService)
        {
            _orderService= orderService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usermail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            int? userid = HttpContext.User.GetUserId();
            if (!userid.HasValue)
            {
                return View(new Order());
            }
            return View(await _orderService.GetCartItemsAsync(userid.Value));
        }


    }
}
