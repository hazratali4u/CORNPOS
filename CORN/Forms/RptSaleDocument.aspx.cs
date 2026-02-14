using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;
using CORNBusinessLayer.Reports;

public partial class Forms_RptSaleDocument : System.Web.UI.Page
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
            this.LoadDistributor();
            LoadCustomers();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>


    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void LoadCustomers()
    {
        CustomerDataController mController = new CustomerDataController();
        drpCustomer.DataSource = null;
        drpCustomer.DataBind();
        drpCustomer.Items.Add("All", Constants.LongNullValue);
        DataTable dt = mController.UspSelectCustomer(int.Parse(drpDistributor.Value.ToString()),"All", "", DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpCustomer, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
        if (dt.Rows.Count > 0)
        {
            drpCustomer.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Shows Purchase Document in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void showReport(int type)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            RptSaleController saleController = new RptSaleController();
            CrpSaleInvoice CrpReport = new CrpSaleInvoice();
            DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));

            DataSet ds = saleController.SelectSaleInvoiceReport(Constants.IntNullValue,
                Constants.IntNullValue.ToString(), int.Parse(drpDistributor.Value.ToString()),
                DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text),
               Constants.IntNullValue, 6, Constants.ByteNullValue, long.Parse(drpCustomer.Value.ToString()));

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("ReportName", "SALE INVOICE");
            CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
            CrpReport.SetParameterValue("ContactNumber", dt.Rows[0]["CONTACT_NUMBER"].ToString());
            CrpReport.SetParameterValue("Address", dt.Rows[0]["ADDRESS1"].ToString());
            CrpReport.SetParameterValue("Email", dt.Rows[0]["ADDRESS2"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", type);
            const string url = "'Default.aspx'";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }
}