using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class item
    {
        public Products product { get; set; }
        public int Quantity { get; set; }
        public DataTable ProductCartTable { get; set; }
        public int CartNumber { get; set; }
        public int TotalCart { get; set; }
        public int pid { get; set; }
    }
}