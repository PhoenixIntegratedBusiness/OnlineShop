using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductGroupViewModel
{
    public class CreateProductGroupViewModel
    { 
        [Required(ErrorMessage = "Please enter your {0}")]
        public string GroupTitle { get; set; }
      
    }
}
