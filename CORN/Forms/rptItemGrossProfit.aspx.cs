using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_rptItemGrossProfit : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptAccountController _rptAccCtl = new RptAccountController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
            LoadCategory();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        drpDistributor.Items.Clear();
        var mUserController = new UserController();
        DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 1);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadCategory()
    {
        try
        {
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, 1, null, null, true, 19, Constants.IntNullValue);
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
    private void ShowReport(int reportType)
    {
        string CategoryIDs = null;
        foreach(ListItem li in cblCategory.Items)
        {
            if(li.Selected)
            {
                CategoryIDs += li.Value;
                CategoryIDs += ",";
            }
        }
        DataSet ds = _rptAccCtl.GetGrossProfitItemWise(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Convert.ToInt32(Session["UserID"]),CategoryIDs, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));

        var crpReport = new CrpGrossProfitItemWise();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("ReportName", "Item Wise Gross Profit Report");
        crpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        crpReport.SetParameterValue("location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("From_Date", txtStartDate.Text);
        crpReport.SetParameterValue("To_date", txtEndDate.Text);

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", reportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {

        ShowReport(1);
    }
}