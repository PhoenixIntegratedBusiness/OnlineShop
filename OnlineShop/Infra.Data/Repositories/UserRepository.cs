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

        #region GetUserByEmailAsync
        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.Email == email && u.isDelete == false);

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
            return await _Context.Users.FirstOrDefaultAsync(u=>u.ActiveCode== activecode && u.Email== email && u.isDelete==false);
        }
        #endregion

    }
}
