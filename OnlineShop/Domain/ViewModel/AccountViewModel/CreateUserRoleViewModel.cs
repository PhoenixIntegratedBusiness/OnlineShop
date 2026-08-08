using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.AccountViewModel
{
    public class CreateUserRoleViewModel
    {
        /// <summary>
        ///SelectedRoles and Roles are used when an admin creates a new user.
        /// The Roles list contains all available roles that are displayed to the admin.
        /// The admin can select one or multiple roles for the user.
        /// 
        /// If a user could have only one role, we would use a single RoleId property
        /// instead of SelectedRoles. However, since a user can have multiple roles,
        /// SelectedRoles stores the selected role IDs that will be saved in the UserRoles table.
        /// 
        /// Roles contains the role information, including the role names displayed in the UI.
        /// </summary>
        /// 


        public List<int> SelectedRoles { get; set; } = new();
        public List<Role> Roles { get; set; } = new();


        [Required(ErrorMessage = "Please enter {0}")]
        public string Username { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter {0}")]
        [EmailAddress(ErrorMessage = "Email Format is not Correct")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter {0}")]
        public string Mobile { get; set; }

        public bool IsAdmin { get; set; }


    }
}
