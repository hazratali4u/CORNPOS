using System;
using System.Web;
using System.Web.UI;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

public partial class Forms_rptItemLedger : System.Web.UI.Page
{
    readonly SkuController SKUCtl = new SkuController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");

            LoadDistributor();
            LoadSKU();
        }

    }
    private void LoadSKU()
    {
        DataTable dt = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 24, int.Parse(Session["CompanyId"].ToString()), 2);
        if (dt.Rows.Count > 0)
        {
            clsWebFormUtil.FillListBox(cblItem, dt, 0, 1);
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = true;
            }
        }
    }

    private void showReport(int Type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        ReportDocument CrpReport = new CORNBusinessLayer.Reports.CrpSKULedger();
        string location = string.Empty;
        string locationName = string.Empty;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if(li.Selected)
            {
                location += li.Value;
                location += ",";

                locationName += li.Text;
                locationName += ",";
            }
        }
        string skuIds = "";
        foreach(ListItem li in cblItem.Items)
        {
            if(li.Selected)
            {
                skuIds += li.Value;
                skuIds += ",";
            }
        }
        DataTable dt = mController.SelectReportTitle(Constants.IntNullValue);
        DataSet ds = null;
        ds = RptInventoryCtl.GetSKULedgerData(Convert.ToDateTime(txtStartDate.Text),Convert.ToDateTime(txtEndDate.Text + " 23:59:59"), Constants.IntNullValue,location, skuIds, Constants.IntNullValue);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("Location", locationName);
        CrpReport.SetParameterValue("FromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("To_date", txtEndDate.Text);
        CrpReport.SetParameterValue("UserName", Session["UserName"].ToString());
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", Type);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
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

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach(ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    } 
}