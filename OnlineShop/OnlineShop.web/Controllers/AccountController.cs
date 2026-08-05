using Application.Extentions;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.ViewModel.AccountViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Claims;

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

        #region Login

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userService.GetUserByEmailAsync(model.Email);
            var res = await userService.LoginUserAsync(model);
            switch (res)
            {
                case LoginResult.Success:
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("IsAdmin",user.IsAdmin.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe,
                    };
                    await HttpContext.SignInAsync(claimsPrincipal, properties);
                    return Redirect("/");

                case LoginResult.UserNotFound:
                    ModelState.AddModelError("Email", "User Not Found");
                    return View(model);

                case LoginResult.Failure:
                    ModelState.AddModelError("Email", "wrong data entry");
                    return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region LogOut
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region Forgot password
        [Route("Forget-Password")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await userService.ForgetPasswordAsync(model);
                switch (result)
                {
                    case ForgetPassResult.Success:
                        return RedirectToAction(nameof(ResetPassword));
                    case ForgetPassResult.Failure:
                        ModelState.AddModelError(nameof(ForgetPasswordViewModel.Email), "Email Not Found!");
                        break;
                }
                return View(model);
            }
        }
        #endregion


        #region ResetPassword

        [Route("Reset-Password")]
        public  IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                var result= await userService.CheckActiveCodeAsync(model);
                switch (result) {
                    case ResetPasswordResult.Success:
                        return RedirectToAction(nameof(Login));
                        case ResetPasswordResult.Failure:
                        return RedirectToAction(nameof(Login));
                }

            }
            return RedirectToAction(nameof(Login));
        }

        #endregion
    }



}
