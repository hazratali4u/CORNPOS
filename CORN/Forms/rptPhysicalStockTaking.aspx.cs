using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_rptIncomeStatement : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptInventoryController _rptInventoryCtl = new RptInventoryController();
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
        var mUserController = new UserController();
        DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), 2, 1, int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 1);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void ShowReport(int reportType)
    {
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();

        if (drpDistributor.SelectedItem.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                sbLocationIDs.Append(li.Value);
                sbLocationIDs.Append(",");
            }
        }
        else
        {
            sbLocationIDs.Append(drpDistributor.SelectedItem.Value);
        }


        DataSet ds = _rptInventoryCtl.GetPhysicalStockTaking(sbLocationIDs.ToString(),DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));


        var crpReport = new CrpPhysicalStockTakingNew();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();

        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("ToDate", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("PrintedBy", Session["UserName"].ToString());


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
}