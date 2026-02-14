using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using System.Linq;
using System.Collections;
/// <summary>
/// Form For Voucher View Report
/// </summary>
public partial class Forms_RptFranchiseSaleMonthWise : System.Web.UI.Page
{
    readonly FranchiseSaleInvoiceController _franchiseController = new FranchiseSaleInvoiceController();
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
            LoadBranchFranchise();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            CreateTable();
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadBranchFranchise()
    {
        DistributorController DController = new DistributorController();
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 5);
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "CUSTOMER_ID", "CUSTOMER_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    protected void drpDistributor_SELECTEDINDEXCHANGED(object sender, EventArgs e)
    {
        CreateTable();
        GrdLedger.DataSource = new DataTable();
        GrdLedger.DataBind();
    }
    /// <summary>
    /// Loads Vouchers To Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnView_Click(object sender, EventArgs e)
    {
        var _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        DateTime month = DateTime.Parse("01" + "-" + txtStartDate.Text + " 00:00:00");
        RptCustomerController RptCustomerCtl = new RptCustomerController();


        DataSet ds = _franchiseController.SelectFranchiseReportByPriceLevel(
            int.Parse(drpDistributor.SelectedItem.Value.ToString()),
           month,
           DateTime.Parse(DateTime.DaysInMonth(month.Year, month.Month) 
           + "-" + txtStartDate.Text + " 23:59:59")
           );

        DataTable dt = ds.Tables["RptFranchiseSaleInvoice"];
        if (dt.Rows.Count > 0)
        {
            DataRow[] foundAuthors = _purchaseSkus.Select("Month = '" + txtStartDate.Text + "'");
            if (foundAuthors.Length != 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record already exists with this Month');", true);
                return;
            }
            //    DataTable newdt = dt.AsEnumerable()
            //.GroupBy(r => new { Col1 = r["account_head_id"], Col2 = r["account_code"] })
            //.Select(g => g.OrderBy(r => r["ledger_date"]).First())
            //.CopyToDataTable();


            //    DataRow[] unqiueDr = dt.Select("account_head_id = '" + newdt.Rows[0]["account_head_id"].ToString() + "'");

            //var credit = dt.Rows.Cast<DataRow>().Sum(row => row.Field<decimal>("credit"));
            //var debit = dt.Rows.Cast<DataRow>().Sum(row => row.Field<decimal>("debit"));
            //var opening = 0;
            //var balance = ((opening > 0 ? +opening : -opening) + credit) - debit;

            DataTable newdt = dt.AsEnumerable()
            .GroupBy(r => new { Col1 = r["CUSTOMER_ID"] })
            .Select(g => g.OrderBy(r => r["FRANCHISE_MASTER_ID"]).First())
            .CopyToDataTable();

            foreach (DataRow item in newdt.Rows)
            {
                DataRow[] unqiueDr = dt.Select("CUSTOMER_ID = '" + item["CUSTOMER_ID"].ToString() + "'");
                decimal subtotalByBranch = 0;

                if (unqiueDr != null && unqiueDr.Length > 0)
                {
                    foreach (var itm in unqiueDr)
                    {
                        subtotalByBranch += (Convert.ToDecimal(itm["PRICE"].ToString()) +
                       (Convert.ToDecimal(itm["PRICE"].ToString()) *
                       (Convert.ToDecimal(itm["PriceFactorPercent"].ToString()) / 100))) *
                       Convert.ToDecimal(itm["QUANTITY"].ToString());
                    }
                }

                DataRow dr = _purchaseSkus.NewRow();
                dr["Month"] = txtStartDate.Text;
                dr["ItemAmount"] = subtotalByBranch.ToString("#,##0.00");

                dr["CUSTOMER_NAME"] = item["CUSTOMER_NAME"].ToString();
                dr["CUSTOMER_ID"] = item["CUSTOMER_ID"].ToString();

                _purchaseSkus.Rows.Add(dr);
            }
        }
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        if (_purchaseSkus != null && _purchaseSkus.Rows.Count > 0)
        {
            decimal grandTotal = 0;
            foreach (DataRow dr in _purchaseSkus.Rows)
            {
                grandTotal += Convert.ToDecimal(dr["ItemAmount"]);
            }

            GrdLedger.Columns[2].FooterText = grandTotal.ToString("#,##0.00");
            GrdLedger.DataSource = _purchaseSkus;
            GrdLedger.DataBind();

           // GroupGridView(GrdLedger.Rows, 0, 3);
        }
    }
    private void CreateTable()
    {
        var _PurchaseSKUS = new DataTable();
        _PurchaseSKUS.Columns.Add("ItemAmount", typeof(string));
        _PurchaseSKUS.Columns.Add("CUSTOMER_NAME", typeof(string));
        _PurchaseSKUS.Columns.Add("CUSTOMER_ID", typeof(string));
        _PurchaseSKUS.Columns.Add("Month", typeof(string));
        this.Session.Add("PurchaseSKUS", _PurchaseSKUS);
    }
    /// <summary>
    /// Shows Voucher in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdLedger_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewRow gvr = GrdLedger.Rows[e.NewEditIndex];
        string date = gvr.Cells[2].Text;

        DateTime month = DateTime.Parse("01" + "-" + date + " 00:00:00");

        DocumentPrintController mController = new DocumentPrintController();
        CrpFranchiseInvoiceBranchWise CrpReport = new CrpFranchiseInvoiceBranchWise();
        DataTable dt = mController.SelectReportTitle(int.Parse(gvr.Cells[0].Text));

        DataSet ds = _franchiseController.SelectFranchiseReportByPriceLevel(
            int.Parse(gvr.Cells[0].Text),
            month,
            DateTime.Parse(DateTime.DaysInMonth(month.Year, month.Month) + "-" + date + " 23:59:59"));

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("ReportName", "Item Wise Detailed Sales");
        CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
        CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
        CrpReport.SetParameterValue("ContactNumber", dt.Rows[0]["CONTACT_NUMBER"].ToString());
        CrpReport.SetParameterValue("Address", dt.Rows[0]["ADDRESS1"].ToString());
        CrpReport.SetParameterValue("Email", dt.Rows[0]["ADDRESS2"].ToString());
        CrpReport.SetParameterValue("FromDate", "01-"+ txtStartDate.Text.ToString());
        CrpReport.SetParameterValue("ToDate", DateTime.Parse(DateTime.DaysInMonth(month.Year, month.Month) 
            + "-" + date + " 23:59:59").ToString("dd-MMM-yyyy"));

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);

        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}