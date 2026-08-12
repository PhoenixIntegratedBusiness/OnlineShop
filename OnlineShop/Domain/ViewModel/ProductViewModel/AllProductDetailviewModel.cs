using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductViewModel
{
    public class AllProductDetailviewModel
    {       
            public int ProductId { get; set; }
            public int GroupId { get; set; }

            [Required(ErrorMessage = "Please enter your {0}")]
            public string Title { get; set; }
            public string? ImageName { get; set; } = "nophoto.png";

            [Required(ErrorMessage = "Please enter your {0}")]
            public int Price { get; set; }

            [DataType(DataType.MultilineText)]
            public string? Summery { get; set; }
            [DataType(DataType.MultilineText)]
            public string? Description { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime CreateDate { get; set; }
            public bool isDelete { get; set; } = false;

            [DataType(DataType.MultilineText)]
            public List<Tags?> Tags { get; set; } = new();
            public List<ProductGallery>? ProductGallery { get; set; } = new();
        }
    }

