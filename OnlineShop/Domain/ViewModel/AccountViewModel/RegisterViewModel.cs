using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your {0}")]
        public required string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email Format is not Correct")]
        [Required(ErrorMessage = "Please enter your {0}")]
        public required string  Email { get; set; }

        [Required(ErrorMessage = "Please enter your {0}")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Please enter your {0}")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public required string RePassword { get; set; }
        [Required(ErrorMessage = "Please enter your {0}")]
        public string Mobile { get; set; }
    }


   
}
