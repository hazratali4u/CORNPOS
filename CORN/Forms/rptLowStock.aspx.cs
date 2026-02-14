using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using System.Web.UI.WebControls;
/// <summary>
/// Form For Route Wise Customer List Report
/// </summary>
public partial class Forms_rptLowStock : System.Web.UI.Page
{
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadCategory();
        }
    }
    
    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        DataTable dt = mController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()),1);        
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        Session.Add("dtLocation", dt);
    }
    private void LoadCategory()
    {
        try
        {
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 20, Constants.IntNullValue);
            clsWebFormUtil.FillListBox(cblCategory, _mDt, 0, 3, true);
            foreach (ListItem li in cblCategory.Items)
            {
                li.Selected = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
    private void showReport(int reportType)
    {
        DataTable dtLocation = (DataTable)Session["dtLocation"];
        DateTime dtClosingDaate = Constants.DateNullValue;
        foreach(DataRow dr in dtLocation.Rows)
        {
            if(dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.SelectedItem.Value.ToString())
            {
                dtClosingDaate = Convert.ToDateTime(dr["MaxDayClose"]);
            }
        }
        
        DocumentPrintController DPrint = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CrpLowStock CrpReport = new CrpLowStock();
        DataSet ds = null;
        string CategoryIDs = "";
        foreach(ListItem li in cblCategory.Items)
        {
            if(li.Selected)
            {
                CategoryIDs += li.Value + ",";
            }
        }
        int Type = 2;
        if(cbLowStock.Checked)
        {
            Type = 1;
        }
        ds = RptInventoryCtl.GetLowStock(int.Parse(drpDistributor.SelectedItem.Value.ToString()),dtClosingDaate,Type, CategoryIDs);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("ReportName", "Low Stock Alert");
        CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        CrpReport.SetParameterValue("location", drpDistributor.SelectedItem.Text);
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