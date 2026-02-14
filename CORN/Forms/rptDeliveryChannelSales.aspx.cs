using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;

public partial class Forms_rptDeliveryChannelSales : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly OrderEntryController _orderEnterController = new OrderEntryController();
    readonly DistributorController _dController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            LoadDistributor();
            LoadDeliveryChannel();

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }


    private void LoadDistributor()
    {
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
       
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadDeliveryChannel()
    {
        DataTable dt = _dController.GetDeliveryChannel(Constants.IntNullValue);
        ddlDeliveryChannel.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(ddlDeliveryChannel, dt, 0, 1);

        if (dt.Rows.Count > 0)
        {
            ddlDeliveryChannel.SelectedIndex = 0;
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
        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;

        if (ddlReportType.SelectedItem.Value.ToString() == "1")
        {
            ds = _orderEnterController.GetChannelWiseSummaryReport(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),
                Convert.ToInt32(ddlDeliveryChannel.SelectedItem.Value));

            var crpReport = new CrpDeliveryChannelSaleSummary();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            crpReport.SetParameterValue("ReportName", "Delivery Channel Wise Summary Report");
            crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
            crpReport.SetParameterValue("Username", Session["UserName"].ToString());

            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", pReprotType);
        }
        else if (ddlReportType.SelectedItem.Value.ToString() == "2")
        {
            ds = _orderEnterController.GetChannelInvoiceWiseReport(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
               DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),
               Convert.ToInt32(ddlDeliveryChannel.SelectedItem.Value));

            var crpReport = new CrpDeliveryChannelInvoiceWise();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            crpReport.SetParameterValue("ReportName", "Delivery Channel Invoice Wise Report");
            crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
            crpReport.SetParameterValue("Username", Session["UserName"].ToString());

            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", pReprotType);
        }
        else if (ddlReportType.SelectedItem.Value.ToString() == "3")
        {
            ds = _orderEnterController.SelectInvoiceDetail(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),
                Convert.ToInt32(ddlDeliveryChannel.SelectedItem.Value));

            var crpReport = new CrpBillInvoiceWiseForDeliveryChannel();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            //crpReport.SetParameterValue("ReportName", "Delivery Channel Item Wise Report");
            crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            //crpReport.SetParameterValue("Username", Session["UserName"].ToString());

            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", pReprotType);
        }


        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}
