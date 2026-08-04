using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.ViewModel.AccountViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace OnlineShop.web.Controllers
{
    public class AccountController
        (IUserService userService) : Controller
    {

        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.RegisterUserAsync(model);


                switch (res)
                {
                    case ResultRegister.Success:
                        return RedirectToAction("UserList", "User", new { area = "UserPanel" });
                        
                    case ResultRegister.Failed:
                        return View(model);
                       
                    case ResultRegister.EmailExists:
                        ModelState.AddModelError("Email", "Email is duplicate");
                        return View(model);
                    default:
                        break;
                }
            }
            return View(model);
        }

        #endregion
    }
}
