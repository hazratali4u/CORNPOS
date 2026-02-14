using System;
using System.Web;
using System.Web.UI;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;

public partial class Forms_rptStockValuation : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            LoadCatagory();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            // chbAllSections.Checked = true;
            if (chbAllSections.Checked == true)
            {
                for (int i = 0; i < chblSection.Items.Count; i++)
                {
                    chblSection.Items[i].Selected = true;
                }
            }
        }
    }
    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>


    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    protected void LoadCatagory()
    {
        var hierarchy = new SkuHierarchyController();
        DataTable dt = hierarchy.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()), Constants.IntNullValue);
        clsWebFormUtil.FillListBox(chblSection, dt, 0, 3);
        if (dt.Rows.Count > 0)
        {
            chblSection.SelectedIndex = 0;
        }
    }
    private void showReport(int reporType)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        string catids = "";
        for (int i = 0; i < chblSection.Items.Count; i++)
        {
            if (chblSection.Items[i].Selected == true)
            {
                catids = catids + chblSection.Items[i].Value + ",";
            }
        }
        int IsLocationWiseItem = 0;
        if (Session["IsLocationWiseItem"].ToString() == "1")
        {
            IsLocationWiseItem = 1;
        }
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = RptInventoryCtl.GetStockValuation2(int.Parse(drpDistributor.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text), catids, Convert.ToInt32(rblRate.SelectedValue), IsLocationWiseItem);
        CrpStockValuation CrpReport = new CrpStockValuation();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();


        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Date", txtStartDate.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("user", Session["UserName"].ToString());
        CrpReport.SetParameterValue("PriceLable", rblRate.SelectedItem.Text);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", reporType);
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

    /// <summary>
    /// Shows Stock Reconciliation in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }
}