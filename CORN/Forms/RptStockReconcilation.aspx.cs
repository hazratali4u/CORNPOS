using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;
using DevExpress.Web;
using System.Web.UI.WebControls;
/// <summary>
/// Form For Stock Reconciliation Report
/// </summary>
public partial class Forms_RptStockReconcilation : System.Web.UI.Page
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
            LoadSection();

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadSection()
    {
        try
        {
            DataTable dt = new DataTable();
            SkuController _skuController = new SkuController();
            dt = _skuController.SelectProductSection(Constants.IntNullValue, null, null);
            clsWebFormUtil.FillListBox(chblSection, dt, 0, 2, true);

            foreach(ListItem li in chblSection.Items)
            {
                li.Selected = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
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

        clsWebFormUtil.FillListBox(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void showReport(int reporType)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        int Type = 1;
        int EliminateZero = 0;
        string IsLocationWiseItem = "0";
        string sectionIDs = "";
        if (cbInActive.Checked)
        {
            Type = 2;
        }
        if(cbZero.Checked)
        {
            EliminateZero = 1;
        }

        if (Session["IsLocationWiseItem"].ToString() == "1")
        {
            IsLocationWiseItem = "1";
        }
        string distributorIds = "";
        string distributorName = "";
        int locationCount = 0;
        foreach (ListItem li in drpDistributor.Items)
        {
            if (li.Selected == true)
            {
                distributorIds += li.Value.ToString() + ",";
                distributorName += li.Text.ToString() + ", ";
                locationCount++;
            }
        }

        if (locationCount == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please select Location')", true);
            return;
        }

        foreach (ListItem li in chblSection.Items)
        {
            if (li.Selected == true)
                sectionIDs += li.Value.ToString() + ",";
        }

        distributorIds = distributorIds.TrimEnd(',');
        distributorName = distributorName.TrimEnd(',');

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));

        DataSet ds = RptInventoryCtl.SelectPrincipalStockReconcilation(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            Constants.IntNullValue, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text),
            int.Parse(this.Session["UserId"].ToString()), Type, Convert.ToInt32(rblRate.SelectedItem.Value), distributorIds,
            IsLocationWiseItem, sectionIDs,EliminateZero);

        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpStockReconsiliationNew();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("division", distributorName);
        CrpReport.SetParameterValue("fromdate", txtStartDate.Text);
        CrpReport.SetParameterValue("todate", txtEndDate.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("ReportType", "Stock Reconciliation Report");
        CrpReport.SetParameterValue("Rate", rblRate.SelectedItem.Text);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", reporType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
    }

    /// <summary>
    /// Shows Stock Reconciliation in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }
}