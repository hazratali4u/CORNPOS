using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Form For General Ledger Report
/// </summary>
public partial class Forms_rptGeneralLedger : System.Web.UI.Page
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
            this.LoadAccountDetail();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }
    private void LoadAccountDetail()
    {
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dtHead = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue, 1);
        clsWebFormUtil.FillDxComboBoxList(ddlAccountHead, dtHead, "ACCOUNT_CODE", "ACCOUNT_DETAIL", true);
        clsWebFormUtil.FillDxComboBoxList(ddlAccountHeadTo,dtHead, "ACCOUNT_CODE", "ACCOUNT_DETAIL", true);

        if (dtHead.Rows.Count > 0)
        {
            ddlAccountHead.SelectedIndex = 0;
            ddlAccountHeadTo.SelectedIndex = 0;
        }

        Session.Add("dtHead", dtHead);
    }
    private void showReport(int pReportType)
    {
        System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
        System.Text.StringBuilder sbDistributorNames = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");

                sbDistributorNames.Append(li.Text);
                sbDistributorNames.Append(",");
            }
        }
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        DataSet ds = null;
        DataControl dc = new DataControl();
        ds = RptAccountCtl.GeneralLedger_View(int.Parse(this.Session["UserId"].ToString()), Constants.LongNullValue, sbDistributorIDs.ToString(), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 0,ddlAccountHead.SelectedItem.Value.ToString(),ddlAccountHeadTo.SelectedItem.Value.ToString());        
        CrpLedgerView CrpReport = new CrpLedgerView();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", sbDistributorNames.ToString());        
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", pReportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
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