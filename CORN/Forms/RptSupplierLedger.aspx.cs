using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using CrystalDecisions.CrystalReports.Engine;

/// <summary>
/// Form For General Ledger Report
/// </summary>
public partial class Forms_RptLedgerReport2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadPrincipal();
            LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
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
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        
        if (drpDistributor.SelectedItem.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                sbLocationIDs.Append(li.Value);
                sbLocationIDs.Append(",");
            }
        }
        else
        {
            sbLocationIDs.Append(drpDistributor.SelectedItem.Value);
        }
        
        int TypeID = 1;
        System.Text.StringBuilder sbVendorID = new System.Text.StringBuilder();
        if (DrpPrincipal.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in DrpPrincipal.Items)
            {
                sbVendorID.Append(li.Value);
                sbVendorID.Append(",");
            }
        }
        else
        {
            sbVendorID.Append(DrpPrincipal.Value.ToString());
            sbVendorID.Append(",");
        }
        if (Session["FranchiseModule"].ToString() == "1")
        {
            if (DrpPrincipal.SelectedItem.Value.ToString() == Constants.IntNullValue.ToString())
            {
                TypeID = 3;
            }
            else
            {
                DataTable dtVendors = (DataTable)Session["dtVendors"];
                foreach (DataRow dr in dtVendors.Rows)
                {
                    if (dr["VendorID"].ToString() == DrpPrincipal.SelectedItem.Value.ToString())
                    {
                        sbVendorID.Append(dr["SupplierLocationID"].ToString());
                        if (dr["VendorType"].ToString() == "1")
                        {
                            TypeID = 1;
                        }
                        else
                        {
                            TypeID = 2;
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            TypeID = 1;
        }
        if (RbReportType.SelectedValue == "2")
        {
            TypeID = 4;
        }
        ds = RptCustCtl.GetSupplierAgeing(sbVendorID.ToString(), sbLocationIDs.ToString(), DateTime.Parse(txtStartDate.Text + " 00:00:00"), TypeID);
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CrpSupplierAgeing();

        if (RbReportType.SelectedValue == "2") {
            CrpReport = new CrpSupplierAgeingDueDateWise();
        }

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("AsOnDate", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
        CrpReport.SetParameterValue("PrintedBy", Session["UserName"].ToString());
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