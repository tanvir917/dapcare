using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class SelectProducts
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DataTable ProductTable { get; set; }
        public int ProductPrice { get; set; }
        public string ProductName { get; set; }
        public int Discount { get; set; }
    }
}