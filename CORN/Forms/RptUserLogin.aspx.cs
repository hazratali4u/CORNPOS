using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form For User Login History Report
/// </summary>
public partial class Forms_RptUserLogin : System.Web.UI.Page
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
            txtStartDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtEndDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            LoadDistributor();
            LoadUsers();

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        drpDistributor.DataSource = null;
        drpDistributor.DataBind();

        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    /// <summary>
    /// Loads Users To User Combo
    /// </summary>
    private void LoadUsers()
    {
        ddlUser.DataSource = null;
        ddlUser.DataBind();
        UserController UserController = new UserController();
        DataTable dt = UserController.SelectAllUser(1, int.Parse(drpDistributor.Value.ToString()));        
        ddlUser.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlUser, dt, 0, 1);
        if (dt.Rows.Count > 0)
        {
            ddlUser.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Shows User Login History in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows User Login History in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
    private void ShowReport(int ReportType)
    {
        System.Text.StringBuilder sbUser = new System.Text.StringBuilder();
        if (ddlUser.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in ddlUser.Items)
            {
                sbUser.Append(li.Value);
                sbUser.Append(",");
            }
        }
        else
        {
            sbUser.Append(ddlUser.Value);
        }
        DocumentPrintController mController = new DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();
        CORNBusinessLayer.Reports.CrpUserLoginDetail CrpReport = new CORNBusinessLayer.Reports.CrpUserLoginDetail();
        DataTable dt = mController.SelectReportTitle(Constants.IntNullValue);
        DataSet ds = RptSaleCtl.GetUserLoginDetail(DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), sbUser.ToString(), Convert.ToInt64(Session["User_Log_ID"]));
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("FromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("ToDate", txtEndDate.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("ReportFor", ddlUser.SelectedItem.Text);
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", ReportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUsers();
    }
}