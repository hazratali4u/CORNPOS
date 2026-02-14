using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;

/// <summary>
/// Form For SKU Price List Report
/// </summary>
public partial class Forms_rptPriceList : System.Web.UI.Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (Page.IsPostBack) return;
        LoadDistributor();
        LoadCatagory();
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        var dController = new DistributorController();
        DataTable dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(this.ChbDistributorList, dt, 0, 2, true);
        if(dt.Rows.Count > 0)
        {
            ChbSelectAll.Checked = true;
            foreach(ListItem item in ChbDistributorList.Items)
            {
                item.Selected = true;
            }
        }
    }

    /// <summary>
    /// Loads SKU Categories To Category Combo
    /// </summary>
    protected void LoadCatagory()
    {
        var hierarchy = new SkuHierarchyController();
        DataTable dt = hierarchy.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, 1, null, null, true, 19, Constants.IntNullValue);
        DrpCatagory.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(DrpCatagory, dt, 0, 3);
        if (dt.Rows.Count > 0)
        {
            DrpCatagory.SelectedIndex = 0;
        }
    }
    /// <summary>
    /// Shows SKU Price List in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows SKU Price List in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
    private void ShowReport(int ReportType)
    {
        try
        {
            System.Text.StringBuilder sbLocation = new System.Text.StringBuilder();
            System.Text.StringBuilder sbLocationName = new System.Text.StringBuilder();
            foreach (ListItem item in ChbDistributorList.Items)
            {
                if (item.Selected == true)
                {
                    sbLocation.Append(item.Value);
                    sbLocation.Append(",");
                    sbLocationName.Append(item.Text);
                    sbLocationName.Append(",");
                }
            }

            var status = int.Parse(drpStatus.Value.ToString());
            if (status == 1)
                status = Constants.IntNullValue;

            DataTable dt = _dPrint.SelectReportTitle(Constants.IntNullValue);            
            var crpReport = new crpPriceList3();
            DataSet ds = null;
            ds = _rptSaleCtl.PriceList(Constants.IntNullValue,int.Parse(DrpCatagory.SelectedItem.Value.ToString()),sbLocation.ToString(), status,Convert.ToInt32(rblRate.SelectedItem.Value));

            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Branch", sbLocationName.ToString().TrimEnd(','));
            crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            crpReport.SetParameterValue("PrintedBy", Session["UserName"].ToString());
            crpReport.SetParameterValue("Status", drpStatus.Text);
            crpReport.SetParameterValue("PriceFormula", "Cost Price on " + rblRate.SelectedItem.Text);
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", ReportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
}