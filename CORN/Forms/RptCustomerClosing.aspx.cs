using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

public partial class Forms_RptCustomerClosing : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly SkuController _orderEntryController = new SkuController();
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
            LoadCreditCustomer();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
        }
    }


    private void LoadDistributor()
    {
        drpDistributor.DataSource = null;
        drpDistributor.DataBind();

        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void LoadCreditCustomer()
    {
        chbAllCustomer.Checked = true;
        int TypeID = 6;
        if(rbCustomerType.SelectedItem.Value == "2")
        {
            TypeID = 8;
        }
        DrpCustomer.Items.Clear();
        string selectedLocation = "";
        foreach (ListItem item in drpDistributor.Items)
        {
            if (item.Selected == true)
            {
                selectedLocation += item.Value + ",";
            }
        }
        CustomerDataController _ledgerCtl = new CustomerDataController();
        DataTable dt = _ledgerCtl.SelectCreditCustomer(selectedLocation, TypeID);
        clsWebFormUtil.FillListBox(DrpCustomer, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
        foreach(ListItem li in DrpCustomer.Items)
        {
            li.Selected = true;
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
        string d_id = null;
        string c_id = null;
        int _value = 0;
        long _customervalue = 0;
        int count = 0;
        int customerCount = 0;
        string locationName = "";
        string customerName = "";
        for (int i = 0; i < drpDistributor.Items.Count; i++)
        {
            if (drpDistributor.Items[i].Selected == true)
            {
                _value = Convert.ToInt32(drpDistributor.Items[i].Value.ToString());
                d_id += _value + ",";
                locationName = locationName + drpDistributor.Items[i].Text + ", ";
                count++;
            }
        }

        for (int i = 0; i < DrpCustomer.Items.Count; i++)
        {
            if (DrpCustomer.Items[i].Selected == true)
            {
                _customervalue = Convert.ToInt64(DrpCustomer.Items[i].Value.ToString());
                c_id += _customervalue + ",";
                customerName = customerName + DrpCustomer.Items[i].Text + ", ";
                customerCount++;
            }
        }

        if (count == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Location');", true);
            return;
        }

        if (customerCount == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Customer');", true);
            return;
        }

        locationName = locationName.Trim().TrimEnd(',');
        customerName = customerName.Trim().TrimEnd(',');

        DataTable dt = _dPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
        DataSet ds;
        int TypeID = 7;
        if (rbCustomerType.SelectedItem.Value == "2")
        {
            TypeID = 9;
        }
        LedgerController CDC = new LedgerController();
        ds = CDC.SelectCreditPendingInvoice(d_id, c_id, TypeID, Convert.ToDateTime(txtStartDate.Text));

        var crpReport = new ReportDocument();
        string ReportName = "Customer Credit Receivable (Invoice Wise)";
        if (RbReportType.SelectedIndex == 0)
        {
            crpReport = new CrpCustomerCreditSummary2();
            if (rbCustomerType.SelectedItem.Value == "1")
            {
                ReportName = "Customer Credit Receivable";
            }
            else
            {
                ReportName = "Franchise Credit Receivable";
            }
        }
        else
        {
            crpReport = new CrpCustomerCreditSummaryDetail();
            if(rbCustomerType.SelectedItem.Value == "1")
            {
                ReportName = "Customer Credit Receivable (Invoice Wise)";
            }
            else
            {
                ReportName = "Franchise Credit Receivable (Invoice Wise)";
            }
        }
        
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Distributor", locationName);
        crpReport.SetParameterValue("To_Date", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Username", Session["UserName"].ToString());
        crpReport.SetParameterValue("ReportType", ReportName);

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
        if (rbCustomerType.SelectedItem.Value == "1")
        {
            LoadCreditCustomer();
        }
    }

    protected void chbAllLocation_CheckedChanged(object sender, EventArgs e)
    {
        LoadCreditCustomer();
    }

    protected void rbCustomerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCreditCustomer();
    }
}