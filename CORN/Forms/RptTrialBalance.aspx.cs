using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;

/// <summary>
/// Form For  Trial Balance Report
/// </summary>
public partial class Forms_RptTrialBalance : System.Web.UI.Page
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
            this.LoadAccount();
            this.LoadAccountDetail();
            this.LoadMinMixAccountCode();
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
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }

    /// <summary>
    /// Loads Main Account Heads To Main Account Combo
    /// </summary>
    private void LoadAccount()
    {
        AccountHeadController mController = new AccountHeadController();
        DataTable dt = mController.SelectAccountHead(Constants.AC_MainTypeId, Constants.LongNullValue, 1);

        DrpMainAccount.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(DrpMainAccount, dt, 0, 10);

        if (dt.Rows.Count > 0)
        {
            DrpMainAccount.SelectedIndex = 0;
        }
    }

    private void LoadMinMixAccountCode()
    {
        AccountHeadController mController = new AccountHeadController();
        DataTable dt = mController.GetMinMaxAccountCode(Constants.AC_AccountHeadId);
        if (dt.Rows.Count > 0)
        {
            ddlFromAccount.Value = dt.Rows[0]["FromAccountCode"].ToString();
            ddlToAccount.Value = dt.Rows[0]["ToAccountCode"].ToString();
        }
    }

    /// <summary>
    /// Loads Account Heads To Account ListBox
    /// </summary>
    private void LoadAccountDetail()
    {
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dtHead = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue, 1);
        DataView dv = new DataView(dtHead);
        dv.Sort = "ACCOUNT_DETAIL";
        dtHead = dv.ToTable();

        clsWebFormUtil.FillDxComboBoxList(ddlFromAccount, dtHead, "ACCOUNT_CODE", "ACCOUNT_DETAIL");
        clsWebFormUtil.FillDxComboBoxList(ddlToAccount, dtHead, "ACCOUNT_CODE", "ACCOUNT_DETAIL");
        if (dtHead.Rows.Count > 0)
        {
            ddlFromAccount.SelectedIndex = 0;
            ddlToAccount.SelectedIndex = 0;
        }
        else
        {
            ddlFromAccount.SelectedIndex = -1;
            ddlToAccount.SelectedIndex = -1;
        }
        this.Session.Add("dtHead", dtHead);
    }

    private void ShowReport(int pReportType)
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


        DocumentPrintController dPrint = new DocumentPrintController();
        RptAccountController rptAccountCtl = new RptAccountController();
        DataSet ds = rptAccountCtl.TrialBalance(Constants.IntNullValue, sbDistributorIDs.ToString(), int.Parse(DrpMainAccount.SelectedItem.Value.ToString()),DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(DrpLevel.SelectedItem.Value.ToString()), ddlFromAccount.Value.ToString(), ddlToAccount.Value.ToString(), Constants.IntNullValue,Convert.ToInt32(Session["UserID"]));
        DataTable dt = dPrint.SelectReportTitle(Constants.IntNullValue);
        CrystalDecisions.CrystalReports.Engine.ReportDocument CrpReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        if(ddlReportType.SelectedItem.Value.ToString() == "1")
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpTrialBalance();
        }
        else
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpTrialBalanceSummary();
        }
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("From_Date", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", sbDistributorNames.ToString());        
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