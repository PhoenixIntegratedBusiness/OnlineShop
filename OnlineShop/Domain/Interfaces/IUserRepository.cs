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
    }
}
