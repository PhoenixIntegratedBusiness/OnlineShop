using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Slider : BaseEntity
    {
        [Key]
        public int SliderId { get; set; }

        [Display(Name = "عنوان ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "تصویر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }
        [Display(Name = "شروع ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateOnly StartDate { get; set; }
        [Display(Name = " پایان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateOnly EndDate { get; set; }
    }
}
