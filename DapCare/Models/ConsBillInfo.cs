using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class ConsBillInfo
    {
        public int ConsBillId { get; set; }
        public string EmployeName { get; set; }
        public string EmployePhoneNumber { get; set; }
        public DateTime SetDate { get; set; }
    }
}