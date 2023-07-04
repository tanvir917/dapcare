using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DapCare.Models
{
    public class Products
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public SelectList Categoried { get; set; }
        [Required]
        public decimal ProductPricePerUnit { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Discount { get; set; }
        public string Tag { get; set; }

    }
}