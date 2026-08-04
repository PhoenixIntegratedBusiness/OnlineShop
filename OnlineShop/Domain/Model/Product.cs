using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Product : BaseEntity
    {
        [Key]
        public int ProductId { get; set; }

     
        public  int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public ProductGroup ? ProductGroup { get; set; }

        [Display(Name = "عنوان محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; } = "nophoto.png";

        [Display(Name = "قیمت")]
        public int Price { get; set; }
        [Display(Name = "خلاصه")]
        public string? Summery { get; set; }


        [Display(Name = "توضیحات")]
        public string? Description { get; set; }


        [Display(Name = "کلمات کلیدی")]
        public string? Tags { get; set; }


        public List<ProductGallery> ProductGallery { get; set; }

    }
}
