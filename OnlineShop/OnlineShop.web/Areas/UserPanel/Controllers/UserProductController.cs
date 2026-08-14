using Application.Extentions;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize(Roles = "Admin,User")]
    public class UserProductController : Controller
    {
        private readonly IOrderService _orderService;
        public UserProductController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region ShowShopCart
        [Route("ShowShopCart")]
        public async Task<IActionResult> ShowShopCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                int? userid = User.GetUserId();
                return View(await _orderService.GetCartItemsAsync(userid.Value));
            }

            return View();
        }
        #endregion


        #region DeleteFromBasketAsync
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                int? userid = User.GetUserId();
                await _orderService.DeleteFromCartAsync(userid.Value,id);
                return Ok();
            }

                return NotFound();
        }
        #endregion
    }
}
