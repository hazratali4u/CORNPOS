using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

/// <summary>
/// Form For General Ledger Report
/// </summary>
public partial class Forms_rptSupplierClosingSummary : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadPrincipal();
            LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    private void LoadPrincipal()
    {
        if (Session["FranchiseModule"].ToString() == "1")
        {
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            DrpPrincipal.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(DrpPrincipal, dtVendors, "VendorID", "VendorName");
        }
        else
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
            DrpPrincipal.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(this.DrpPrincipal, m_dt, 0, 1);
        }
        if (DrpPrincipal.Items.Count > 0)
        {
            DrpPrincipal.SelectedIndex = 0;
        }
        else
        {
            DrpPrincipal.SelectedIndex = -1;
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
    private void showReport(int reportType)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        VenderEntryController RptCustCtl = new VenderEntryController();
        DataSet ds = null;
        string VendorID = null;
        DataTable dtVendors = (DataTable)Session["dtVendors"];
        if (Session["FranchiseModule"].ToString() == "1")
        {
            if (DrpPrincipal.SelectedItem.Value.ToString() == Constants.IntNullValue.ToString())
            {
                
            }
            else
            {                
                foreach (DataRow dr in dtVendors.Rows)
                {
                    if (dr["VendorID"].ToString() == DrpPrincipal.SelectedItem.Value.ToString())
                    {
                        VendorID = dr["SupplierLocationID"].ToString();                        
                        break;
                    }
                }
            }
        }
        if (DrpPrincipal.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in DrpPrincipal.Items)
            {
                if (Session["FranchiseModule"].ToString() == "1")
                {
                    foreach (DataRow dr in dtVendors.Rows)
                    {
                        if (dr["VendorID"].ToString() == li.Value.ToString())
                        {
                            if (li.Value.ToString() != Constants.IntNullValue.ToString())
                            {
                                VendorID += dr["SupplierLocationID"].ToString();
                                VendorID += ",";
                            }
                        }
                    }
                }
                else
                {
                    VendorID += li.Value.ToString();
                    VendorID += ",";
                }
            }
        }
        else
        {
            VendorID = DrpPrincipal.Value.ToString();
        }

        System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
        if(drpDistributor.SelectedItem.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach(DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                if(li.Value.ToString() != Constants.IntNullValue.ToString())
                {
                    sbDistributorIDs.Append(li.Value.ToString());
                    sbDistributorIDs.Append(",");
                }
            }
        }
        else
        {
            sbDistributorIDs.Append(drpDistributor.SelectedItem.Value.ToString());
        }
        ds = RptCustCtl.GetSupplierClosingSummary(VendorID, sbDistributorIDs.ToString(), DateTime.Parse(txtEndDate.Text + " 23:59:59"), Convert.ToInt32(ddlClosingType.SelectedItem.Value));
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CrpVendorLedgerSummary CrpReport = new CrpVendorLedgerSummary();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("AsOnDate", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", reportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
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