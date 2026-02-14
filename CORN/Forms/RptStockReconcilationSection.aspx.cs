using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;
using System.Web.UI.WebControls;
/// <summary>
/// Form For Stock Reconciliation Report
/// </summary>
public partial class Forms_RptStockReconcilationSection : System.Web.UI.Page
{
    readonly SkuController _mSkuController = new SkuController();
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

        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadSection()
    {
        DataTable dt = _mSkuController.SelectProductSection(0, null, null);
        clsWebFormUtil.FillListBox(cblSection, dt, "SECTION_ID", "SECTION_NAME");
        foreach(ListItem li in cblSection.Items)
        {
            li.Selected = true;
        }
    }

    private void showReport(int reporType)
    {
        string SectionIDs = null;
        int Type = 1;
        if(cbInActive.Checked)
        {
            Type = 2;
        }

        foreach (ListItem li in cblSection.Items)
        {
            if (li.Selected)
            {
                SectionIDs += li.Value.ToString() + ",";
            }
        }

        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = RptInventoryCtl.SelectPrincipalStockReconcilation(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), int.Parse(this.Session["UserId"].ToString()), Type, 0,SectionIDs);
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpStockReconsiliationSection();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("division", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("fromdate", txtStartDate.Text);
        CrpReport.SetParameterValue("todate", txtEndDate.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("Price", "Trade Price");
        CrpReport.SetParameterValue("ReportType", "Section Wise Stock Reconciliation");
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