using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form For  Trial Balance Report
/// </summary>
public partial class Forms_RptFinancialRpt : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {            
            this.LoadDistributor();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
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
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        //drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }

    }

    private void ShowReport(int pReportType)
    {
        DocumentPrintController dPrint = new DocumentPrintController();
        RptAccountController rptAccountCtl = new RptAccountController();
        DataSet ds = rptAccountCtl.FinancialRptCocoRush(int.Parse(drpDistributor.SelectedItem.Value.ToString()), 
            DateTime.Parse(txtStartDate.Text + " 00:00:00"),
            DateTime.Parse(txtEndDate.Text + " 23:59:59"),
            Convert.ToInt32(Session["UserID"]));

        DataTable dt = dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CrystalDecisions.CrystalReports.Engine.ReportDocument CrpReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        CrpReport = new CORNBusinessLayer.Reports.CrpFinancialRpt();

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("From_Date", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", pReportType);
        const string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
    /// <summary>
    /// Shows Trial Balance in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
}