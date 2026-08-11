using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductViewModel
{
    public class DeleteProductViewModel
    {
        public int GroupId { get; set; }
        public int ProductId { get; set; }
        public ProductGroup? ProductGroup { get; set; }


        public string Title { get; set; }
        public string? ImageName { get; set; } = "nophoto.png";

        public int Price { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Summery { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.MultilineText)]
        public List<Tags> Tags { get; set; } = new();
        public List<ProductGallery>? ProductGallery { get; set; } = new();
    }
}
