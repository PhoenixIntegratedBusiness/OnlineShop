using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.AccountViewModel
{
    public class UserRoleViewModel
    {
        public int RoleId { get; set; }
        public List<string> Roles { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Username { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public bool IsAdmin { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isDelete { get; set; }
    }
}
