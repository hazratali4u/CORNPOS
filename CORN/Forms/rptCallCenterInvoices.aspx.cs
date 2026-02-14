using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;

public partial class Forms_rptCallCenterInvoices : Page
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
            LoadAllCustomers();
            LoadRider();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    private void LoadAllCustomers()
    {
        ddlCustomer.Items.Clear();
        CustomerDataController mController = new CustomerDataController();
        try
        {
            DataTable dtCustomers = mController.SelectCustomerLocationWise(3, int.Parse(drpDistributor.SelectedItem.Value.ToString()));
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

    private void LoadDistributor()
    {
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All",Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadRider()
    {
        try
        {
            ddlRider.Items.Clear();
            SaleForceController mDController = new SaleForceController();
            DataTable dt = mDController.SelectSaleForceAssignedArea(Constants.SALES_FORCE_DELIVERYMAN, Convert.ToInt32(drpDistributor.Value), Constants.IntNullValue, int.Parse(HttpContext.Current.Session["companyId"].ToString()), Constants.IntNullValue);
            ddlRider.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddlRider, dt, "USER_ID", "USER_NAME");
            ddlRider.SelectedIndex = 0;
        }
        catch (Exception)
        {
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
        System.Text.StringBuilder sbDistributorID = new System.Text.StringBuilder();
         if(drpDistributor.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach(DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                sbDistributorID.Append(li.Value);
                sbDistributorID.Append(",");
            }
        }
        else
        {
            sbDistributorID.Append(drpDistributor.Value.ToString());
        }
        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;
        RptSaleController rptSale = new RptSaleController();
        ds = rptSale.GetCallCenterInvoices(sbDistributorID.ToString(),DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),Convert.ToInt32(ddlMOP.Value),Convert.ToInt64(ddlCustomer.Value),Convert.ToInt32(ddlRider.Value));

        var crpReport = new CrpCallCenterInvoices();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("ReportName", "Call Center Invoices Report");        
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
        LoadRider();
    }
}