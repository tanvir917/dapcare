using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public int Discount { get; set; }
        public int Price { get; set; }
    }
}