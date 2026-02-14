using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

public partial class Forms_RptInvoiceWiseSale : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadOrderBooker();
            LoadPaymentMode();
            LoadUser();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadUser()
    {
        ddUser.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            try
            {
                Distributor_UserController du = new Distributor_UserController();
                DataTable dt = du.SelectDistributorUser(10, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(Session["CompanyId"].ToString()));
                ddUser.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
                clsWebFormUtil.FillDxComboBoxList(ddUser, dt, 0, 1);
                ddUser.SelectedIndex = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void LoadOrderBooker()
    {
        DrpOrderBooker.Items.Clear();
        Distributor_UserController UController = new Distributor_UserController();
        if (drpDistributor.Items.Count > 0)
        {
            DataTable dtEmployee = UController.SelectDistributorUser(Constants.SALES_FORCE_ORDERBOOKER, int.Parse(drpDistributor.SelectedItem.Value.ToString().ToString()), int.Parse(this.Session["CompanyId"].ToString()));
            if (dtEmployee.Rows.Count <= 0) return;
            DrpOrderBooker.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

            clsWebFormUtil.FillDxComboBoxList(DrpOrderBooker, dtEmployee, "USER_ID", "USER_NAME");

            if (dtEmployee.Rows.Count > 0)
            {
                DrpOrderBooker.SelectedIndex = 0;
            }

        }
    }
    private void LoadPaymentMode()
    {
        ddlPaymentMode.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        ddlPaymentMode.Items.Add(new DevExpress.Web.ListEditItem("Cash", "0"));
        ddlPaymentMode.Items.Add(new DevExpress.Web.ListEditItem("Credit Card", "1"));
        ddlPaymentMode.Items.Add(new DevExpress.Web.ListEditItem("Credit", "2"));
        ddlPaymentMode.SelectedIndex = 0;
    }

    /// <summary>
    /// Shows Customer Invoice Wise Sales Either in PDF Or in Excel
    /// </summary>
    /// <param name="pReportType">ReportType</param>
    private void ShowReport()
    {
        var mDocumentPrntControl = new DocumentPrintController();
        var rptCustomerCtl = new RptCustomerController();
        string invoiceId = (from GridViewRow dr in Grid_users.Rows let chbSelect = (CheckBox)(dr.Cells[0].FindControl("ChbCustomer")) where chbSelect.Checked select dr).Aggregate("0", (current, dr) => current + "," + dr.Cells[2].Text);
        DataSet ds = rptCustomerCtl.SelectInvoiceDetail(Convert.ToInt32(Session["UserID"]), invoiceId,
            int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"),
            DateTime.Parse(txtEndDate.Text + " 23:59:59"),
            6,
            int.Parse(DrpOrderBooker.SelectedItem.Value.ToString()),
            int.Parse(ddUser.SelectedItem.Value.ToString())
            );

        var crpReport = new CrpBillInvoiceWiseReport();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("CompanyName", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("ReportType", "");
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    /// <summary>
    /// Shows Customer Invoice Wise Sales in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void BtnViewPdf_Click(object sender, EventArgs e)
    {
        ShowReport();

    }
    /// <summary>
    /// Shows Customer Invoice Wise Sales in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        string ReportName = "Invoice Wise Sales Report";
        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (ExcelPackage p = new ExcelPackage())
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add(ReportName);
            var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
            var headerFont = headerCells.Style.Font;
            headerFont.Bold = true;
            var mDocumentPrntControl = new DocumentPrintController();
            var rptCustomerCtl = new RptCustomerController();
            string invoiceId = (from GridViewRow dr in Grid_users.Rows let chbSelect = (CheckBox)(dr.Cells[0].FindControl("ChbCustomer")) where chbSelect.Checked select dr).Aggregate("0", (current, dr) => current + "," + dr.Cells[2].Text);
            DataSet ds = rptCustomerCtl.SelectInvoiceDetail(Convert.ToInt32(Session["UserID"]), invoiceId, int.Parse(drpDistributor.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 6, int.Parse(DrpOrderBooker.SelectedItem.Value.ToString()), int.Parse(ddUser.SelectedItem.Value.ToString()));
            ShowReportExcel(ws, p, ds.Tables["UspBillWiseInvoiceReport"]);
            Byte[] fileBytes = p.GetAsByteArray();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + ReportName.Replace(" ", "") + ".xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
        }        
    }
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        string strInvoiceNo2 = null;
        string[] strInvoiceNo = null;
        if (txtInvoiceNo.Text.Trim().Length > 0)
        {
            if (txtInvoiceNo.Text.Contains(","))
            {
                strInvoiceNo = txtInvoiceNo.Text.Split(',');

                foreach (string s in strInvoiceNo)
                {
                    strInvoiceNo2 += s;
                    strInvoiceNo2 += ",";
                }
            }
            else
            {
                strInvoiceNo2 = txtInvoiceNo.Text;
            }
        }
        var rptCustomerCtl = new RptCustomerController();
        DataTable dt2 = rptCustomerCtl.SelectInvoiceDetail2(Convert.ToInt32(Session["UserID"]),
            strInvoiceNo2, int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"),
            DateTime.Parse(txtEndDate.Text + " 23:59:59"),
            Convert.ToInt32(DrpOrderBooker.SelectedItem.Value), 2,
            Convert.ToInt32(ddlPaymentMode.SelectedItem.Value),
            Convert.ToInt32(ddUser.SelectedItem.Value)
            );

        Grid_users.DataSource = dt2;
        Grid_users.DataBind();
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOrderBooker();
        LoadUser();
    }

    private void ShowReportExcel(
    ExcelWorksheet ws,
    ExcelPackage p,
    DataTable UspBillWiseInvoiceReport)
    {
        int rowIndex = 1;

        if (UspBillWiseInvoiceReport.Rows.Count == 0)
            return;

        // GROUP BY BILL NO
        var billGroups = UspBillWiseInvoiceReport.AsEnumerable()
            .GroupBy(r => r.Field<long>("BILL NO"));

        foreach (var bill in billGroups)
        {
            DataRow header = bill.First(); // one row per bill

            // =========================
            // REPORT HEADER
            // =========================
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Invoice Wise Sales Report";
            ws.Cells[rowIndex, 1].Style.Font.Size = 20;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Location: " + drpDistributor.SelectedItem.Text;
            ws.Cells[rowIndex, 1].Style.Font.Size = 14;

            rowIndex+=2;
            var range = ws.Cells[rowIndex, 1, rowIndex, 15];
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Color.SetColor(Color.Black);

            rowIndex += 2;

            ws.Cells[rowIndex, 1].Value =
                "Order Date: " +
                Convert.ToDateTime(header["BILL DATE"])
                .ToString("dd-MMM-yyyy hh:mm tt");

            ws.Cells[rowIndex, 6].Value =
                "Service Type: " + header["CUSTOMER_NAME"];

            // =========================
            // ITEM TABLE HEADER
            // =========================
            ws.Cells[rowIndex, 9].Value = "Item Name";
            ws.Cells[rowIndex, 10].Value = "Quantity";
            ws.Cells[rowIndex, 11].Value = "Price";
            ws.Cells[rowIndex, 12].Value = "Gross Amount";

            ws.Cells[rowIndex, 9, rowIndex, 12]
                .Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            // =========================
            // INVOICE DETAILS
            // =========================
            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Invoice No: " + header["BILL NO"];
            ws.Cells[rowIndex, 6].Value =
                "Table No: " + header["SKU_CODE"];

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Order Taker: " + header["orderBooker"];
            ws.Cells[rowIndex, 6].Value =
                "KOT No: " + header["MANUAL_ORDER_NO"];

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Cover Table: " + header["coverTable"];

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Customer Name: " + header["CUSTOMER_NAME2"];

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Contact No: " + header["CONTACT_NUMBER"];

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Address: " + header["ADDRESS"];

            rowIndex++;
            ws.Cells[rowIndex, 1].Value =
                "Cash Out Time: " +
                Convert.ToDateTime(header["LASTUPDATE_DATE"])
                .ToString("dd-MMM-yyyy hh:mm tt");

            // =========================
            // ITEM ROWS (INNER LOOP)
            // =========================
            decimal TotalAmount = 0;
            decimal TotalQty = 0;
            foreach (DataRow item in bill)
            {
                rowIndex++;

                ws.Cells[rowIndex, 9].Value = item["SKU_NAME"];
                ws.Cells[rowIndex, 10].Value = item["QUANTITY_UNIT"];
                ws.Cells[rowIndex, 11].Value = item["UNIT_PRICE"];
                ws.Cells[rowIndex, 12].Value = item["AMOUNT"];

                TotalAmount += Convert.ToDecimal(item["AMOUNT"]);
                TotalQty += Convert.ToDecimal(item["QUANTITY_UNIT"]);
            }

            rowIndex ++;
            var range2 = ws.Cells[rowIndex, 1, rowIndex, 15];
            range2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range2.Style.Border.Bottom.Color.SetColor(Color.Black);

            rowIndex++;
            ws.Cells[rowIndex, 12].Value = TotalAmount;
            ws.Cells[rowIndex, 10].Value = TotalQty;

            rowIndex++;
            var range3 = ws.Cells[rowIndex, 1, rowIndex, 15];
            range3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range3.Style.Border.Bottom.Color.SetColor(Color.Black);
            #region Footer            

            //Discount
            rowIndex++;
            if (header["STATUS"].ToString() == "0")
            {
                ws.Cells[rowIndex, 11].Value = "Discount @ " + header["VALUE OFTER SCHEME"].ToString() + ":";
            }
            else
            {
                ws.Cells[rowIndex, 11].Value = "Discount" + ":";
            }
            ws.Cells[rowIndex, 12].Value = header["SCHEME AMOUNT"];

            //GST
            rowIndex++;
            decimal GST = 0;
            if(Convert.ToDecimal(header["GROSS SALE"]) != Convert.ToDecimal(header["SCHEME AMOUNT"]))
            {
                GST = Convert.ToDecimal(header["GSTPER"]);
            }
            ws.Cells[rowIndex, 11].Value = "Gst " + GST.ToString() + " %";
            ws.Cells[rowIndex, 12].Value = header["GST AMOUNT"];

            //Service Charges
            rowIndex++;
            if (header["CUSTOMER_NAME"].ToString() == "Delivery")
            {
                ws.Cells[rowIndex, 11].Value = "Delivery Charges";
            }
            else
            {
                ws.Cells[rowIndex, 11].Value = "Service Charges";
            }
            ws.Cells[rowIndex, 12].Value = header["SERVICE_CHARGES"];

            //Net Amount
            rowIndex++;
            decimal NetAmount = Convert.ToDecimal(header["NET_AMOUNT"]) + Convert.ToDecimal(header["SERVICE_CHARGES"]) + Convert.ToDecimal(header["GST AMOUNT"]);
            ws.Cells[rowIndex, 11].Value = "Net Amount";
            ws.Cells[rowIndex, 12].Value = NetAmount;

            //Credit Card Or Credit Invoice
            if (header["STATUS2"].ToString() == "Credit Card" || header["STATUS2"].ToString() == "Credi")
            {
                //Payment In
                rowIndex++;
                ws.Cells[rowIndex, 11].Value = "Payment IN";
                ws.Cells[rowIndex, 12].Value = header["CREDIT AMOUNT"];

                //Credit Card Or Credit
                rowIndex++;
                decimal CreditCardValue = NetAmount - Convert.ToDecimal(header["CREDIT AMOUNT"]);
                ws.Cells[rowIndex, 11].Value = header["STATUS2"].ToString();
                ws.Cells[rowIndex, 12].Value = CreditCardValue;

                //Balance
                rowIndex++;
                ws.Cells[rowIndex, 11].Value = "Balance";
                ws.Cells[rowIndex, 12].Value = 0;
            }

            #endregion
            // =========================
            // SPACE BEFORE NEXT BILL
            // =========================
            rowIndex += 3;
        }
    }

}