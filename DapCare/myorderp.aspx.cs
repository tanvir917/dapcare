using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DapCare
{
    public partial class pdf : System.Web.UI.Page
    {
        DataTable dtblemp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = DESKTOP-2V8523P\SQLEXPRESS;  Initial Catalog =DapCare;  Integrated Security  = true";
            int cons = 1;
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Select TracNumber from CartTracs where Id=@Id";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@Id", cons);
                sqlda.Fill(dtbl);
            }
            int foundTracnumber = Convert.ToInt32(dtbl.Rows[0][0]);
            DataTable dtblProduct = new DataTable();
            int tt = Convert.ToInt32(Session["CartNumber"]);
            Console.WriteLine(tt);
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                String query = "Select * from Cart where CartId=@CartId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@CartId", foundTracnumber);
                sqlda.Fill(dtblProduct);
            }
            
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                String query = "Select * from Employe where EmployeId=@EmployeId";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("@EmployeId", Convert.ToInt32( Session["EmployeId"]));
                sqlda.Fill(dtblemp);
            }

            Console.WriteLine(dtblemp.Rows.Count);
           
            gridViewData.DataSource = dtblProduct;
            gridViewData.DataBind();
        }
        public void btnPrint_Click(object sender,EventArgs e) {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition","attachment;filename=print.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.Write("hhhhhhhh");
            hw.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
            hw.AddAttribute(HtmlTextWriterAttribute.Height, "50%");

            hw.RenderBeginTag(HtmlTextWriterTag.Img);
            hw.RenderEndTag();
            panelPdf.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4,10f,10f,100f,10f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc,Response.OutputStream);

            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
    }
}