using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Slider : BaseEntity
    {
        [Key]
        public int SliderId { get; set; }

        public string Title { get; set; }

      
        public string? ImageName { get; set; }

        [DataType(DataType.DateTime)]
        public DateOnly? StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateOnly? EndDate { get; set; }
    }
}
