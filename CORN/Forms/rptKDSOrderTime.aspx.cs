using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_rptKDSOrderTime : System.Web.UI.Page
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
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
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
        if (Grid_users.Rows.Count > 0)
        {
            var crpReport = new CrpKDSOrderTime();
            var rptSaleCtl = new RptSaleController();
            DataSet ds = (DataSet)Session["uspGetKDSOrderTime"];
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("FROM_DATE", txtStartDate.Text);
            crpReport.SetParameterValue("TO_DATE", txtEndDate.Text);
            crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", pReportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
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
        var rptSaleCtl = new RptSaleController();
        DataSet ds = rptSaleCtl.GetKDSOrderTim(int.Parse(drpDistributor.SelectedItem.Value.ToString()),DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"));
        Grid_users.DataSource = ds.Tables["uspGetKDSOrderTime"];
        Grid_users.DataBind();
        Session.Add("uspGetKDSOrderTime", ds);
    }
}