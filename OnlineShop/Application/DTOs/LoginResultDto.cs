using Application.Enums.Account;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class LoginResultDto
    {
        public LoginResult Result { get; set; }    //Enum
        public Users? User { get; set; }
    }
}
//User+(Enum)loginresult