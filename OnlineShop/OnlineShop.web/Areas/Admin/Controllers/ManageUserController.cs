using Application.Enums.Account;
using Application.Services.Interfaces;
using Domain.ViewModel.AccountViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.web.Web.Extentions;
using System.Threading.Tasks;

namespace OnlineShop.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
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
            var list = await _userService.GetUsersWithRoleAsync();
            return View(list);
        }
        #endregion


        #region CreateUserWith Role

        [Route("/CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            var model = new CreateUserRoleViewModel();
            var listrole = await _userService.GetRoles();
            model.Roles = listrole;
            return View(model);
        }


        [HttpPost("/CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //foreach (var item in ModelState)
                //{
                //    foreach (var error in item.Value.Errors)
                //    {
                //        Console.WriteLine($"{item.Key} : {error.ErrorMessage}");
                //    }
                //}

                model.Roles = await _userService.GetRoles();
                return View(model);
            }
            var res = await _userService.CreateUserRoleAsync(model);

            switch (res)
            {
                case CreateUserRoleResult.DuplicateEmail:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Duplicate Email";                   
                    break;

                case CreateUserRoleResult.DuplicateMobile:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Duplicate Mobile";                   
                    break;

                case CreateUserRoleResult.DuplicateUsername:
                    TempData["AlertType"] = SwalExtentions.Warning;
                    TempData["AlertMessage"] = "Duplicate Username";
                    break;

                case CreateUserRoleResult.Fauiler:
                    TempData["AlertType"] = SwalExtentions.Error;
                    TempData["AlertMessage"] = "Operation faild";
                    break;

                case CreateUserRoleResult.Success:
                    TempData["AlertType"] = SwalExtentions.Success;
                    TempData["AlertMessage"] = "Operation has been done successfuly";
                    return RedirectToAction(nameof(UserList));
            }

            model.Roles = await _userService.GetRoles();
            return View(model);
        }
        #endregion
    }
}
