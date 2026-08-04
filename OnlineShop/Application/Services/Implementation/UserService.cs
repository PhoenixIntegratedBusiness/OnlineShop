using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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



        #region RegisterUserAsync
        public async Task<ResultRegister> RegisterUserAsync(RegisterViewModel model)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(model.Email);

            if (existUser != null)
            {
                return ResultRegister.EmailExists;
            }

            var user = new Users
            {
                Email = model.Email,
                Username = model.UserName,
                Mobile = model.Mobile,
                CreateDate = DateTime.Now,
                isDelete = false,
                IsAdmin = false
            };

            var passwordHasher = new PasswordHasher<Users>();
            user.Password =passwordHasher.HashPassword(user, model.Password);

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangeAsync();

            return ResultRegister.Success;
        }
        #endregion
    }
}
