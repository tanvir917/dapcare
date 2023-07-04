using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DapCare.Models
{
    public class DataModel : DbContext
    {
        public DbSet <Cart> cart {

            get;
            set;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().ToTable("tblCart");
        }

    }
}