using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;

public partial class Forms_RptFranchiseSaleInvoiceFactorWise : Page
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
            //LoadDistributor();
            LoadBranchFranchise();

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
    }
    private void LoadBranchFranchise()
    {
        DistributorController DController = new DistributorController();
        drpFranchise.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 4);
        clsWebFormUtil.FillDxComboBoxList(drpFranchise, dt, "CUSTOMER_ID", "CUSTOMER_NAME");

        if (dt.Rows.Count > 0)
        {
            drpFranchise.SelectedIndex = 0;
        }
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
        CrpFranchiseInvoiceFactorWise CrpReport = new CrpFranchiseInvoiceFactorWise();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpFranchise.SelectedItem.Value.ToString()));
        DataSet ds = _franchiseController.SelectFranchiseReportByPriceLevel(int.Parse(drpFranchise.SelectedItem.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"),
            DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("ReportName", "Franchise Sale Invoice (Price Factor)");
        CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
        CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
        CrpReport.SetParameterValue("ContactNumber", dt.Rows[0]["CONTACT_NUMBER"].ToString());
        CrpReport.SetParameterValue("Address", dt.Rows[0]["ADDRESS1"].ToString());
        CrpReport.SetParameterValue("Email", dt.Rows[0]["ADDRESS2"].ToString());

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", pReprotType);



        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}
