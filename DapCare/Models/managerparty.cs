using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class managerparty
    {
        public int EmployeId { get; set; }
        public int PartyId { get; set; }
        public string PartName { get; set; }
        public string Cash { get; set; }
        public DateTime dates { get; set; }
        public int DipId { get; set; }
    }
}