using Application.Extentions;
using Application.Services.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineShop.web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region AddToCard
        [Route("AddToCard")]
        //[Authorize]
        public async Task<IActionResult> AddToCard(int id)
        {
            var usermail = User.FindFirstValue(ClaimTypes.Email);
            int? userid = User.GetUserId();
            if (!userid.HasValue)
                return Unauthorized();
            await _orderService.AddtocardAsync(id, userid.Value);
            var headers = Request.GetTypedHeaders();
            var referer = headers.Referer;
            var refererRelativePath = referer?.PathAndQuery;
            return Redirect(refererRelativePath);
        }

        #endregion

        #region CountShopCart
        public async Task<int> CountShopCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var usermail = User.FindFirstValue(ClaimTypes.Email);
                int? userid = User.GetUserId();
                if (!userid.HasValue)
                    return 0;
                return await _orderService.CountShopCardAsync(userid.Value);

            }
            return 0;

        }
        #endregion
    }
}
