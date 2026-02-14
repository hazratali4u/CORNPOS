using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_RptCashRegisterClosing : System.Web.UI.Page
{
    readonly RptInventoryController _InvCtl = new RptInventoryController();
    readonly SkuController _skuController = new SkuController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    readonly DistributorController _dController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
            LoadUser();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        drpDistributor.Items.Clear();
        drpDistributor.Items.Add("All", Constants.IntNullValue);

        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue,
            int.Parse(Session["UserId"].ToString()),
            int.Parse(Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadUser()
    {
        if (drpDistributor.Items.Count > 0)
        {
            Distributor_UserController du = new Distributor_UserController();
            DataTable dt = du.SelectDistributorUser(7, int.Parse(drpDistributor.Value.ToString()), 
                int.Parse(Session["CompanyId"].ToString()), int.Parse(Session["UserId"].ToString()));
            clsWebFormUtil.FillDxComboBoxList(drpUser, dt, 0, 1, true);
            if (dt.Rows.Count > 0)
            {
                drpUser.SelectedIndex = 0;
            }
        }
    }

    private void ShowReport(int reportType)
    {
        RptSaleController _rptPurchaseController = new RptSaleController();
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        
        if(drpDistributor.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                if (li.Index != 0)
                {
                    sbLocationIDs.Append(li.Value.ToString());
                    sbLocationIDs.Append(",");
                }
            }
        }
        else
        {
            sbLocationIDs.Append(drpDistributor.Value.ToString());
        }

        DataSet ds = _rptPurchaseController.GetNetCashSalevsCashSubmitted(sbLocationIDs.ToString(),
            int.Parse(drpUser.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        var crpReport = new CrpCashSalevsCashSubmitted();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        //crpReport.SetParameterValue("FromDate", txtStartDate.Text);
        //crpReport.SetParameterValue("ToDate", txtEndDate.Text);
        crpReport.SetParameterValue("LOCATION", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("ReportType", "Net Cash Sales vs Cash Deposited");
        crpReport.SetParameterValue("PritedBy", Session["UserName"].ToString());
        crpReport.SetParameterValue("Cashier", drpUser.SelectedItem.Text);
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

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUser();
    }
}