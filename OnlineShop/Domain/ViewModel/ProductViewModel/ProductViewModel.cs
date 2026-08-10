using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public int GroupId { get; set; }
        public ProductGroup? ProductGroup { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; } 
        public string? Summery { get; set; }
        public string? Description { get; set; }       
        public DateTime CreateDate { get; set; } 
        public bool isDelete { get; set; } = false;
    }
}
