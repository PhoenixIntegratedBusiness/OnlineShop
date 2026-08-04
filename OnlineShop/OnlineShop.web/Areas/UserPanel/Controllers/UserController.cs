using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        public async Task<IActionResult> UserList()
        {
            var list = await _userService.GetUsersAsync();
            return View(list);
        }


    }
}
