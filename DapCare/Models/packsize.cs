using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class packsize
    {
        [Required]
        public int PackId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string PackSizeName { get; set; }
        [Required]
        public int Price { get; set; }
      
       

    }
}