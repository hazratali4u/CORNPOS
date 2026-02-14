using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;

public partial class Forms_RptTopSellingItems : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly SkuController _orderEntryController = new SkuController();
    readonly DistributorController _dController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        drpDistributor.DataSource = null;
        drpDistributor.DataBind();
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add("All", Constants.IntNullValue.ToString());
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
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
    private void ShowReport(int pReprotType)
    {
        System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
        if(drpDistributor.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach(DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");
            }
        }
        else
        {
            sbDistributorIDs.Append(drpDistributor.Value);
        }
        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;
        ds = _orderEntryController.GetTopSellingItems(sbDistributorIDs.ToString(),DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"),int.Parse(RbReportType.SelectedValue),int.Parse(drpOrderBy.Value.ToString()));
        var crpReport = new ReportDocument();
        if (RbReportType.SelectedValue == "1")
        {
            crpReport = new CrpTopSellingItems();
        }
        else
        {
            crpReport = new CrpTopSellingItemsCategoryWise();
        }
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("ReportName", RbReportType.SelectedValue == "1" ? "Top Selling Items": "Top Selling Categories");
        crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Username", Session["UserName"].ToString());
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", pReprotType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void RbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}