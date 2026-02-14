using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;

public partial class Forms_RptRawIssuanceReport : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    readonly DistributorController _dController = new DistributorController();
    readonly SkuController _skuController = new SkuController();
    readonly RptInventoryController _rptInvenController = new RptInventoryController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            LoadLocations();
            LoadSection();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");

            categoryRow.Visible = false;
            sectionRow.Visible = true;
            RbType.SelectedIndex = 0;
            LoadCatagory();
        }
    }
    
    private void LoadSection()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = _skuController.SelectProductSection(Constants.IntNullValue, null, null);
            clsWebFormUtil.FillListBox(chblSection, dt, 0, 2, true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
    protected void LoadCatagory()
    {
        var hierarchy = new SkuHierarchyController();
        DataTable dt = hierarchy.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()), Constants.IntNullValue);
        clsWebFormUtil.FillListBox(chblCategory, dt, 0, 3);
    }
    private void LoadLocations()
    {
        try
        {
            DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));

            clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);

            if (dt.Rows.Count > 0)
            {
                drpDistributor.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
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
    protected void RbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbType.SelectedIndex == 0 || RbType.SelectedIndex == 2)
        {
            categoryRow.Visible = false;
            sectionRow.Visible = true;
        }
        else
        {
            categoryRow.Visible = true;
            sectionRow.Visible = false;
        }
    }

    private void ShowReport(int pReprotType)
    {
        string ItemId = null;
        int _value = 0;

        if (RbType.SelectedIndex == 0  || RbType.SelectedIndex == 2)
        {
            for (int i = 0; i < chblSection.Items.Count; i++)
            {
                if (chblSection.Items[i].Selected == true)
                {
                    _value = Convert.ToInt32(chblSection.Items[i].Value.ToString());
                    ItemId += _value + ",";
                }
            }
        }
        else
        {
            for (int i = 0; i < chblCategory.Items.Count; i++)
            {
                if (chblCategory.Items[i].Selected == true)
                {
                    _value = Convert.ToInt32(chblCategory.Items[i].Value.ToString());
                    ItemId += _value + ",";
                }
            }
        }

        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;
        int ReportType = 1;
        if (RbType.SelectedIndex == 1)
        {
            ReportType = 2;
        }
        else if (RbType.SelectedIndex == 2)
        {
            ReportType = 5;
        }
        ds = _rptInvenController.SelectRawIssuenceReport(int.Parse(drpDistributor.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), ItemId, ReportType, int.Parse(drpPriceFilter.Value.ToString()));

        var crpReport = new ReportDocument();

        if (RbType.SelectedIndex == 1)
        {
            crpReport = new CrpRawIssuenceCategory();
        }
        else if (RbType.SelectedIndex == 0)
        {
            crpReport = new CrpRawIssuence();
        }
        else
        {
            crpReport = new CrpRawIssuenceAmount();
        }
        string ReportName = "Section Wise Stock Issuance";
        if (RbType.SelectedIndex == 1)
        {
            ReportName = "Category Wise Stock Issuance";
        }
        else if (RbType.SelectedIndex == 1)
        {
            ReportName = "Amount Wise Stock Issuance";
        }
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("ReportName", ReportName);
        crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        crpReport.SetParameterValue("PriceType", drpPriceFilter.SelectedItem.Text);

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", pReprotType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}