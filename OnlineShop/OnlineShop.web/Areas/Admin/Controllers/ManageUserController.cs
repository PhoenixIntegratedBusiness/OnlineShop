using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ManageUserController : Controller
    {
        private readonly IUserService _userService;
        public ManageUserController(IUserService userService)
        {
            this._userService = userService;
        }

        #region UserList
        [Route("/User-List")]
        public async Task<IActionResult> UserList()
        {
            var list = await _userService.GetUsersAsync();
            return View(list);
        }
        #endregion



    }
}
