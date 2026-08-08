using Application.Enums.Account;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Enums.Account
{
    public enum EditUserRoleResult
    {
        Success, Fauiler, DuplicateEmail, DuplicateUsername, DuplicateMobile
    }
}


