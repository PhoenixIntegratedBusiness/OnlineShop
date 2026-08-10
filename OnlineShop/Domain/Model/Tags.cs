using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Tags: BaseEntity
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }

}
