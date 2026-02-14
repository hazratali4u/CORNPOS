using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

public partial class Forms_RptHourlySales : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    readonly SkuController SKUCtl = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadHours();
            LoadAssingned();
            LoadSKU();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        drpDistributor.Items.Clear();
        var mUserController = new DistributorController();
        DataTable dt = mUserController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void LoadHours()
    {
        cblHour.Items.Add(new ListItem("08-09 AM", "8"));
        cblHour.Items.Add(new ListItem("09-10 AM", "9"));
        cblHour.Items.Add(new ListItem("10-11 AM", "10"));
        cblHour.Items.Add(new ListItem("11-12 AM", "11"));
        cblHour.Items.Add(new ListItem("12-01 PM", "12"));
        cblHour.Items.Add(new ListItem("01-02 PM", "13"));
        cblHour.Items.Add(new ListItem("02-03 PM", "14"));
        cblHour.Items.Add(new ListItem("03-04 PM", "15"));
        cblHour.Items.Add(new ListItem("04-05 PM", "16"));
        cblHour.Items.Add(new ListItem("05-06 PM", "17"));
        cblHour.Items.Add(new ListItem("06-07 PM", "18"));
        cblHour.Items.Add(new ListItem("07-08 PM", "19"));
        cblHour.Items.Add(new ListItem("08-09 PM", "20"));
        cblHour.Items.Add(new ListItem("09-10 PM", "21"));
        cblHour.Items.Add(new ListItem("10-11 PM", "22"));
        cblHour.Items.Add(new ListItem("11-12 PM", "23"));
        cblHour.Items.Add(new ListItem("12-01 AM", "0"));
        cblHour.Items.Add(new ListItem("01-02 AM", "1"));
        cblHour.Items.Add(new ListItem("02-03 AM", "2"));
        cblHour.Items.Add(new ListItem("03-04 AM", "3"));
        cblHour.Items.Add(new ListItem("04-05 AM", "4"));
        cblHour.Items.Add(new ListItem("05-06 AM", "5"));
        cblHour.Items.Add(new ListItem("06-07 AM", "6"));
        cblHour.Items.Add(new ListItem("07-08 AM", "7"));

        foreach (ListItem li in cblHour.Items)
        {
            li.Selected = true;
        }
    }
    private void LoadSKU()
    {
        cblItem.Items.Clear();
        DataTable dt = SKUCtl.GetSKUInfo(Constants.IntNullValue, Constants.DateNullValue, 7);
        if (dt.Rows.Count > 0)
        {
            clsWebFormUtil.FillListBox(cblItem, dt, 0, 1);
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = true;
            }
            cbItemAll.Checked = true;
        }
        else
        {
            cbItemAll.Checked = false;
        }
    }
    private void ShowReport(int reportType)
    {
        System.Text.StringBuilder sbHour = new System.Text.StringBuilder();        
        foreach (ListItem li in cblHour.Items)
        {
            if(li.Selected)
            {
                sbHour.Append(li.Value);
                sbHour.Append(",");
            }
        }
        
        DataSet ds = null;
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument crpReport = new ReportDocument();
        if (rbReportType.SelectedValue == "0")
        {
            ds = _rptSaleCtl.GethourlySaleDetail(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text),sbHour.ToString(), int.Parse(rbReportType.SelectedValue));
            crpReport = new CrpHourlySale();
        }
        else if (rbReportType.SelectedValue == "1")
        {
            ds = _rptSaleCtl.GethourlySaleDetailSummary(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text),sbHour.ToString(), int.Parse(rbReportType.SelectedValue));
            crpReport = new CrpHourlySaleSummary();
        }
        else if (rbReportType.SelectedValue == "2")
        {
            System.Text.StringBuilder sbSKUS = new System.Text.StringBuilder();
            foreach (ListItem li in cblItem.Items)
            {
                if (li.Selected)
                {
                    sbSKUS.Append(li.Value);
                    sbSKUS.Append(",");
                }
            }
            ds = _rptSaleCtl.GethourlySalesItemWise(int.Parse(drpDistributor.SelectedItem.Value.ToString()),int.Parse(Session["UserId"].ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), sbSKUS.ToString(), 1);
            crpReport = new CrpHourlyItemWiseSales();
        }
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("fromDate", txtStartDate.Text);
        crpReport.SetParameterValue("todate", txtEndDate.Text);
        crpReport.SetParameterValue("PrintedBy", Session["UserName"].ToString());
        if (rbReportType.SelectedValue == "0")
        {
            crpReport.SetParameterValue("ReportTitle", "Date Wise Hourly Sales Detail");
        }
        else if (rbReportType.SelectedValue == "1")
        {
            crpReport.SetParameterValue("ReportTitle", "Date Wise Hourly Net Sales Summary");
        }
        else if (rbReportType.SelectedValue == "2")
        {
            crpReport.SetParameterValue("ReportTitle", "Item Wise Hourly Sales Summary");
        }

        crpReport.SetParameterValue("CompanyName", drpDistributor.SelectedItem .Text);

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", reportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {


        ShowReport(0);

    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {

        ShowReport(1);
    }
    protected void rbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvItem.Visible = false;
        if(rbReportType.SelectedValue == "2")
        {
            dvItem.Visible = true;
        }
    }
}