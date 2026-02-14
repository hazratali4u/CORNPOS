using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_rptOpenOrders : System.Web.UI.Page
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
        }
    }
    
    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
            DistributorController DController = new DistributorController();
            DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
          //  drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
           
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Shows Customer Invoice Wise Sales Either in PDF Or in Excel
    /// </summary>
    /// <param name="pReportType">ReportType</param>
    private void ShowReport(int pReportType)
    {
        var mDocumentPrntControl = new DocumentPrintController();
        var rptCustomerCtl = new RptCustomerController();

        string invoiceId = (from GridViewRow dr in Grid_users.Rows let chbSelect = (CheckBox)(dr.Cells[0].FindControl("ChbCustomer")) where chbSelect.Checked select dr).Aggregate("0", (current, dr) => current + "," + dr.Cells[11].Text);
        DataSet ds = rptCustomerCtl.SelectInvoiceDetail(Convert.ToInt32(Session["UserID"]), 
            invoiceId, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 
            Constants.DateNullValue, Constants.DateNullValue, 5,Constants.IntNullValue, Constants.IntNullValue);
        var crpReport = new CrpOpeningOrders();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("CompanyName", drpDistributor.SelectedItem.Text);
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", pReportType);
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
        ShowReport(0);

    }
    /// <summary>
    /// Shows Customer Invoice Wise Sales in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        string strInvoiceNo2 = null;
        
        var rptCustomerCtl = new RptCustomerController();
        DataTable dt2 = rptCustomerCtl.SelectInvoiceDetail2(Constants.IntNullValue,strInvoiceNo2,int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.DateNullValue,Constants.DateNullValue, 4);
        Grid_users.DataSource = dt2;
        Grid_users.DataBind();
    }
}