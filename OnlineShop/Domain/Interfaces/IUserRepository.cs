using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Users>> GetUsersAsync();
        Task<Users?> GetUserByEmailAsync(string email);
        Task AddUserAsync(Users user);
        Task SaveChangeAsync();
        void UserUpdate(Users user);
        Task<Users?> CheckActiveCodeAsync(string email,string activecode);
        Task<Users?> GetUserByIdAsync(int id);
        Task<List<Users>> GetUsersWithRoleAsync();
        Task<List<Role>> GetRoles();
        Task<Users?> IsUserExistAsync(string username,string email,string Mobile);
        Task CreateUserRoleAsync(Users users);
        Task <Users?> GetUserByIdlAsync(int userId);
        void UpdateUserRoleAsync(Users users);
        Task<Users> GetRoleUserAsync(int userid);
        Task<Users?> IsUserExistanceAsync( string username,string email,string mobile,int userId);

    }
}
