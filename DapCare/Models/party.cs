using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class party
    {   public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyPhoneNumber { get; set; }

        public string PartyAddress { get; set; }

        public int EmployeId { get; set; }
        public string EmployeName { get; set; }
        public string PartyDuplicate { get; set; }



    }
}