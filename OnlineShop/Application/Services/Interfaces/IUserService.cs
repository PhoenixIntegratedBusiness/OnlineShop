using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetUsersAsync();
      
        Task<ResultRegister> RegisterUserAsync(RegisterViewModel model);

        Task<LoginResult> LoginUserAsync(LoginViewModel model);

        Task<Users?> GetUserByEmailAsync(string email);
        Task<ForgetPassResult> ForgetPasswordAsync(ForgetPasswordViewModel model);

        Task<ResetPasswordResult> CheckActiveCodeAsync(ResetPasswordViewModel model);


    }

   
}
