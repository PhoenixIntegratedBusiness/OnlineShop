using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Order:BaseEntity
    {
        public int UserId { get; set; }
       
        public int OrderId { get; set; }
        public bool IsFinaly { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
