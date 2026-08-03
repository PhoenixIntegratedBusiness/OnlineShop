using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class BaseEntity
    {
      
       
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "حذف شده ؟")]
        public bool isDelete { get; set; } = false;
    }
}
