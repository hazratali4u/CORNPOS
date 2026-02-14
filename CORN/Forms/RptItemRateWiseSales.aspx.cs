using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_RptItemRateWiseSales : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
           
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");

            LoadItemName();
        }
    }
    private void LoadItemName()
    {
        SkuController SKUCtl = new SkuController();
        DataTable Dtsku_Price = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 32, int.Parse(Session["CompanyId"].ToString()), null);
        drpItemName.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(this.drpItemName, Dtsku_Price, "SKU_ID", "SKU_NAME");
        drpItemName.SelectedIndex = 0;
    }
    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        DistributorController _dController = new DistributorController();
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void ShowReport(int reportType)
    {

        DataTable dt = _mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));

        DataSet ds = _rptSaleCtl.SelectDateWiseSaleReport(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            int.Parse(Session["UserID"].ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),
            int.Parse(drpItemName.Value.ToString()), 1);

        CrpItemRateWiseSale crpReport = new CrpItemRateWiseSale();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();

        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("ReportName", "Item Rate Wise Sale Report");
        crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Username", Session["UserName"].ToString());

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
}