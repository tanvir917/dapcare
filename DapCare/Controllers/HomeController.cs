using DapCare.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DapCare.Controllers
{
    public class HomeController : Controller
    {    //Connect TO DataBase Named DapeCare

        public ActionResult Index() {

            return View();
        }
        public ActionResult login(string username, string pass)
        {


            if (username == "rahat"  && pass == "12345")
            {
             
                    return Redirect("~/Admin/ViewSelectedProducts");

                
            }
            else {
                return Redirect("~/Home/ViewSelectedProducts");

            }
        }
    }

    
}