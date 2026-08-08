using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyContext _Context;
        public UserRepository(MyContext context)
        {
            _Context = context;
        }

        #region IsUserExistAsync
        public async Task<Users?> IsUserExistAsync(string username, string email, string Mobile)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email || u.Mobile==Mobile);
        }
        #endregion

        #region CreateUserRoleAsync
        public async Task CreateUserRoleAsync(Users users)
        {
            await _Context.Users.AddAsync(users);
            //foreach (var item in users.userInRoles)
            //{
            //   await _Context.userInRoles.AddAsync(item);
            //}
        }
        #endregion

        #region   
        public async Task<List<Role>> GetRoles()
        {
            return await _Context.Roles.ToListAsync();
        }
        #endregion

        #region  GetUsersWithRoleAsync
        public async Task<List<Users>> GetUsersWithRoleAsync()
        {
            return await _Context.Users.Include(r => r.userInRoles).ThenInclude(u => u.Role).ToListAsync();
        }
        #endregion

        #region GetUserByIdAsync
        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }
        #endregion

        #region GetUserByEmailAsync
        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _Context.Users.Include(ur => ur.userInRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(t => t.Email == email && t.isDelete == false);

        }
        #endregion

        #region  GetUsersAsync
        public async Task<List<Users>> GetUsersAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        #endregion

        #region AddUserAsync
        public async Task AddUserAsync(Users user)
        {
            await _Context.Users.AddAsync(user);
        }
        #endregion

        #region SaveChangeAsync
        public async Task SaveChangeAsync()
        {
            await _Context.SaveChangesAsync();
        }
        #endregion

        #region UserUpdate
        public void UserUpdate(Users user)
        {
            _Context.Users.Update(user);
        }
        #endregion

        #region CheckActiveCodeAsync
        public async Task<Users?> CheckActiveCodeAsync(string email, string activecode)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.ActiveCode == activecode && u.Email == email && u.isDelete == false);
        }

        #endregion

    }
}
