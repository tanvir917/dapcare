using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class ProductPack
    {
        public int ProductId { get; set; }
        [Required]
        public int PackSizeId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public int Bonus { get; set; }
    }
}