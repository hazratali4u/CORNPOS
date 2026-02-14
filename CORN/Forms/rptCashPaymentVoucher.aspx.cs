using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
/// <summary>
/// Form For Voucher View Report
/// </summary>
public partial class Forms_rptCashPaymentVoucher : System.Web.UI.Page
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

        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadAccountDetail()
    {
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dtHead = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue, 1);
        clsWebFormUtil.FillDxComboBoxList(ddlAccountHead, dtHead, "ACCOUNT_HEAD_ID", "ACCOUNT_DETAIL", true);

        if (dtHead.Rows.Count > 0)
        {
            ddlAccountHead.SelectedIndex = 0;
        }

        Session.Add("dtHead", dtHead);
    }


    private void showReport(int pReportType)
    {
        RptAccountController RptAccountCtl = new RptAccountController();

        DataSet ds = RptAccountCtl.GetCashPaymentVoucher(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"),
            DateTime.Parse(txtEndDate.Text + " 23:59:59"),
            long.Parse(ddlAccountHead.SelectedItem.Value.ToString()), 24);

        CrpCashPaymentVoucher CrpReport = new CrpCashPaymentVoucher();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("From_ACCOUNT", ddlAccountHead.SelectedItem.Text);
        CrpReport.SetParameterValue("Username", Session["UserName"].ToString());

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