using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.AccountViewModel
{
    public class EditUserRoleViewModel
    {
        public List<int> SelectedRoles { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }

        public string Mobile { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ActiveCode { get; set; }
        public bool isDelete { get; set; }


    }
}
