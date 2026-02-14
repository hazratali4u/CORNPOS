using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Data;

public partial class Forms_RptCustomerLedger : System.Web.UI.Page
{
    public static string opType = "CR";

    readonly SkuController SKUCtl = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!IsPostBack)
        {
            this.LoadDistributor();
            LoadAllCustomers();

            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", true);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void LoadAllCustomers()
    {
        drpCustomer.Items.Clear();
        CustomerDataController mController = new CustomerDataController();
        try
        {
            DataTable dtCustomers = mController.SelectCustomerLocationWise(Convert.ToInt32(rblType.SelectedItem.Value),int.Parse(drpDistributor.SelectedItem.Value.ToString()));
            clsWebFormUtil.FillDxComboBoxList(drpCustomer, dtCustomers, 0, 1);
            if (dtCustomers.Rows.Count > 0)
            {
                drpCustomer.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            throw;
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
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAllCustomers();
    }

    private void ShowReport(int pType)
    {
        DataControl dc = new DataControl();
        CORNBusinessLayer.Classes.DocumentPrintController DPrint = new CORNBusinessLayer.Classes.DocumentPrintController();
        RptCustomerController RptCustomerCtl = new RptCustomerController();
        DataSet ds;
        ds = RptCustomerCtl.CustomerLedger_View(Constants.IntNullValue, drpDistributor.SelectedItem.Value.ToString(), int.Parse(drpCustomer.SelectedItem.Value.ToString()),DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),Convert.ToInt32(rblType.SelectedItem.Value));
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CORNBusinessLayer.Reports.CrpCustomerLedger CrpReport = new CORNBusinessLayer.Reports.CrpCustomerLedger();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        string customer = drpCustomer.SelectedItem.Text;
        CrpReport.SetParameterValue("custCode", "");
        CrpReport.SetParameterValue("Customer", customer);        
        CrpReport.SetParameterValue("Op_Type", pType);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        if (rblType.SelectedValue == "1")
        {
            CrpReport.SetParameterValue("Op_Balance", LoadCustomerOpBalance());
            CrpReport.SetParameterValue("CustomerOrFranchise", "Customer Ledger");
        }
        else
        {
            CrpReport.SetParameterValue("Op_Balance", LoadCustomerOpBalanceFranchise());
            CrpReport.SetParameterValue("CustomerOrFranchise", "Franchise Ledger");
        }
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", pType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    private decimal LoadCustomerOpBalance()
    {
        if (drpDistributor.Items.Count > 0 && drpCustomer.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dt = mController.SelectPrincipalCustomerOpBalance(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpCustomer.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), Constants.IntNullValue);
            if (Convert.ToDecimal(dt.Rows[0][0]) < 0)
            {
                opType = "DR";
            }
            else if (Convert.ToDecimal(dt.Rows[0][0]) == 0)
            {
                opType = "";
            }
            else
            {
                opType = "CR";
            }
            return decimal.Parse(dt.Rows[0][0].ToString());
        }
        return 0;
    }
    private decimal LoadCustomerOpBalanceFranchise()
    {
        if (drpDistributor.Items.Count > 0 && drpCustomer.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dt = mController.SelectPrincipalCustomerOpBalanceFranchise(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpCustomer.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), Constants.IntNullValue);
            if (Convert.ToDecimal(dt.Rows[0][0]) < 0)
            {
                opType = "DR";
            }
            else if (Convert.ToDecimal(dt.Rows[0][0]) == 0)
            {
                opType = "";
            }
            else
            {
                opType = "CR";
            }
            return decimal.Parse(dt.Rows[0][0].ToString());
        }
        return 0;
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAllCustomers();
    }
}