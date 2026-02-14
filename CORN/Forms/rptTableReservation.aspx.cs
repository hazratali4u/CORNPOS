using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;

public partial class Forms_rptTableReservation : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptSaleController _saleController = new RptSaleController();
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
            LoadAllCustomers();
            LoadBookingSlot();
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

    private void LoadAllCustomers()
    {
        ddlCustomer.Items.Clear();
        CustomerDataController mController = new CustomerDataController();
        try
        {
            DataTable dtCustomers = mController.SelectCustomerLocationWise(1, int.Parse(drpDistributor.SelectedItem.Value.ToString()));
            ddlCustomer.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.LongNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dtCustomers, "CUSTOMER_ID", "CUSTOMER_NAME");
            if (dtCustomers.Rows.Count > 0)
            {
                ddlCustomer.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void LoadBookingSlot()
    {
        var mTableRes = new TableReservationController();
        DataTable dtTables = mTableRes.GetTimeSlot(2,Convert.ToInt32(drpDistributor.Value));

        ddlBookingSlot.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlBookingSlot, dtTables, "BookingSlotID", "TimeSlot");
        if (dtTables.Rows.Count > 0)
        {
            ddlBookingSlot.SelectedIndex = 0;
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
        DataSet ds = _saleController.GetTableReservationReport(int.Parse(drpDistributor.SelectedItem.Value.ToString()),Convert.ToInt64(ddlCustomer.Value), Convert.ToInt32(ddlBookingSlot.Value), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));
        var crpReport = new CrpTableReservation();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("ReportName", "Table Reservation Report");
        crpReport.SetParameterValue("Username", Session["UserName"].ToString());
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", pReprotType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAllCustomers();
        LoadBookingSlot();
    }
}