using Domain.Interfaces;
using Domain.Model;
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
        public UserRepository( MyContext context)
        {
            _Context = context;     
        }


        public async Task<List<Users>> GetUsersAsync()
        {
           return await _Context.Users.ToListAsync();
        }
    }
}
