using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.AccountViewModel
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Email Format is not Correct")]
        [Required(ErrorMessage = "Please enter your {0}")]
        public required string Email { get; set; }
    }
}
