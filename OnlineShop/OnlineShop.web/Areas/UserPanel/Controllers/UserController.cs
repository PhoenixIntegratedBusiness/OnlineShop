using Application.Extentions;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.ViewModel.AccountViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OnlineShop.web.Web.Extentions;

namespace OnlineShop.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Route("/Change-Password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("/Change-Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = SwalExtentions.Error;
                TempData["AlertMessage"] = "Operation faild";
                return View(model);
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userid = User.GetUserId();
                    var username=User.GetUsername();
                    if (userid != null)
                    {
                        model.UserId = (int)userid;                      
                        var result = await _userService.ChangePasswordAsync(model, (int)userid);
                        switch (result)
                        {
                            case ChangePassResult.Success:
                                TempData["AlertType"] = SwalExtentions.Success;
                                TempData["AlertMessage"] = $"Dear {username}Your Password has been Changed successfully";
                                return RedirectToAction(nameof(ChangePassword));
                            case ChangePassResult.Failure:
                                TempData["AlertType"] = SwalExtentions.Error;
                                TempData["AlertMessage"] = "Operation faild";
                                return View(model);

                            case ChangePassResult.WrongCurrentPass:
                                TempData["AlertType"] = SwalExtentions.Warning;
                                TempData["AlertMessage"] = "Curront password is not correct";
                                return View(model);
                            case ChangePassResult.NewPassNotMaching:
                                TempData["AlertType"] = SwalExtentions.Warning;
                                TempData["AlertMessage"] = "New password and ReNew password is not matching";
                                return View(model);

                            case ChangePassResult.Unauthorized:
                                TempData["AlertType"] = SwalExtentions.Warning;
                                TempData["AlertMessage"] = "Unauthorized User";
                                return View(model);
                        }
                    }
                }
                return View(model);
            }
        }
    }
}
