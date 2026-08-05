using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email.ToLower().Trim());
        }

        #region GetUsersAsync  
        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var list = await _userRepository.GetUsersAsync();

            var users = list.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                Username = u.Username,
                IsAdmin = u.IsAdmin,
                Email = u.Email,
                Mobile = u.Mobile,
                CreateDate = u.CreateDate,
                isDelete = u.isDelete

            }).ToList();

            return users;
        }
        #endregion


        #region LoginUserAsync 
        public async Task<LoginResult> LoginUserAsync(LoginViewModel model)
        {

            var existUser = await _userRepository.GetUserByEmailAsync(model.Email.Trim().ToLower());
            if (existUser == null || existUser.isDelete)
            {
                return LoginResult.UserNotFound;
            }
            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(existUser, existUser.Password, model.Password);

            switch (result)
            {
                case PasswordVerificationResult.Success:
                    return LoginResult.Success;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    existUser.Password =passwordHasher.HashPassword(existUser, model.Password);
                    await _userRepository.SaveChangeAsync();
                    return LoginResult.Success;

                case PasswordVerificationResult.Failed:
                    return LoginResult.Failure;

                default:
                    return LoginResult.Failure;
            }
        }
        #endregion



        #region RegisterUserAsync
        public async Task<ResultRegister> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                var existUser = await _userRepository.GetUserByEmailAsync(model.Email.Trim().ToLower());

                if (existUser != null)
                {
                    return ResultRegister.EmailExists;
                }

                var user = new Users
                {
                    Email = model.Email.Trim().ToLower(),
                    Username = model.UserName,
                    Mobile = model.Mobile,
                    CreateDate = DateTime.Now,
                    isDelete = false,
                    IsAdmin = false
                };

                var passwordHasher = new PasswordHasher<Users>();
                user.Password = passwordHasher.HashPassword(user, model.Password);

                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangeAsync();

                return ResultRegister.Success;
            }
            catch 
            {
                return ResultRegister.Failed;
            }
        }
        #endregion
    }
}
