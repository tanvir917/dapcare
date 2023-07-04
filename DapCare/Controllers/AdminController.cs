using DapCare.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DapCare.Controllers
{
    public class AdminController : Controller
    {
        string connectionString = @"Data Source =(local); Database=db;Trusted_Connection=false;Initial Catalog =DapCare; password=Rahatdapagrocare2020;User Id=dapuser2; Integrated Security  = false;MultipleActiveResultSets=true";
        //string connectionString = @"Data Source = DESKTOP-2V8523P\SQLEXPRESS;  Initial Catalog =DapCare;  Integrated Security  = true";
        //103.125.254.20,9433

        //Create Product
        [HttpGet]
        public ActionResult CreateProduct()
        {
            return View(new Products());
        }
        [HttpPost]
        public ActionResult CreateProduct(Products Products, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Productimg/"), pic);
                file.SaveAs(path);
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Product  values(@ProductName,@ProductTypeId,@ProductDescription,@ProductPrice,@ProductImage,@Discount,@Tag)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductName", Products.ProductName);
                sqlCmd.Parameters.AddWithValue("@ProductTypeId", Products.ProductTypeId);
                sqlCmd.Parameters.AddWithValue("@ProductDescription", Products.Description);
                sqlCmd.Parameters.AddWithValue("@ProductPrice", Products.ProductPricePerUnit);
                sqlCmd.Parameters.AddWithValue("@ProductImage", pic);
                sqlCmd.Parameters.AddWithValue("@Discount", Products.Discount);
                sqlCmd.Parameters.AddWithValue("@Tag", Products.Tag);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");
        }


        [HttpGet]
        public ActionResult MyProducts()
        {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

        public ActionResult MyPackSize()
        {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Packsize", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

        [HttpGet]
        public ActionResult Edit(int ProductId)
        {
            Nproduct productlist = new Nproduct();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Nproduct  where Nproductid = @Nproductid";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Nproductid", ProductId);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productlist.ProdductId = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productlist.Name = (dtblProduct.Rows[0][1].ToString());
                productlist.Description = dtblProduct.Rows[0][2].ToString();
                productlist.image = dtblProduct.Rows[0][3].ToString();
                return View(productlist);
            }
            else
            {
                return RedirectToAction("MyProducts");
            }

        }

        [HttpPost]

        public ActionResult Edit(Nproduct productlist, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Productimg/"), pic);
                file.SaveAs(path);
                productlist.image = pic;
            }
            else
            {
                pic = productlist.image;
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Nproduct SET ProductName=@ProductName,Descriptions=@Descriptions,ProductImage=@ProductImage  where Nproductid=@Nproductid";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductName", productlist.Name.ToString());
                sqlCmd.Parameters.AddWithValue("@Descriptions", productlist.Description.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductImage", productlist.image.ToString());
                sqlCmd.Parameters.AddWithValue("@Nproductid", productlist.ProdductId);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");
        }

        //Deposit
        public ActionResult Deposit()
        {
            List<Employe> employe = GetEmployeList();
            ViewBag.employelist = new SelectList(employe, "EmployeId", "EmployeFirstName");
            return View();
        }

        public JsonResult InsertDeposit(string Deposit, string PartyId, string EmployeId)
        {
            DataTable dtbl = new DataTable();
            int Pid = Convert.ToInt32(PartyId);
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Party  where PartyId = @PartyId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PartyId", Pid);
                sqlDa.Fill(dtbl);
            }
            return Json(new
            {
                resut = " OK  got it"
            });

        }









        public ActionResult Deletestocks(int id)
        {

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Delete  from Stock where StockId=@StockId";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@StockId", id);
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("ViewStock");
        }


        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Delete  from Nproduct where Nproductid=@Nproductid";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@Nproductid", id);
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");
        }

        public ActionResult DeleteMyPack(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Delete  from Packsize where PackId=@PackId";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@PackId", id);
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("MyPackSize");
        }


        [HttpGet]

        public ActionResult EditPackSize(int PackId)
        {
            ViewBag.GetAllProducts = GetAllProducts();
            packsize productlist = new packsize();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Packsize  where PackId = @PackId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PackId", PackId);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productlist.PackId = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productlist.ProductId = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                productlist.PackSizeName = (dtblProduct.Rows[0][2].ToString());

                productlist.Price = Convert.ToInt32(dtblProduct.Rows[0][3].ToString());


                return View(productlist);
            }
            else
            {
                return RedirectToAction("MyProducts");
            }

        }

        [HttpPost]
        public ActionResult EditPackSize(packsize packsize)
        {
            DataTable dtblPackSize = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct  where  Nproductid=@Nproductid", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Nproductid", packsize.ProductId);
                sqlDa.Fill(dtblPackSize);
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Packsize SET ProductId=@ProductId,PackSizeName=@PackSizeName,Price=@Price,ProductName=@ProductName  where PackId=@PackId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@PackId", packsize.PackId);

                sqlCmd.Parameters.AddWithValue("@ProductId", packsize.ProductId);

                sqlCmd.Parameters.AddWithValue("@PackSizeName", packsize.PackSizeName.ToString());
                sqlCmd.Parameters.AddWithValue("@Price", packsize.Price);
                sqlCmd.Parameters.AddWithValue("@ProductName", dtblPackSize.Rows[0][1].ToString());
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");

        }

        public ActionResult ViewAllPack()
        {


            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Packsize", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);

        }


        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View(new Employe());
        }


        [HttpPost]
        public ActionResult AddEmployee(Employe Employe, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Productimg/"), pic);
                file.SaveAs(path);
            }
            else
            {
                pic = "No Image supplied";
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Employe  values(@EmployeFirstName,@EMployeLastName,@PhoneNumber,@EMployeImage,@Area)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@EmployeFirstName", Employe.EmployeFirstName);
                sqlCmd.Parameters.AddWithValue("@EMployeLastName", Employe.EmployeLastName);
                sqlCmd.Parameters.AddWithValue("@PhoneNumber", Employe.phonenumber);
                sqlCmd.Parameters.AddWithValue("@EMployeImage", pic);
                sqlCmd.Parameters.AddWithValue("@Area", Employe.Area);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("EmployeRecord");
        }



        [HttpGet]
        public ActionResult EmployeRecord()
        {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Employe", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

        public ActionResult ShowDeposit()
        {
            DataTable dtblDeposit = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Deposit ";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.Fill(dtblDeposit);
            }


            return View(dtblDeposit);
        }

        [HttpGet]
        public ActionResult EditEmploye(int EmployeId)
        {
            Employe Employe = new Employe();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Employe  where EmployeId = @EmployeId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeId);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                Employe.EmployeId = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                Employe.EmployeFirstName = dtblProduct.Rows[0][1].ToString();
                Employe.EmployeLastName = dtblProduct.Rows[0][2].ToString();
                Employe.EmploeImage = dtblProduct.Rows[0][3].ToString();
                Employe.phonenumber = dtblProduct.Rows[0][4].ToString();
                Employe.Area = dtblProduct.Rows[0][5].ToString();
                return View(Employe);
            }
            else
            {
                return RedirectToAction("EmployeRecord");
            }

        }


        [HttpGet]
        public ActionResult EditDeposit(int id)
        {
            managerparty mgpty = new managerparty();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Deposit  where DipId = @DipId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@DipId", id);
                sqlDa.Fill(dtblProduct);
            }
            Session["DipId"] = id;
            int emplid = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
            int partyid = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
            DataTable employeName = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from  Employe  where EmployeId=@EmployeId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", emplid);
                sqlda.Fill(employeName);
            }
            DataTable PartyTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from  Party  where PartyId=@PartyId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(PartyTable);
            }
            int empid = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
            ViewBag.getAllEMployee = GetMyEmployeeNameById(emplid);
            ViewBag.getAllPartyy = GetMyParty(empid);

            if (dtblProduct.Rows.Count == 1)
            {
                mgpty.EmployeId = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
                mgpty.PartyId = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
                mgpty.dates = Convert.ToDateTime(dtblProduct.Rows[0][4]);
                mgpty.Cash = dtblProduct.Rows[0][3].ToString();
                return View(mgpty);
            }
            else
            {
                return RedirectToAction("EmployeRecord");
            }

        }



        [HttpPost]
        public ActionResult EditDeposit(managerparty mgprty)
        {
            int Dipid = ((int)Session["DipId"]);
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Deposit  where DipId = @DipId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@DipId", Dipid);
                sqlDa.Fill(dtblProduct);
            }

            int emplid = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
            int partyid = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
            DataTable employeName = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from  Employe  where EmployeId=@EmployeId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", emplid);
                sqlda.Fill(employeName);
            }
            DataTable PartyTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from  Party  where PartyId=@PartyId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(PartyTable);
            }

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Deposit SET PartyId=@PartyId,EmployeId=@EmployeId,Cash=@Cash,Dates=@Dates,PartyEmployeName=@PartyEmployeName,EmployeEmployeName=@EmployeEmployeName where DipId=@DipId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@PartyId", mgprty.PartyId.ToString());
                sqlCmd.Parameters.AddWithValue("@EmployeId", mgprty.EmployeId.ToString());
                sqlCmd.Parameters.AddWithValue("@Cash", mgprty.Cash.ToString());
                sqlCmd.Parameters.AddWithValue("@Dates", mgprty.dates.ToString());
                sqlCmd.Parameters.AddWithValue("@Dipid", Dipid);
                sqlCmd.Parameters.AddWithValue("@PartyEmployeName", PartyTable.Rows[0][1].ToString());
                sqlCmd.Parameters.AddWithValue("@EmployeEmployeName", employeName.Rows[0][5].ToString());
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("EmployeRecord");
        }


        [HttpPost]
        public ActionResult EditEmploye(Employe employe, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Productimg/"), pic);
                file.SaveAs(path);
                employe.EmploeImage = pic;
            }
            else
            {
                pic = employe.EmploeImage;
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Employe SET EmployeFirstName=@EmployeFirstName,EMployeLastName=@EMployeLastName,PhoneNumber=@PhoneNumber,EMployeImage=@EMployeImage,Area=@Area    where EmployeId=@EmployeId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@EmployeFirstName", employe.EmployeFirstName.ToString());
                sqlCmd.Parameters.AddWithValue("@EMployeLastName", employe.EmployeLastName.ToString());
                sqlCmd.Parameters.AddWithValue("@PhoneNumber", employe.phonenumber.ToString());
                sqlCmd.Parameters.AddWithValue("@EMployeImage", employe.EmploeImage.ToString());
                sqlCmd.Parameters.AddWithValue("@Area", employe.Area.ToString());
                sqlCmd.Parameters.AddWithValue("@EmployeId", employe.EmployeId);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("EmployeRecord");
        }





        public ActionResult Showdeletepirtyfirst()
        {

            return View();
        }



        public ActionResult DeleteEmploye(int id)
        {
            DataTable dtblemp = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Party  where EmployeId = @EmployeId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", id);
                sqlDa.Fill(dtblemp);
            }

            if (dtblemp.Rows.Count == 0)
            {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    string query = $"Delete  from Employe where EmployeId=@EmployeId";
                    SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                    sqldlt.Parameters.AddWithValue("@EmployeId", id);
                    sqldlt.ExecuteNonQuery();
                }

            }
            else
            {

                return RedirectToAction("Showdeletepirtyfirst");
            }

            return RedirectToAction("EmployeRecord");
        }


        public ActionResult SelectProduct()
        {
            DataTable dtblselectProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from product ";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.Fill(dtblselectProduct);
            }

            ViewBag.getProduct = GetProductName();
            return View(dtblselectProduct);
        }


        public List<SelectListItem> GetProductName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("select * from Product ", sqlcon);
                sqlDa.Fill(dtbl);
                foreach (DataRow item in dtbl.Rows)
                {
                    list.Add(new SelectListItem { Value = item["ProductId"].ToString(), Text = item["ProductName"].ToString() });
                }
            }
            return list;
        }


        public int GetCartNumber()
        {
            int consnum = 1;
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select TracNumber from CartTracs where id=@id";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@id", consnum);
                sqlda.Fill(dtbl);
            }
            int cartnumber = Convert.ToInt32(dtbl.Rows[0][0]);
            return cartnumber;
        }
        public int GetMyCartNumber()
        {
            int cons = 1;
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                string query = "Select TracNumber from CartTracs where Id=@Id";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@Id", cons);
                sqlda.Fill(dtbl);
            }
            int foundTracnumber = Convert.ToInt32(dtbl.Rows[0][0]); ;
            if (Session["MyCartNumber"] == null)
            {
                int inccartnumber = foundTracnumber + 1;
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    string query = "Update CartTracs set TracNumber=@TracNumber where Id=@id";
                    SqlCommand sqlda = new SqlCommand(query, sqlcon);
                    sqlda.Parameters.AddWithValue("@Id", 1);
                    sqlda.Parameters.AddWithValue("TracNumber", inccartnumber);
                    sqlda.ExecuteNonQuery();
                }
                Session["MyCartNumber"] = 5;
                return inccartnumber;
            }
            else
            {
                return foundTracnumber;
            }

        }
        public void IncCartNumber(int Cartnumber)
        {
            Cartnumber = Cartnumber + 1;


        }
        public ActionResult getselectproduct(int Quantity, int pid, int Discount, int Price)
        {
            int cartnumber = GetMyCartNumber();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from Product where ProductId= @ProductId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@ProductId", pid);
                sqlda.Fill(dtbl);
            }

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Cart  values(@CartId,@ProductId,@Quantity,@ProductName,@Discount,@Price)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@CartId", cartnumber);
                sqlCmd.Parameters.AddWithValue("@ProductId", pid);
                sqlCmd.Parameters.AddWithValue("@Quantity", Quantity);
                sqlCmd.Parameters.AddWithValue("@ProductName", dtbl.Rows[0][1]);
                sqlCmd.Parameters.AddWithValue("@Discount", Discount);
                sqlCmd.Parameters.AddWithValue("@Price", Price);
                sqlCmd.ExecuteNonQuery();
            }

            if (Session["TracSelectedProduct"] == null)
            {
                List<SelectProducts> MyProduct = new List<SelectProducts>();
                MyProduct.Add(new SelectProducts()
                {
                    ProductId = pid,
                    Quantity = Quantity,
                    ProductPrice = Convert.ToInt32(dtbl.Rows[0][4]),
                    ProductName = dtbl.Rows[0][1].ToString(),
                    Discount = Discount

                });
                Session["TracSelectedProduct"] = MyProduct;

            }
            else
            {
                List<SelectProducts> MyProduct = (List<SelectProducts>)Session["TracSelectedProduct"];

                MyProduct.Add(new SelectProducts()
                {
                    ProductId = pid,
                    Quantity = Quantity,
                    ProductPrice = Convert.ToInt32(dtbl.Rows[0][4]),
                    ProductName = dtbl.Rows[0][1].ToString(),
                    Discount = Discount
                }
                    );
                Session["TracSelectedProduct"] = MyProduct;

            }
            return Redirect("SelectProduct");
        }

        DataTable dtblcart = new DataTable();
        public ActionResult CalculateOrder()
        {

            int cartid = GetMyCartNumber();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from Cart where CartId=@CartId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartId", cartid);
                sqlda.Fill(dtblcart);
            }
            return View(dtblcart);

        }
        public ActionResult updateselectedproduct(int pid, int Discount, int Quantity)
        {
            int cartnumber = GetMyCartNumber();
            Session["CartNumber"] = cartnumber;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Cart SET Quantity=@Quantity,Discount=@Discount where CartId=@CartId and ProductId=@ProductId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@Quantity", Quantity);
                sqlCmd.Parameters.AddWithValue("@Discount", Discount);
                sqlCmd.Parameters.AddWithValue("@ProductId", pid);
                sqlCmd.Parameters.AddWithValue("@CartId", cartnumber);
                sqlCmd.ExecuteNonQuery();
            }
            return Redirect("CalculateOrder");
        }
        public ActionResult MyOrder()
        {
            int cartnum = GetCartNumber();
            using (SqlConnection sqlcon = new SqlConnection())
            {
                sqlcon.Open();
                string query = "Insert into Cart values (@CartNumber)";
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.Parameters.AddWithValue("@CartNumber", cartnum);
                sqlcmd.ExecuteNonQuery();
            }
            return Redirect("Order");
        }

        public ActionResult OrderEmploye()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                string query = "Select * from Employe";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.Fill(dtbl);

            }

            return View(dtbl);
        }

        public ActionResult SelectEmploye(int id)
        {
            Session["EmployeId"] = id;
            TempData["MyEmployeeId"] = id;
            return View();

        }



        public ActionResult InsertOrder()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                int cartnum = GetMyCartNumber();
                string query = "Select * from Car where CartId=@CartId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartId", cartnum);
                sqlda.Fill(dtbl);
            }
            return View(dtbl);


        }










        public static List<Employee> employees = new List<Employee>()
        {
            new Employee() { Id = "1", Name = "Shihab" },
            new Employee() { Id = "2", Name = "Shibli" },
            new Employee() { Id = "3", Name = "Abir" }
        };
        public ActionResult Testt()
        {
            return View(employees);
        }

        // pdf

        public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 100f, 10f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayoutemp = new PdfPTable(5);
            PdfPTable tableLayout = new PdfPTable(8);
            PdfPTable SumTable = new PdfPTable(2);
            PdfPTable PartyTable = new PdfPTable(3);
            doc.SetMargins(10f, 10f, 100f, 10f);
            PdfPTable Signature = new PdfPTable(2);

            doc.SetMargins(10f, 10f, 100f, 10f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   

            doc.Add(Add_Content_To_PDFemp(tableLayoutemp));
            doc.Add(AddPartyToTable(PartyTable));

            doc.Add(Add_Content_To_PDF(tableLayout));
            doc.Add(AddSumToTable(SumTable));
            doc.Add(AddSignatureToTable(Signature));

            // Closing the document  
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            Session["MyCartNumber"] = null;
            return File(workStream, "application/pdf", strPDFFileName);
        }


        public FileResult Downloadpdf(string id)
        {
            int downloadcartid = Convert.ToInt32(id);
            Session["downloadcartid"] = downloadcartid;
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 100f, 10f);
            //Create PDF Table with 5 columns  
            PdfPTable DownloadPartyToTables = new PdfPTable(3);
            PdfPTable DownloadtableLayoutemp = new PdfPTable(5);
            PdfPTable DownloadtableLayout = new PdfPTable(8);
            PdfPTable DownloadSumTable = new PdfPTable(2);
            PdfPTable Signature = new PdfPTable(2);
            doc.SetMargins(10f, 10f, 100f, 10f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            //Add Content to PDF   
            doc.Add(Download_Add_Content_To_PDFemp(DownloadtableLayoutemp));
            doc.Add(DownloadPartyToTable(DownloadPartyToTables));
            doc.Add(Download_Add_Content_To_PDF(DownloadtableLayout));
            doc.Add(Download_AddSumToTable(DownloadSumTable));
            doc.Add(DownloadAddSignatureToTable(Signature));

            // Closing the document  
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        public FileResult CreateStateMent()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("StateMent" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 100f, 10f);
            //Create PDF Table with 5 columns  
            PdfPTable AddStatement = new PdfPTable(4);
            PdfPTable Employeinfo = new PdfPTable(3);
            PdfPTable Totaldue = new PdfPTable(2);
            doc.SetMargins(10f, 10f, 100f, 10f);
            PdfPTable Signature = new PdfPTable(2);
            doc.SetMargins(10f, 10f, 100f, 10f);
            //Create PDF Table  
            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            //Add Content to PDF   
            doc.Add(AddEmployeinfo(Employeinfo));
            doc.Add(Add_Content_To_PDFStatement(AddStatement));
            doc.Add(AddTotaldue(Totaldue));
            // Closing the document  


            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);
        }
        public int callDeposit(int partyid)
        {   DataTable deposit = new DataTable();
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                String query = "Select sum(Cash) from Deposit  where  EmployeId=@EmployeId and PartyId=@PartyId and Dates between  @StartDate   and @EndDate";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.SelectCommand.Parameters.AddWithValue("@StartDate", StartDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@EndDate", EndDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(deposit);
            }
            int cash = 0;
            if (deposit.Rows[0][0] != DBNull.Value)
            {
               
                cash = Convert.ToInt32(deposit.Rows[0][0]);

            }
            else
            {

                cash = 0;
            }
            return cash;
        }

        public int callcredit(int partyid)
        {
            DataTable Credit = new DataTable();
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select Sum(SUmCart.CartSum) as 'total' from SumCart  inner Join EmployeCart  On SumCart.CartNumber=EmployeCart.CartNumber where EmployeCart.EmployeId=@EmployeId and PartyId=@PartyId and Dates between  @StartDate   and @EndDate  Group by EmployeCart.PartyId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.SelectCommand.Parameters.AddWithValue("@StartDate", StartDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@EndDate", EndDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(Credit);
            }
            int cash = 0;
            if (Credit.Rows.Count == 0)
            {
                cash = 0;
            }
            else
            {
                cash = Convert.ToInt32(Credit.Rows[0][0]);

            }
            return cash;
        }

        public int calldue(int partyid)
        {
            DataTable Credit = new DataTable();
            DataTable deposit = new DataTable();
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select Sum(SUmCart.CartSum) as 'total' from SumCart  inner Join EmployeCart  On SumCart.CartNumber=EmployeCart.CartNumber where EmployeCart.EmployeId=@EmployeId and PartyId=@PartyId and Dates<  @StartDate  Group by EmployeCart.PartyId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.SelectCommand.Parameters.AddWithValue("@StartDate", StartDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(Credit);
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Deposit  where  EmployeId=@EmployeId  and PartyId=@PartyId and Dates <  @StartDate";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.SelectCommand.Parameters.AddWithValue("@StartDate", StartDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(deposit);
            }
            int depositcash;
            int Creditcash;
            int cash = 0;
           

            if ( deposit.Rows.Count==0 || deposit.Rows[0][0] == DBNull.Value)
            {

                depositcash = 0;
               

            }
            else
            {

                depositcash = Convert.ToInt32(deposit.Rows[0][0]);
            }
            if (Credit.Rows.Count==0  || Credit.Rows[0][0] == DBNull.Value)
            {

                Creditcash = 0;
            }
            else
            {
                Creditcash = Convert.ToInt32(Credit.Rows[0][0]);


                
            }


            int nitblance = depositcash - Creditcash;

            return nitblance;
        }

        public string Callpartyname(int partyid)
        {
            DataTable partytable = new DataTable();
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Party  where  EmployeId=@EmployeId  and PartyId=@PartyId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", partyid);
                sqlda.Fill(partytable);
            }
            string name = partytable.Rows[0][1].ToString();
            return name;
        }
        public string callEmploye(int empid)
        {
            DataTable Emptable = new DataTable();
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Employe  where  EmployeId=@EmployeId  ";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.Fill(Emptable);
            }
            string name = Emptable.Rows[0][1].ToString() + "  " + Emptable.Rows[0][2];
            return name;
        }
        protected PdfPTable AddEmployeinfo(PdfPTable tableLayout)
        {
            float[] headers = { 30, 35, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            DataTable deposit = new DataTable();
            DataTable partytable = new DataTable();
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            string employeName = callEmploye(empId);
            AddCellToHeader(tableLayout, "EmployeName");
            AddCellToHeader(tableLayout, "StartDate");
            AddCellToHeader(tableLayout, "EndDate");
            AddCellToBody(tableLayout, employeName.ToString());
            AddCellToBody(tableLayout, StartDate.ToString());
            AddCellToBody(tableLayout, EndDate.ToString());
            return tableLayout;
        }
        protected PdfPTable AddTotaldue(PdfPTable tableLayout)
        {
            float[] headers = { 80, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            DataTable deposit = new DataTable();
            DataTable partytable = new DataTable();
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            int sum = ((int)Session["sum"]);
            string employeName = callEmploye(empId);
            AddCellToHeader(tableLayout, "TotalDue");
            AddCellToHeader(tableLayout, sum.ToString());



            return tableLayout;
        }

        protected PdfPTable Add_Content_To_PDFStatement(PdfPTable tableLayout)
        {
            float[] headers = { 50, 15, 15, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            DataTable deposit = new DataTable();
            DataTable partytable = new DataTable();
            int duecal = 0;
            int creditcalc = 0;
            int depocalc = 0;
            int sum;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Party where EmployeId=@EmployeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.Fill(partytable);
            }
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            AddCellToHeader(tableLayout, "PartyName");
            AddCellToHeader(tableLayout, "Credit");
            AddCellToHeader(tableLayout, "Deposit");
            AddCellToHeader(tableLayout, "Due");
            for (int i = 0; i < partytable.Rows.Count; i++)
            {
                int depositt = callDeposit(Convert.ToInt32(partytable.Rows[i][0].ToString()));
                int credit = callcredit(Convert.ToInt32(partytable.Rows[i][0].ToString()));
                int due = calldue(Convert.ToInt32(partytable.Rows[i][0].ToString()));
                string partyName = Callpartyname(Convert.ToInt32(partytable.Rows[i][0].ToString()));
                AddCellToBody(tableLayout, partyName.ToString());
                AddCellToBody(tableLayout, credit.ToString());
                AddCellToBody(tableLayout, depositt.ToString());
                AddCellToBody(tableLayout, due.ToString());
                duecal = due + duecal;
                creditcalc = creditcalc + credit;
                depocalc = depocalc + depositt;
            }
            ////Add header  
            ///
            AddCellToHeader(tableLayout, "  Sum  ");
            AddCellToHeader(tableLayout, creditcalc.ToString());
            AddCellToHeader(tableLayout, depocalc.ToString());
            AddCellToHeader(tableLayout, duecal.ToString());


            //AddCellToHeader(tableLayout, dtblsum.Rows[0][2].ToString());

            sum = ((duecal + creditcalc) - depocalc);
            Session["sum"] = sum;
            Session["due"] = duecal;
            Session["credit"] = creditcalc;
            Session["deposit"] = depocalc;
            return tableLayout;
        }





        public ActionResult Negative()
        {

            return View();

        }


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout2)
        {

            int mycartnumber = GetMyCartNumber();
            DataTable dtblProduct = new DataTable();

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from InsertBillUnit where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblProduct);
            }

            DataTable stockpid = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Stock where productId=@productId  and PackSizeId=PackSizeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@productId", dtblProduct.Rows[0][1]);
                sqlda.SelectCommand.Parameters.AddWithValue("@PackSizeId", dtblProduct.Rows[0][2]);
                sqlda.Fill(stockpid);
            }
            int qntity = Convert.ToInt32(dtblProduct.Rows[0][3]);
            int bonus = Convert.ToInt32(dtblProduct.Rows[0][5]);
            int mainqntity = Convert.ToInt32(stockpid.Rows[0][5]);
            int updatequantity = mainqntity - (qntity + bonus);
            int pid = Convert.ToInt32(dtblProduct.Rows[0][1]);
            int packid = Convert.ToInt32(dtblProduct.Rows[0][2]);


            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Stock SET Quantity=@Quantity  where productId=@productId and PackSizeId=@PackSizeId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@Quantity", updatequantity);
                sqlCmd.Parameters.AddWithValue("@productId", pid);
                sqlCmd.Parameters.AddWithValue("@PackSizeId", packid);
                sqlCmd.ExecuteNonQuery();
            }

            float[] headers = { 10, 25, 15, 10, 10, 10, 10, 10 }; //Header Widths

            tableLayout2.SetWidths(headers); //Set the pdf headers  

            tableLayout2.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout2.HeaderRows = 1;

            string test = "Cash";

            tableLayout2.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            AddCellToHeader(tableLayout2, "SrNo");
            AddCellToHeader(tableLayout2, "ProductName");
            AddCellToHeader(tableLayout2, "PackSize Name");
            AddCellToHeader(tableLayout2, "Quantity");
            AddCellToHeader(tableLayout2, "Rate");
            AddCellToHeader(tableLayout2, "Discount");
            AddCellToHeader(tableLayout2, "Bonus");
            AddCellToHeader(tableLayout2, "Amount");
            double mytotalbill = 0;
            for (int i = 0; i < dtblProduct.Rows.Count; i++)
            {
                int z = i + 1;
                double g = Convert.ToDouble(dtblProduct.Rows[i][4].ToString());
                double p = g / 100;
                double storeunit = Convert.ToDouble(dtblProduct.Rows[i][9].ToString());
                double unitcalc = (storeunit * p);
                double storeminus = storeunit - unitcalc;
                double storetotalbillunit = Convert.ToDouble(dtblProduct.Rows[i][3].ToString());
                double totalunitbill = storetotalbillunit * storeminus;
                AddCellToBody(tableLayout2, z.ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][7].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][8].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][3].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][9].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][4].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][5].ToString());
                AddCellToBody(tableLayout2, totalunitbill.ToString());
                mytotalbill = mytotalbill + totalunitbill;

            }

            DataTable EmployeCart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from EmployeCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(EmployeCart);
            }
            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from Employe where EmployeId=@EmployeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeCart.Rows[0][1]);
                sqlda.Fill(dtblemploye);
            }

            DateTime dt = DateTime.Now;
            String date = dt.ToShortDateString();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int mycartNumber = GetMyCartNumber();
                sqlcon.Open();
                string query = "Insert into SumCart  values(@CartNumber,@CartSum,@DateTimes,@Dates,@EmployeName,@PhoneNumber,@Locations)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@CartNumber", mycartNumber);
                sqlCmd.Parameters.AddWithValue("@CartSum", mytotalbill);
                sqlCmd.Parameters.AddWithValue("@DateTimes", DateTime.Now);
                sqlCmd.Parameters.AddWithValue("@Dates", date);
                sqlCmd.Parameters.AddWithValue("@EmployeName", dtblemploye.Rows[0][1].ToString() + " " + dtblemploye.Rows[0][2].ToString());
                sqlCmd.Parameters.AddWithValue("@PhoneNumber", dtblemploye.Rows[0][3].ToString());
                sqlCmd.Parameters.AddWithValue("@Locations", dtblemploye.Rows[0][5].ToString());
                sqlCmd.ExecuteNonQuery();
            }


            return tableLayout2;
        }


        protected PdfPTable Download_Add_Content_To_PDF(PdfPTable tableLayout2)
        {

            int mycartnumber = Convert.ToInt32(Session["downloadcartid"].ToString());
            DataTable dtblProduct = new DataTable();

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from InsertBillUnit where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblProduct);
            }
            Console.WriteLine(dtblcart.Rows.Count);

            float[] headers = { 10, 25, 15, 10, 10, 10, 10, 10 }; //Header Widths

            tableLayout2.SetWidths(headers); //Set the pdf headers  
            //Add Title to the PDF file at the top  

            //List<Employee> employees = _context.employees.ToList<Employee>();

            tableLayout2.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout2.HeaderRows = 1;


            tableLayout2.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            AddCellToHeader(tableLayout2, "SrNo");
            AddCellToHeader(tableLayout2, "ProductName");
            AddCellToHeader(tableLayout2, "PackSize Name");
            AddCellToHeader(tableLayout2, "Quantity");
            AddCellToHeader(tableLayout2, "Rate");
            AddCellToHeader(tableLayout2, "Discount");
            AddCellToHeader(tableLayout2, "Bonus");
            AddCellToHeader(tableLayout2, "Amount");
            double mytotalbill = 0;
            for (int i = 0; i < dtblProduct.Rows.Count; i++)
            {
                int z = i + 1;
                double g = Convert.ToDouble(dtblProduct.Rows[i][4].ToString());
                double p = g / 100;
                double storeunit = Convert.ToDouble(dtblProduct.Rows[i][9].ToString());
                double unitcalc = (storeunit * p);
                double storeminus = storeunit - unitcalc;
                double storetotalbillunit = Convert.ToDouble(dtblProduct.Rows[i][3].ToString());
                double totalunitbill = storetotalbillunit * storeminus;
                AddCellToBody(tableLayout2, z.ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][7].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][8].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][3].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][9].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][4].ToString());
                AddCellToBody(tableLayout2, dtblProduct.Rows[i][5].ToString());
                AddCellToBody(tableLayout2, totalunitbill.ToString());
                mytotalbill = mytotalbill + totalunitbill;

            }

            DataTable EmployeCart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from EmployeCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(EmployeCart);
            }
            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from Employe where EmployeId=@EmployeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeCart.Rows[0][1]);
                sqlda.Fill(dtblemploye);
            }




            DateTime dt = DateTime.Now;
            String date = dt.ToShortDateString();


            return tableLayout2;
        }





        protected PdfPTable Add_Content_To_PDFemp(PdfPTable tableLayout)
        {
            float[] headers = { 13, 22, 15, 25, 25 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            int mycartnumber = GetMyCartNumber();
            DataTable EmployeCart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from EmployeCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(EmployeCart);
            }

            string cash = EmployeCart.Rows[0][4].ToString();
            if (cash != "")
            {
                tableLayout.AddCell(new PdfPCell(new Phrase("Invoice Table" + "(" + cash + ")", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                {
                    Colspan = 12,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new Phrase("Invoice Table", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                {
                    Colspan = 12,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

            }

            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from Employe where EmployeId=@EmployeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeCart.Rows[0][1]);
                sqlda.Fill(dtblemploye);
            }

            ////Add header  
            ///
            AddCellToHeader(tableLayout, "InvoiceNo");
            AddCellToHeader(tableLayout, "Employeename");
            AddCellToHeader(tableLayout, "Phone Number");

            AddCellToHeader(tableLayout, "Location");
            AddCellToHeader(tableLayout, "Date");

            ////Add body  
            ///          
            int mycartnumbera = GetMyCartNumber();
            AddCellToBody(tableLayout, mycartnumbera.ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][1].ToString() + " " + dtblemploye.Rows[0][2].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][3].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][5].ToString());
            AddCellToBody(tableLayout, DateTime.Now.ToString());

            return tableLayout;
        }







        protected PdfPTable Download_Add_Content_To_PDFemp(PdfPTable tableLayout)
        {
            float[] headers = { 13, 22, 15, 25, 25 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            //List<Employee> employees = _context.employees.ToList<Employee>();
            tableLayout.AddCell(new PdfPCell(new Phrase("Invoice Table", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            int mycartnumber = Convert.ToInt32(Session["downloadcartid"].ToString());
            DataTable EmployeCart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from EmployeCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(EmployeCart);
            }
            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from Employe where EmployeId=@EmployeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeCart.Rows[0][1]);
                sqlda.Fill(dtblemploye);
            }

            string cash = EmployeCart.Rows[0][4].ToString();
            ////Add header  
            ///
            AddCellToHeader(tableLayout, "InvoiceNo");
            AddCellToHeader(tableLayout, "Employeename");
            AddCellToHeader(tableLayout, "Phone Number");
            AddCellToHeader(tableLayout, "Location");
            AddCellToHeader(tableLayout, "Date");

            ////Add body  
            ///          
            DataTable dtblsum = new DataTable();
            // int downloadcartId = Convert.ToInt32(Session["downloadcartid"].ToString());
            int mycartnumberid = Convert.ToInt32(Session["downloadcartid"].ToString());

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from SumCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumberid);
                sqlda.Fill(dtblsum);
            }
            AddCellToBody(tableLayout, dtblsum.Rows[0][1].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][1].ToString() + " " + dtblemploye.Rows[0][2].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][3].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][5].ToString());
            AddCellToBody(tableLayout, dtblsum.Rows[0][3].ToString());

            return tableLayout;
        }




        protected PdfPTable AddSumToTable(PdfPTable tableLayout)
        {

            float[] headers = { 90, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            //List<Employee> employees = _context.employees.ToList<Employee>();
            int mycartnumber = GetMyCartNumber();
            DataTable dtblsum = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from SumCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblsum);
            }

            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            ///
            AddCellToHeader(tableLayout, "Total");
            AddCellToHeader(tableLayout, dtblsum.Rows[0][2].ToString());

            return tableLayout;
        }



        protected PdfPTable AddSignatureToTable(PdfPTable tableLayout)
        {
            tableLayout.SpacingAfter = 30;
            tableLayout.SpacingBefore = 30;
            float[] headers = { 50, 50 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            tableLayout.PaddingTop = 100f;

            //Add Title to the PDF file at the top  

            //List<Employee> employees = _context.employees.ToList<Employee>();
            int mycartnumber = GetMyCartNumber();
            DataTable dtblsum = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from SumCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblsum);
            }

            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                PaddingTop = 100,
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            ///
            AddCellToHeader(tableLayout, "Party Signature");
            AddCellToHeader(tableLayout, "Company Signature");

            return tableLayout;
        }




        protected PdfPTable DownloadAddSignatureToTable(PdfPTable tableLayout)
        {
            tableLayout.SpacingAfter = 30;
            tableLayout.SpacingBefore = 30;
            float[] headers = { 50, 50 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            tableLayout.PaddingTop = 100f;

            //Add Title to the PDF file at the top  

            //List<Employee> employees = _context.employees.ToList<Employee>();
            int mycartnumber = GetMyCartNumber();
            DataTable dtblsum = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from SumCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblsum);
            }

            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                PaddingTop = 100,
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            ///
            AddCellToHeader(tableLayout, "Party Signature");
            AddCellToHeader(tableLayout, "Company Signature");

            return tableLayout;
        }


        protected PdfPTable AddPartyToTable(PdfPTable tableLayout)
        {

            float[] headers = { 40, 30, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            int mycartnumber = GetMyCartNumber();
            DataTable EmployeCart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from EmployeCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(EmployeCart);
            }
            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from Party where PartyId=@PartyId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", EmployeCart.Rows[0][3]);
                sqlda.Fill(dtblemploye);
            }

            ////Add header  
            ///
            AddCellToHeader(tableLayout, "PartyName");
            AddCellToHeader(tableLayout, "PhoneNumber");
            AddCellToHeader(tableLayout, "Address");

            ////Add body  
            ///          
            int mycartnumbera = GetMyCartNumber();

            AddCellToBody(tableLayout, dtblemploye.Rows[0][1].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][2].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][3].ToString());




            ////Add header  
            ///

            return tableLayout;
        }
        protected PdfPTable DownloadPartyToTable(PdfPTable tableLayout)
        {

            float[] headers = { 40, 30, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            int mycartnumber = Convert.ToInt32(Session["downloadcartid"].ToString());
            //  int mycartnumber = ((int)Session["downloadcartid"]);


            DataTable EmployeCart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from EmployeCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(EmployeCart);
            }
            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int cons = 1;
                sqlcon.Open();
                string query = "Select  * from Party where PartyId=@PartyId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", EmployeCart.Rows[0][3]);
                sqlda.Fill(dtblemploye);
            }

            ////Add header  
            ///
            AddCellToHeader(tableLayout, "PartyName");
            AddCellToHeader(tableLayout, "PhoneNumber");
            AddCellToHeader(tableLayout, "Address");

            ////Add body  
            ///          
            int mycartnumbera = GetMyCartNumber();

            AddCellToBody(tableLayout, dtblemploye.Rows[0][1].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][2].ToString());
            AddCellToBody(tableLayout, dtblemploye.Rows[0][3].ToString());

            return tableLayout;
        }

        protected PdfPTable Download_AddSumToTable(PdfPTable tableLayout)
        {
            float[] headers = { 90, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            //List<Employee> employees = _context.employees.ToList<Employee>();
            int mycartnumber = Convert.ToInt32(Session["downloadcartid"].ToString());
            DataTable dtblsum = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from SumCart where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblsum);
            }

            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                PaddingLeft = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            AddCellToHeader(tableLayout, "Total");
            AddCellToHeader(tableLayout, dtblsum.Rows[0][2].ToString());

            return tableLayout;
        }


        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,

                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }



        public ActionResult CreateMyProducts()
        {

            return View();
        }

        [HttpPost]
        public ActionResult InsertMyProducts(Nproduct Products, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Productimg/"), pic);
                file.SaveAs(path);
            }
            else
            {

                pic = "No Image given";
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Nproduct  values(@ProductName,@Descriptions,@ProductImage)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductName", Products.Name);
                sqlCmd.Parameters.AddWithValue("@Descriptions", Products.Description);
                sqlCmd.Parameters.AddWithValue("@ProductImage", pic.ToString());
                sqlCmd.ExecuteNonQuery();
            }




            return RedirectToAction("MyProducts");
        }


        public List<SelectListItem> GetAllProducts()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct", sqlcon);
                sqlDa.Fill(dtblProduct);
                foreach (DataRow item in dtblProduct.Rows)
                {
                    list.Add(new SelectListItem { Value = item["Nproductid"].ToString(), Text = item["ProductName"].ToString() });
                }
            }
            return list;
        }

        [HttpGet]
        public ActionResult PackSize()
        {
            ViewBag.GetAllProducts = GetAllProducts();
            return View();

        }

        [HttpPost]
        public ActionResult PackSize(packsize PackSize)
        {
            DataTable dtblPackSize = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct  where  Nproductid=@Nproductid", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Nproductid", PackSize.ProductId);
                sqlDa.Fill(dtblPackSize);
            }
            int Quantity = 0;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Packsize  values(@ProductId,@PackSizeName,@Price,@ProductName)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductId", PackSize.ProductId);
                sqlCmd.Parameters.AddWithValue("@PackSizeName", PackSize.PackSizeName);
                sqlCmd.Parameters.AddWithValue("@Price", PackSize.Price);
                sqlCmd.Parameters.AddWithValue("@ProductName", dtblPackSize.Rows[0][1].ToString());
                sqlCmd.ExecuteNonQuery();
            }

            int ProductId = Convert.ToInt32(PackSize.ProductId);

            DataTable dtblp = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT TOP 1 PackId FROM Packsize ORDER BY PackId DESC", sqlcon);
                sqlDa.Fill(dtblp);
            }
            int PackSizeId = Convert.ToInt32(dtblp.Rows[0][0].ToString());


            InsertStockByPack(ProductId, PackSizeId, Quantity);

            //  InsertStock(dtblPackSize.Rows[0][0].ToString(),PackSize.PackId,);

            return RedirectToAction("MyPackSize");
        }


        public List<packsize> GetPacklistInsideController(int ProductId)
        {
            List<packsize> list = new List<packsize>();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Packsize  where  ProductId=@ProductId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductId", ProductId);
                sqlDa.Fill(dtblProduct);
                foreach (DataRow item in dtblProduct.Rows)
                {
                    list.Add(new packsize()
                    {
                        PackId = Convert.ToInt32(item["PackId"]),
                        ProductId = Convert.ToInt32(item["ProductId"]),
                        PackSizeName = item["PackSizeName"].ToString(),
                        Price = Convert.ToInt32(item["Price"])

                    });
                }
            }
            return list;

        }

        public List<party> GetPartylistInsideController(int EmployeId)
        {
            List<party> list = new List<party>();
            DataTable dtblemploye = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Party  where  EmployeId=@EmployeId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeId);
                sqlDa.Fill(dtblemploye);
                int x = dtblemploye.Rows.Count;
                foreach (DataRow item in dtblemploye.Rows)
                {
                    list.Add(new party()
                    {
                        PartyId = Convert.ToInt32(item["PartyId"]),
                        PartyName = item["PartName"].ToString(),
                        PartyPhoneNumber = item["PartyPhonenumber"].ToString(),
                        PartyAddress = item["PartyAddress"].ToString(),
                        EmployeId = Convert.ToInt32(item["EmployeId"]),
                        EmployeName = item["EmployeName"].ToString(),
                        PartyDuplicate = item["PartyDuplicate"].ToString()

                    });
                }
            }
            return list;

        }

        public List<Nproduct> GetProductList()
        {
            List<Nproduct> list = new List<Nproduct>();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct", sqlcon);
                sqlDa.Fill(dtblProduct);
                foreach (DataRow item in dtblProduct.Rows)
                {   list.Add(new Nproduct()
                    {
                        ProdductId = Convert.ToInt32(item["Nproductid"]),
                        Name = item["ProductName"].ToString(),
                        Description = item["Descriptions"].ToString(),
                        image = item["ProductImage"].ToString()

                    });
                }
            }
            return list;

        }

        public List<Employe> GetEmployeList()
        {
            List<Employe> list = new List<Employe>();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Employe", sqlcon);
                sqlDa.Fill(dtblProduct);
                foreach (DataRow item in dtblProduct.Rows)
                {
                    list.Add(new Employe()
                    {
                        EmployeId = Convert.ToInt32(item["EmployeId"]),
                        EmployeFirstName = item["EmployeFirstName"].ToString(),
                        EmployeLastName = item["EMployeLastName"].ToString(),
                    });
                }
            }
            return list;

        }


        public JsonResult GetPacklist(int ProductId)
        {
            List<packsize> pack = GetPacklistInsideController(ProductId);
            return Json(pack, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPartyList(int EmployeId)
        {
            List<party> party = GetPartylistInsideController(EmployeId);
            return Json(party, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ProductPack()
        {
            List<Nproduct> list = GetProductList();
            ViewBag.Countrylist = new SelectList(list, "ProdductId", "Name");
            return View();
        }
        public ActionResult StockProducts()
        {
            List<Nproduct> list = GetProductList();
            ViewBag.Countrylist = new SelectList(list, "ProdductId", "Name");
            return View();
        }
        public ActionResult UpdateStock()
        {
            List<Nproduct> list = GetProductList();
            ViewBag.Countrylist = new SelectList(list, "ProdductId", "Name");
            return View();
        }


        [HttpPost]
        public JsonResult PostBillUnit(ProductPack productpack)
        {
            string msg = "Success";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagerParty()
        {
            List<Employe> employe = GetEmployeList();
            ViewBag.employelist = new SelectList(employe, "EmployeId", "EmployeFirstName");
            return View();

        }

        public JsonResult PostEMployeManager(managerparty managerparty)
        {
            string msg = "Success";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public void CallController(string id)
        {
            string x = id;
        }
        public void InsertBillUnit(string ProductId, string PackSizeId, string Quantity, string Discount, string Bonus)
        {
            int MyCartNumber;

            MyCartNumber = GetMyCartNumber();

            Session["BillingStart"] = 5;
            int MProductId = Convert.ToInt32(ProductId.ToString());
            int MPackSizeId = Convert.ToInt32(PackSizeId.ToString());
            int MQuantity = Convert.ToInt32(Quantity);
            int MDiscount = Convert.ToInt32(Discount);
            int MBonus = Convert.ToInt32(Bonus);
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct  where  Nproductid=@Nproductid", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Nproductid", ProductId);
                sqlDa.Fill(dtblProduct);
            }
            DataTable dtblPackSize = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Packsize  where  PackId=@PackId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PackId", PackSizeId);
                sqlDa.Fill(dtblPackSize);
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into InsertBillUnit  values(@ProductId,@PackSizeId,@Quantity,@Discount,@Bonus,@CartNumber,@ProductName,@PackSizeName,@ProductPrice)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductId", MProductId);
                sqlCmd.Parameters.AddWithValue("@PackSizeId", MPackSizeId);
                sqlCmd.Parameters.AddWithValue("@Quantity", MQuantity);
                sqlCmd.Parameters.AddWithValue("@Discount", MDiscount);
                sqlCmd.Parameters.AddWithValue("@Bonus", MBonus);
                sqlCmd.Parameters.AddWithValue("@CartNumber", MyCartNumber);
                sqlCmd.Parameters.AddWithValue("@ProductName", dtblProduct.Rows[0][1].ToString());
                sqlCmd.Parameters.AddWithValue("@PackSizeName", dtblPackSize.Rows[0][2].ToString());
                sqlCmd.Parameters.AddWithValue("@ProductPrice", dtblPackSize.Rows[0][3]);
                sqlCmd.ExecuteNonQuery();
            }


        }


        public void InsertStock(string ProductId, string PackSizeId, string Quantity)
        {
            int MyCartNumber;

            MyCartNumber = GetMyCartNumber();

            Session["BillingStart"] = 5;
            int MProductId = Convert.ToInt32(ProductId.ToString());
            int MPackSizeId = Convert.ToInt32(PackSizeId.ToString());
            int MQuantity = Convert.ToInt32(Quantity);
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct  where  Nproductid=@Nproductid", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Nproductid", ProductId);
                sqlDa.Fill(dtblProduct);
            }
            DataTable dtblPackSize = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Packsize  where  PackId=@PackId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PackId", PackSizeId);
                sqlDa.Fill(dtblPackSize);
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Stock  values(@productId,@PackSizeId,@ProductName,@PackSizeName,@Quantity)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductId", MProductId);
                sqlCmd.Parameters.AddWithValue("@PackSizeId", MPackSizeId);
                sqlCmd.Parameters.AddWithValue("@ProductName", dtblProduct.Rows[0][1].ToString());
                sqlCmd.Parameters.AddWithValue("@PackSizeName", dtblPackSize.Rows[0][2].ToString());

                sqlCmd.Parameters.AddWithValue("@Quantity", MQuantity);
                sqlCmd.ExecuteNonQuery();
            }


        }





        public void InsertStockByPack(int ProductId, int PackSizeId, int Quantity)
        {
            int MyCartNumber;

            MyCartNumber = GetMyCartNumber();

            Session["BillingStart"] = 5;
            int MProductId = Convert.ToInt32(ProductId.ToString());
            int MPackSizeId = Convert.ToInt32(PackSizeId.ToString());
            int MQuantity = Convert.ToInt32(Quantity);
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Nproduct  where  Nproductid=@Nproductid", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Nproductid", ProductId);
                sqlDa.Fill(dtblProduct);
            }
            DataTable dtblPackSize = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Packsize  where  PackId=@PackId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PackId", PackSizeId);
                sqlDa.Fill(dtblPackSize);
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Stock  values(@productId,@PackSizeId,@ProductName,@PackSizeName,@Quantity)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductId", MProductId);
                sqlCmd.Parameters.AddWithValue("@PackSizeId", MPackSizeId);
                sqlCmd.Parameters.AddWithValue("@ProductName", dtblProduct.Rows[0][1].ToString());
                sqlCmd.Parameters.AddWithValue("@PackSizeName", dtblPackSize.Rows[0][2].ToString());

                sqlCmd.Parameters.AddWithValue("@Quantity", MQuantity);
                sqlCmd.ExecuteNonQuery();
            }


        }





        public JsonResult TestFunction(string ProductId, string PackSizeId, string Quantity, string Discount, string Bonus)
        {
            int mycartnumber = GetMyCartNumber();
            DataTable dtblProduct = new DataTable();

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from InsertBillUnit where CartNumber=@CartNumber";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlda.Fill(dtblProduct);
            }
            int MProductId = Convert.ToInt32(ProductId.ToString());
            int MPackSizeId = Convert.ToInt32(PackSizeId.ToString());
            int MQuantity = Convert.ToInt32(Quantity);
            int MDiscount = Convert.ToInt32(Discount);
            int MBonus = Convert.ToInt32(Bonus);

            DataTable stockpid = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Stock where productId=@productId  AND PackSizeId=@PackSizeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@productId", MProductId);
                sqlda.SelectCommand.Parameters.AddWithValue("@PackSizeId", MPackSizeId);
                sqlda.Fill(stockpid);
            }
            int counnt = stockpid.Rows.Count;
            int mainqntity = Convert.ToInt32(stockpid.Rows[0][5]);
            int updatequantity = mainqntity - (MQuantity + MBonus);
            if (updatequantity < 0)
            {
                return Json(true);
            }
            else
            {
                InsertBillUnit(ProductId, PackSizeId, Quantity, Discount, Bonus);
                return Json(new
                {
                    resut = " OK  got it"
                });
            }


        }
        public JsonResult UpdateStocks(string ProductId, string PackSizeId, string Quantity)
        {
            int qntty = Convert.ToInt32(Quantity);
            int pid = Convert.ToInt32(ProductId);
            int packid = Convert.ToInt32(PackSizeId);

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Stock SET Quantity=@Quantity  where productId=@productId and PackSizeId=@PackSizeId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@Quantity", qntty);
                sqlCmd.Parameters.AddWithValue("@productId", pid);
                sqlCmd.Parameters.AddWithValue("@PackSizeId", packid);
                sqlCmd.ExecuteNonQuery();
            }

            return Json(new
            {
                resut = " OK  got it"
            });
        }

        public JsonResult StockManagement(string ProductId, string PackSizeId, string Quantity)
        {

            InsertStock(ProductId, PackSizeId, Quantity);
            return Json(new
            {
                resut = " OK  got it"
            });
        }

        public JsonResult TestManagerParty(string EmployeId, string PartyId, string Cash)
        {
            Session["PartyId"] = PartyId;
            Session["ManagerId"] = EmployeId;
            int mycartnumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into EmployeCart  values(@EmployeId,@CartNumber,@PartyId,@Cash)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@EmployeId", EmployeId);
                sqlCmd.Parameters.AddWithValue("@CartNumber", mycartnumber);
                sqlCmd.Parameters.AddWithValue("@PartyId", PartyId);
                sqlCmd.Parameters.AddWithValue("@Cash", Cash);
                sqlCmd.ExecuteNonQuery();
            }
            return Json(new
            {
                redirectUrl = Url.Action("ViewManagerparty", "Admin"),
                isRedirect = true
            });


        }
        //InsertDeposit
        //Cash= Deposit Amount
        public JsonResult TestDeposit(string EmployeId, string PartyId, string Cash, string dates)
        {
            int Cashs = Convert.ToInt32(Cash);
            int PtyId = Convert.ToInt32(PartyId);
            int EId = Convert.ToInt32(EmployeId);
            DateTime dt = DateTime.Now;
            String date = dt.ToShortDateString();
            DataTable employeName = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from  Employe  where EmployeId=@EmployeId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", EId);
                sqlda.Fill(employeName);
            }
            DataTable PartyTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select * from  Party  where PartyId=@PartyId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@PartyId", PtyId);
                sqlda.Fill(PartyTable);
            }

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Deposit  values(@PartyId,@EmployeId,@Cash,@Dates,@PartyEmployeName,@EmployeEmployeName)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@PartyId", PtyId);
                sqlCmd.Parameters.AddWithValue("@EmployeId", EId);
                sqlCmd.Parameters.AddWithValue("@Cash", Cashs);
                sqlCmd.Parameters.AddWithValue("@Dates", dates);
                sqlCmd.Parameters.AddWithValue("@PartyEmployeName", PartyTable.Rows[0][1].ToString());
                sqlCmd.Parameters.AddWithValue("@EmployeEmployeName", employeName.Rows[0][1].ToString() + ' ' + employeName.Rows[0][2].ToString());
                sqlCmd.ExecuteNonQuery();
            }
            return Json(new
            {
                redirectUrl = Url.Action("ShowDeposit", "Admin"),
                isRedirect = true
            });


        }


        public ActionResult Test()
        {
            return View();
        }

        public ActionResult FindStateMent()
        {
            List<Employe> employe = GetEmployeList();
            ViewBag.employelist = new SelectList(employe, "EmployeId", "EmployeFirstName");
            return View();

        }
        public ActionResult ShowSateMent(FindStateMent fdsmnt)
        {
            DataTable deposit = new DataTable();
            DataTable Credit = new DataTable();
            Session["EmpId"] = fdsmnt.EmployeId;
            Session["StartDate"] = fdsmnt.StartDate;
            Session["EndDate"] = fdsmnt.EndDate;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select Sum(SUmCart.CartSum) as 'total',PartyId from SumCart  inner Join EmployeCart  On SumCart.CartNumber=EmployeCart.CartNumber where EmployeCart.EmployeId=@EmployeId  and Dates between  @StartDate   and @EndDate  Group by EmployeCart.PartyId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", fdsmnt.EmployeId);
                sqlda.SelectCommand.Parameters.AddWithValue("@StartDate", fdsmnt.StartDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@EndDate", fdsmnt.EndDate);
                sqlda.Fill(Credit);
            }
            DataTable deposits = new DataTable();
            int empId = Convert.ToInt32(Session["EmpId"].ToString());
            DateTime StartDate = (DateTime)Session["StartDate"];
            DateTime EndDate = (DateTime)Session["EndDate"];
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                String query = "Select * from Deposit  where  EmployeId=@EmployeId and Dates between  @StartDate   and @EndDate";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlda.SelectCommand.Parameters.AddWithValue("@StartDate", StartDate);
                sqlda.SelectCommand.Parameters.AddWithValue("@EndDate", EndDate);
                
                sqlda.Fill(deposits);
            }

            return View(deposits);
        }



        public JsonResult SearchManagerParty(string EmployeId, string PartyId, string Cash)
        {
            Session["PartyId"] = PartyId;
            Session["ManagerId"] = EmployeId;
            int mycartnumber = GetMyCartNumber();
            DataTable dtblPackSize = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from EmployeCart  where EmployeId=@EmployeId  AND PartyId=@PartyId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", EmployeId);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PartyId", PartyId);
                sqlDa.Fill(dtblPackSize);
            }

            DataTable dtblFIN = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from SumCart  where  CartNumber=@CartNumber  AND  Dates >= CURRENT_TIMESTAMP -7  ", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@CartNumber", dtblPackSize.Rows[0][2]);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PartyId", PartyId);
                sqlDa.Fill(dtblFIN);
            }

            //  return Json(dtblFIN, "ViewSearch");
            return Json(new { result = "Redirect", url = Url.Action("ViewSearch", "Admin", dtblFIN) });

        }

        public ActionResult Searchorder()
        {
            List<Employe> employe = GetEmployeList();
            ViewBag.employelist = new SelectList(employe, "EmployeId", "EmployeFirstName");
            return View();

        }

        public ActionResult ViewSearch(DataTable dtbl)
        {
            return View(dtbl);

        }


        public ActionResult ViewManagerparty()
        {
            int PartyId = Convert.ToInt32(Session["PartyId"].ToString());
            DataTable dtblPackSize = new DataTable();
            int CartNumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Party  where  PartyId=@PartyId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PartyId", PartyId);
                sqlDa.Fill(dtblPackSize);
            }
            return View(dtblPackSize);
        }


        public ActionResult ViewSelectedProducts()
        {
            DataTable dtblPackSize = new DataTable();
            int CartNumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from InsertBillUnit  where  CartNumber=@CartNumber", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@CartNumber", CartNumber);
                sqlDa.Fill(dtblPackSize);
            }
            ViewBag.Sum = 100;
            return View(dtblPackSize);
        }

        public ActionResult DeleteBillUnit(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Delete  from InsertBillUnit where BillUnitId=@BillUnitId";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@BillUnitId", id);
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("ViewSelectedProducts");
        }
        public List<SelectListItem> GetMyEmployeeName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("select * from Employe ", sqlcon);
                sqlDa.Fill(dtbl);
                foreach (DataRow item in dtbl.Rows)
                {
                    list.Add(new SelectListItem { Value = item["EmployeId"].ToString(), Text = item["EmployeFirstName"].ToString() });
                }
            }
            return list;
        }




        public List<SelectListItem> GetMyEmployeeNameById(int empId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("select * from Employe where EmployeId=@EmployeId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", empId);
                sqlDa.Fill(dtbl);
                foreach (DataRow item in dtbl.Rows)
                {
                    list.Add(new SelectListItem { Value = item["EmployeId"].ToString(), Text = item["EmployeFirstName"].ToString() });
                }
            }
            return list;
        }

        public List<SelectListItem> GetMyParty(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Party  where  EmployeId=@EmployeId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", id);
                sqlDa.Fill(dtbl);
                foreach (DataRow item in dtbl.Rows)
                {
                    list.Add(new SelectListItem { Value = item["PartyId"].ToString(), Text = item["PartName"].ToString() });
                }
            }
            return list;
        }



        public ActionResult SelectEmployee()
        {
            ViewBag.getAllEMployee = GetMyEmployeeName();
            return View();
        }

        public ActionResult GetSelectedEmploye(Employe Employe)
        {
            int CartNumber = GetMyCartNumber();
            int EmployeId = Employe.EmployeId;
            Session["EmployeId"] = EmployeId;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into EmployeCart  values(@EmployeId,@CartNumber)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@EmployeId", Employe.EmployeId);
                sqlCmd.Parameters.AddWithValue("@CartNumber", CartNumber);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("ViewSelectedEmployeDetails");
        }

        public ActionResult ViewSelectedEmployeDetails()
        {
            int employeId = Convert.ToInt32(Session["EmployeId"].ToString());
            DataTable dtblPackSize = new DataTable();
            int CartNumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Employe  where  EmployeId=@EmployeId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", employeId);
                sqlDa.Fill(dtblPackSize);
            }

            return View(dtblPackSize);
        }
        public ActionResult ChangeEmploye(int id)
        {
            int CartNumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Delete  from EmployeCart where PartyId=@id";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@id", id);
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("ManagerParty");
        }

        public ActionResult DeleteStock(int id)
        {
            int CartNumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Delete  from Stock where StockId=@StockId";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@id", id);
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("ManagerParty");
        }

        public ActionResult ViewOrderPdf()
        {
            DataTable dtblsumcart = new DataTable();
            List<Employe> employe = GetEmployeList();
            ViewBag.employelist = new SelectList(employe, "EmployeId", "EmployeFirstName");


            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from SumCart order by id DESC", sqlcon);
                sqlDa.Fill(dtblsumcart);
            }
            return View(dtblsumcart);
        }
        public ActionResult party()
        {

            return View();
        }
        [HttpGet]
        public ActionResult AddParty()
        {
            ViewBag.getAllEMployee = GetMyEmployeeName();
            return View();
        }



        [HttpPost]
        public ActionResult AddParty(party party)
        {
            int employeId = party.EmployeId;
            DataTable dtblPackSize = new DataTable();
            int CartNumber = GetMyCartNumber();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Employe  where  EmployeId=@EmployeId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", employeId);
                sqlDa.Fill(dtblPackSize);
            }

            DataTable dtblparty = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Party  values(@PartName,@PartyPhonenumber,@PartyAddress,@EmployeId,@EmployeName,@PartyDuplicate)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@PartName", party.PartyName);
                sqlCmd.Parameters.AddWithValue("@PartyPhonenumber", party.PartyPhoneNumber);
                sqlCmd.Parameters.AddWithValue("@PartyAddress", party.PartyAddress);
                sqlCmd.Parameters.AddWithValue("@EmployeId", party.EmployeId);
                sqlCmd.Parameters.AddWithValue("@EmployeName", dtblPackSize.Rows[0][1].ToString() + "  " + dtblPackSize.Rows[0][2].ToString());
                sqlCmd.Parameters.AddWithValue("@PartyDuplicate", party.PartyName);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("AddParty");
        }

        public ActionResult ViewAllParty()
        {
            DataTable dtblparty = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "select * from Party";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.Fill(dtblparty);
            }
            return View(dtblparty);
        }

        [HttpGet]
        public ActionResult EditParty(int PartyId)
        {
            ViewBag.getAllEMployee = GetMyEmployeeName();
            party party = new party();
            DataTable dtblparty = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Party  where PartyId = @PartyId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PartyId", PartyId);
                sqlDa.Fill(dtblparty);
            }
            if (dtblparty.Rows.Count == 1)
            {
                party.EmployeId = Convert.ToInt32(dtblparty.Rows[0][0].ToString());
                party.PartyName = dtblparty.Rows[0][1].ToString();
                party.PartyPhoneNumber = dtblparty.Rows[0][2].ToString();
                party.PartyAddress = dtblparty.Rows[0][3].ToString();
                party.EmployeName = dtblparty.Rows[0][4].ToString();
                return View(party);
            }
            else
            {
                return RedirectToAction("ViewAllParty");
            }

        }
        [HttpPost]
        public ActionResult EditParty(party party)
        {
            DataTable dtbliftable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Party  where PartyId = @PartyId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PartyId", party.PartyId);
                sqlDa.Fill(dtbliftable);
            }

            DataTable dtblparty = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = $"Select  * from Employe  where EmployeId = @EmployeId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@EmployeId", party.EmployeId);
                sqlDa.Fill(dtblparty);
            }
            int x = party.PartyId;

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Party SET PartName=@PartName,PartyPhonenumber=@PartyPhonenumber,PartyAddress=@PartyAddress,EmployeId=@EmployeId,EmployeName=@EmployeName  where PartyId=@PartyId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@PartName", party.PartyName.ToString());
                sqlCmd.Parameters.AddWithValue("@PartyPhonenumber", party.PartyPhoneNumber.ToString());
                sqlCmd.Parameters.AddWithValue("@PartyAddress", party.PartyAddress.ToString());
                if (party.EmployeId == 0)
                {
                    sqlCmd.Parameters.AddWithValue("@EmployeId", dtbliftable.Rows[0][0]);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@EmployeId", party.EmployeId);
                }

                sqlCmd.Parameters.AddWithValue("@PartyId", party.PartyId);
                if (party.EmployeId == 0)
                {
                    sqlCmd.Parameters.AddWithValue("@EmployeName", dtbliftable.Rows[0][5].ToString());
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@EmployeName", dtblparty.Rows[0][1].ToString() + " " + dtblparty.Rows[0][2].ToString());

                }
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ViewAllParty");
        }

        public ActionResult ViewStock()
        {
            DataTable dtblstock = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Stock", sqlcon);
                sqlDa.Fill(dtblstock);
            }
            return View(dtblstock);


        }

        public ActionResult login(string email, string pass)
        {
            string setmail = "mir@gmail.com";
            string SetPassword = "state2021";

            if (email == setmail.ToString() && pass == SetPassword.ToString())
            {

                return Redirect("~/Admin/ViewSelectedProducts");


            }
            else
            {
                return Redirect("~/Home/Index");
            }


        }

        public ActionResult Index()
        {

            return View();
        }
    }
}