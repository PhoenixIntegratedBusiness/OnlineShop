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

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


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
                CreateDate=u.CreateDate,
                isDelete=u.isDelete

            }).ToList();

            return users;
        }
    }
}
