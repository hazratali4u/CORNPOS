using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;

public partial class Forms_RptFranchiseSalesSummary : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptInventoryController _rptInventoryController = new RptInventoryController();
    readonly DistributorController _dController = new DistributorController();
    readonly FranchiseSaleInvoiceController _franchiseController = new FranchiseSaleInvoiceController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }


    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 3);
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        LoadCustomers();
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }

    private void ShowReport(int pReprotType)
    {

        DocumentPrintController mController = new DocumentPrintController();
        CrpFranchiseSalesSummary CrpReport = new CrpFranchiseSalesSummary();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = _franchiseController.GetFranchiseSummary(int.Parse(drpDistributor.SelectedItem.Value.ToString()), long.Parse(ddlFranchise.SelectedItem.Value.ToString()),DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("ReportName", "Franchise Sales Summary");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("date",Convert.ToDateTime(txtStartDate.Text));
        CrpReport.SetParameterValue("fromDate", Convert.ToDateTime(txtEndDate.Text));
        CrpReport.SetParameterValue("user", Session["UserName"].ToString());
        CrpReport.SetParameterValue("location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("customer", ddlFranchise.SelectedItem.Text);

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", pReprotType);



        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomers();
    }
    private void LoadCustomers()
    {
        ddlFranchise.Items.Clear();
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 4);
        ddlFranchise.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.LongNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlFranchise, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlFranchise.SelectedIndex = 0;
        }
    }
}