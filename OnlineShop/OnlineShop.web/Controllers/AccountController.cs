using Application.DTOs;
using Application.Enums.Account;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OnlineShop.web.Web.Extentions;
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
                        //return RedirectToAction("UserList", "User", new { area = "UserPanel" });
                        TempData["AlertType"] = SwalExtentions.Success;
                        TempData["AlertMessage"] = "Your account has been created successfully. Please log in. ";
                        return RedirectToAction(nameof(Login));


                    case ResultRegister.Failed:
                        TempData["AlertType"] = SwalExtentions.Error;
                        TempData["AlertMessage"] = "Operation faild";
                        return View(model);

                    case ResultRegister.EmailExists:
                        //ModelState.AddModelError("Email", "Email is duplicate");
                        TempData["AlertType"] = SwalExtentions.Error;
                        TempData["AlertMessage"] = "Email already exists.";
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
            var res = await userService.LoginUserAsync(model);

            switch (res.Result)
            {
                case LoginResult.Success:
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,res.User.UserId.ToString()),
                        new Claim(ClaimTypes.Name,res.User.Username),
                        new Claim(ClaimTypes.Email,res.User.Email),
                        new Claim("IsAdmin",res.User.IsAdmin.ToString()),


                    };
                    foreach (var userRole in res.User.userInRoles)
                    {
                        claims.Add(new Claim(
                            ClaimTypes.Role,
                            userRole.Role.RoleName
                        ));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe,
                    };
                    await HttpContext.SignInAsync(claimsPrincipal, properties);
                    TempData["AlertType"] = SwalExtentions.Success;
                    TempData["AlertMessage"] = $"Welcome back Dear {res.User.Username}! You have successfully logged in.";
                    return Redirect("/");

                case LoginResult.UserNotFound:
                    ModelState.AddModelError("Email", "User Not Found");
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "User Not Found";
                    return View(model);

                case LoginResult.Failure:
                    TempData["AlertType"] = SwalExtentions.Error;
                    TempData["AlertMessage"] = "User Not Found";
                    return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region LogOut
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["AlertType"] = SwalExtentions.Warning;
            TempData["AlertMessage"] = "Logged Out Successfully!Thank you for visiting Online Shop. See you again!";
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
                        TempData["AlertType"] = SwalExtentions.Warning;
                        TempData["AlertMessage"] = "User Not Found";
                        ModelState.AddModelError(nameof(ForgetPasswordViewModel.Email), "Email Not Found!");
                        break;
                }
                return View(model);
            }
        }
        #endregion

        #region ResetPassword

        [Route("Reset-Password")]
        public IActionResult ResetPassword()
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
                var result = await userService.CheckActiveCodeAsync(model);
                switch (result)
                {
                    case ResetPasswordResult.Success:
                        TempData["AlertType"] = SwalExtentions.Success;
                        TempData["AlertMessage"] = "Your password has been reset successfully. Please log in.";
                        return RedirectToAction(nameof(Login));
                    case ResetPasswordResult.Failure:
                        TempData["AlertType"] = SwalExtentions.Error;
                        TempData["AlertMessage"] = "Something went wrong. Please try again.";
                        return RedirectToAction(nameof(Login));
                }

            }
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion


    }

}
