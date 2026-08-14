using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.OrderViewModel
{
    public class OrderProductByIdViewModel
    {

        public int ProductId { get; set; }

        public string Title { get; set; }
        public string ImageName { get; set; } = "nophoto.png";
        public int Price { get; set; }

    }
}
