using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductViewModel
{
    public class ProductCardViewModel
    {
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string? Summery { get; set; }
        public int Price { get; set; }

    }
}
