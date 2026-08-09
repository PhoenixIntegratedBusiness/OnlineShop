using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductGroupViewModel
{
    public class ProductGroupViewModel
    {
        public int GroupId { get; set; }
        public string? GroupTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isDelete { get; set; }


    }
}
