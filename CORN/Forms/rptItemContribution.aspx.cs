using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_rptItemContribution : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtFromMonth.Text = Configuration.SystemCurrentDateTime.ToString("MMM-yyyy");
            txtToMonth.Text = Configuration.SystemCurrentDateTime.ToString("MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
            txtFromMonth.Attributes.Add("readonly", "readonly");
            txtToMonth.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        drpDistributor.Items.Clear();
        var mUserController = new UserController();
        DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), 2, 1, int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 1);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void ShowReport(int reportType)
    {
        DateTime dtFrom;
        DateTime dtTo;
        if (DrpReportType.SelectedItem.Value.ToString() == "2")
        {
            DateTime dtFromMonth = DateTime.Parse(txtFromMonth.Text);
            dtFrom = new DateTime(dtFromMonth.Year, dtFromMonth.Month, 1);

            DateTime dtToMonth = DateTime.Parse(txtToMonth.Text);
            dtTo = new DateTime(dtToMonth.Year, dtToMonth.Month, 1);
            dtTo = dtTo.AddMonths(1).AddDays(-1);

        }
        else
        {
            dtFrom = Convert.ToDateTime(txtStartDate.Text);
            dtTo = Convert.ToDateTime(txtEndDate.Text);
        }
        if (DrpReportType.Value.ToString() == "1")
        {
            DataSet ds = _rptSaleCtl.GetRegionItemSaleDetail(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), dtFrom, dtTo, 2, null);
            var crpReport = new CrpItemContribution();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("fromDate", txtStartDate.Text);
            crpReport.SetParameterValue("todate", txtEndDate.Text);
            crpReport.SetParameterValue("ReportTitle", "Item Sales Contribution");
            crpReport.SetParameterValue("CompanyName", drpDistributor.SelectedItem.Text);

            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", reportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else
        {
            DataSet ds = _rptSaleCtl.GetMonthWiseContribution(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), dtFrom, dtTo, 6, null);
            var crpReport = new CrpItemContributionMonth();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("fromDate", txtStartDate.Text);
            crpReport.SetParameterValue("todate", txtEndDate.Text);
            crpReport.SetParameterValue("ReportTitle", "Item Sales Contribution Month Wise");
            crpReport.SetParameterValue("CompanyName", drpDistributor.SelectedItem.Text);

            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", reportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {

        ShowReport(1);
    }

    protected void DrpReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        divDate.Visible = false;
        divMonth.Visible = false;
        if(DrpReportType.Value.ToString() == "1")
        {
            divDate.Visible = true;
        }
        else if (DrpReportType.Value.ToString() == "2")
        {
            divMonth.Visible = true;
        }
    }
}