using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_rptTimeCard : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly SaleForceController _SaleForceCtl = new SaleForceController();
    readonly SkuController _mSkuController = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
            LoadDepartment();
            LoadEmployee();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
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

    private void LoadDepartment()
    {
        SLASHCodesController mController = new SLASHCodesController();
        DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.Employee_Depoartment_Id, null, Constants.IntNullValue, bool.Parse("True"));
        clsWebFormUtil.FillListBox(cblDepartment, dt, "REF_ID", "SLASH_DESC");
        foreach(ListItem li in cblDepartment.Items)
        {
            li.Selected = true;
        }
    }

    private void LoadEmployee()
    {
        SaleForceController mController = new SaleForceController();
        System.Text.StringBuilder sbDepartment = new System.Text.StringBuilder();
        cbEmployee.Items.Clear();
        foreach (ListItem li in cblDepartment.Items)
        {
            if(li.Selected)
            {
                sbDepartment.Append(li.Value);
                sbDepartment.Append(",");
            }
        }
        DataTable dt = mController.GetDepartmentWiseEmployees(Convert.ToInt32(drpDistributor.Value),Convert.ToInt32(Session["CompanyId"]),Convert.ToInt32(Session["UserID"]),sbDepartment.ToString(),1);
        clsWebFormUtil.FillListBox(cbEmployee, dt, "USER_ID", "USER_NAME");
        foreach (ListItem li in cbEmployee.Items)
        {
            li.Selected = true;
        }
    }
    private void ShowReport(int reportType)
    {

        var firstDayOfMonth = new DateTime(Convert.ToDateTime(txtStartDate.Text).Year, Convert.ToDateTime(txtStartDate.Text).Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        System.Text.StringBuilder sbDepartment = new System.Text.StringBuilder();
        System.Text.StringBuilder sbEmployee = new System.Text.StringBuilder();
        foreach (ListItem li in cblDepartment.Items)
        {
            if (li.Selected)
            {
                sbDepartment.Append(li.Value);
                sbDepartment.Append(",");
            }
        }
        foreach (ListItem li in cbEmployee.Items)
        {
            if (li.Selected)
            {
                sbEmployee.Append(li.Value);
                sbEmployee.Append(",");
            }
        }

        DataSet ds = _SaleForceCtl.GetTimeCardData(int.Parse(drpDistributor.SelectedItem.Value.ToString()), sbDepartment.ToString(), sbEmployee.ToString(), firstDayOfMonth, lastDayOfMonth);

        var crpReport = new CrpTimeCard();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("todate", txtStartDate.Text);
        crpReport.SetParameterValue("ReportTitle", "Time Card Report");
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("PritedBy", Session["UserName"].ToString());
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

    protected void cbhAllDepartment_CheckedChanged(object sender, EventArgs e)
    {
        if(cbhAllDepartment.Checked)
        {
            foreach (ListItem li in cblDepartment.Items)
            {
                li.Selected = true;
            }
        }
        else
        {
            foreach (ListItem li in cblDepartment.Items)
            {
                li.Selected = false;
            }
        }
        LoadEmployee();
    }

    protected void cbAllEmployee_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAllEmployee.Checked)
        {
            foreach (ListItem li in cbEmployee.Items)
            {
                li.Selected = true;
            }
        }
        else
        {
            foreach (ListItem li in cbEmployee.Items)
            {
                li.Selected = false;
            }
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployee();
    }

    protected void cblDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployee();
    }
}