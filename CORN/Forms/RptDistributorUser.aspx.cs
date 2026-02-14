using System;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Data;
using CrystalDecisions.Shared;


public partial class Forms_RptDistributorUser : System.Web.UI.Page
{

    Distributor_UserController mDUController = new Distributor_UserController(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            this.LoadDepartment();
            this.LoadDesignation();
            LoadEmployee();
        }
    }

    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }

    }

    private void LoadDesignation()
    {
        SLASHCodesController mController = new SLASHCodesController();
        try
        {
            DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.SaleForce, null, Constants.IntNullValue, true);

            ddlDesignation.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddlDesignation, dt, "REF_ID", "SLASH_DESC");
            if (dt.Rows.Count > 0)
            {
                ddlDesignation.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadDepartment()
    {
        SLASHCodesController mController = new SLASHCodesController();
        try
        {
            DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.Employee_Depoartment_Id, null, Constants.IntNullValue, true);

            ddlDepartment.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddlDepartment, dt, "REF_ID", "SLASH_DESC");
            if (dt.Rows.Count > 0)
            {
                ddlDepartment.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadEmployee()
    {
        drpEmployee.Items.Clear();
        DataTable dt = null;
        dt = mDUController.GetEmployee(Convert.ToInt32(drpStatus.SelectedItem.Value), int.Parse(drpDistributor.SelectedItem.Value.ToString()), Convert.ToInt32(ddlDesignation.SelectedItem.Value),Convert.ToInt32(ddlDepartment.SelectedItem.Value),1);
        
        drpEmployee.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpEmployee, dt, "USER_ID", "USER_NAME");
        if (dt.Rows.Count > 0)
        {
            drpEmployee.SelectedIndex = 0;
        }
    }
    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployee();
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployee();
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows Route Wise Customer List in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
    protected void ShowReport(int ReportType)
    {
        DocumentPrintController DPrint = new DocumentPrintController();

        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CORNBusinessLayer.Reports.CrpUserList CrpReport = new CORNBusinessLayer.Reports.CrpUserList();
        DataSet ds = null;
        ds = mDUController.GetEmployee(Convert.ToInt32(drpStatus.SelectedItem.Value), int.Parse(drpDistributor.SelectedItem.Value.ToString()), Convert.ToInt32(ddlDesignation.SelectedItem.Value), Convert.ToInt32(ddlDepartment.SelectedItem.Value), 2,Convert.ToInt32(drpEmployee.SelectedItem.Value));
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("DISTRIBUTOR_NAME", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Status", drpStatus.SelectedItem.Text);
        CrpReport.SetParameterValue("Department", ddlDepartment.SelectedItem.Text);
        CrpReport.SetParameterValue("Designation", ddlDesignation.SelectedItem.Text);
        CrpReport.SetParameterValue("PrintedBy", Session["UserName"].ToString());
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", ReportType);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployee();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployee();
    }
}