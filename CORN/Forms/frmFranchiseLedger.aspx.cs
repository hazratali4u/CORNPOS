using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using System.Linq;
/// <summary>
/// Form For Voucher View Report
/// </summary>
public partial class Forms_frmFranchiseLedger : System.Web.UI.Page
{
    public static string opType = "CR";
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
        DataSet ds = RptCustomerCtl.CustomerLedger_View(Constants.IntNullValue, Session["DISTRIBUTOR_ID"].ToString(), int.Parse(drpDistributor.SelectedItem.Value.ToString()), month, DateTime.Parse(DateTime.DaysInMonth(month.Year, month.Month) + "-" + txtStartDate.Text + " 23:59:59"), 4);
        DataTable dt = ds.Tables["RptCustomerLedgerView"];
        if (dt.Rows.Count > 0)
        {
            DataRow[] foundAuthors = _purchaseSkus.Select("Month = '" + txtStartDate.Text + "'");
            if (foundAuthors.Length != 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record already exists with this Month');", true);
                return;
            }
            var credit = dt.Rows.Cast<DataRow>().Sum(row => row.Field<decimal>("credit"));
            var debit = dt.Rows.Cast<DataRow>().Sum(row => row.Field<decimal>("debit"));
            var opening = LoadCustomerOpBalance();
            var balance = ((opening > 0 ? +opening : -opening) + credit) - debit;

            DataRow dr = _purchaseSkus.NewRow();
            dr["Month"] = txtStartDate.Text;
            dr["credit"] = credit;
            dr["debit"] = debit;
            dr["opening"] = balance;

            _purchaseSkus.Rows.Add(dr);
        }
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        if (_purchaseSkus != null && _purchaseSkus.Rows.Count > 0)
        {
            GrdLedger.DataSource = _purchaseSkus;
            GrdLedger.DataBind();
        }
    }
    private decimal LoadCustomerOpBalance()
    {
        if (drpDistributor.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dt = mController.SelectPrincipalCustomerOpBalance(
                int.Parse(Session["DISTRIBUTOR_ID"].ToString()),
                int.Parse(drpDistributor.SelectedItem.Value.ToString()),
                DateTime.Parse("01" + "-" + txtStartDate.Text + " 00:00:00"), Constants.IntNullValue);

            return decimal.Parse(dt.Rows[0][0].ToString());
        }
        return 0;
    }
    private void CreateTable()
    {
        var _PurchaseSKUS = new DataTable();
        _PurchaseSKUS.Columns.Add("debit", typeof(string));
        _PurchaseSKUS.Columns.Add("credit", typeof(decimal));
        _PurchaseSKUS.Columns.Add("opening", typeof(decimal));
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
        string date = gvr.Cells[0].Text;

        DateTime month = DateTime.Parse("01" + "-" + date + " 00:00:00");

        DataControl dc = new DataControl();
        CORNBusinessLayer.Classes.DocumentPrintController DPrint = new CORNBusinessLayer.Classes.DocumentPrintController();
        RptCustomerController RptCustomerCtl = new RptCustomerController();
        DataSet ds;
        ds = RptCustomerCtl.CustomerLedger_View(Constants.IntNullValue,Session["DISTRIBUTOR_ID"].ToString(),int.Parse(drpDistributor.SelectedItem.Value.ToString()),month,DateTime.Parse(DateTime.DaysInMonth(month.Year, month.Month) + "-" + date + " 23:59:59"),4);
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CORNBusinessLayer.Reports.CrpCustomerLedger CrpReport = new CORNBusinessLayer.Reports.CrpCustomerLedger();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();


        CrpReport.SetParameterValue("FromDate", DateTime.Parse("01" + "-" + date));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(DateTime.DaysInMonth(month.Year, month.Month) + "-" + date));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        string customer = drpDistributor.SelectedItem.Text;
        CrpReport.SetParameterValue("custCode", "");
        CrpReport.SetParameterValue("Customer", customer);
        CrpReport.SetParameterValue("Op_Balance", LoadCustomerOpBalanceForReport(month));
        CrpReport.SetParameterValue("Op_Type", 0);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("CustomerOrFranchise", "Franchise Ledger");
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    private decimal LoadCustomerOpBalanceForReport(DateTime date)
    {
        if (drpDistributor.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dt = mController.SelectPrincipalCustomerOpBalance(
                int.Parse(Session["DISTRIBUTOR_ID"].ToString()),
                int.Parse(drpDistributor.SelectedItem.Value.ToString()),
                date,
                Constants.IntNullValue);

            if (Convert.ToDecimal(dt.Rows[0][0]) < 0)
            {
                opType = "DR";
            }
            else if (Convert.ToDecimal(dt.Rows[0][0]) == 0)
            {
                opType = "";
            }
            else
            {
                opType = "CR";
            }
            return decimal.Parse(dt.Rows[0][0].ToString());
        }
        return 0;
    }
}