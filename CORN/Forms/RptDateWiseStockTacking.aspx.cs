using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_RptDateWiseStockTacking : System.Web.UI.Page
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
            LoadItems();

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    private void LoadSection()
    {
        try
        {
            SkuHierarchyController _mHerController = new SkuHierarchyController();
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 15, Constants.IntNullValue);
            clsWebFormUtil.FillListBox(chblSection, _mDt, 0, 3, true);

            foreach (ListItem li in chblSection.Items)
            {
                li.Selected = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
    private void LoadItems()
    {
        try
        {
            System.Text.StringBuilder sbSection = new System.Text.StringBuilder();
            chblItem.Items.Clear();
            foreach (ListItem li in chblSection.Items)
            {
                if (li.Selected)
                {
                    sbSection.Append(li.Value);
                    sbSection.Append(",");
                }
            }

            DataTable dt = new DataTable();
            dt = _skuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 21, Constants.IntNullValue, sbSection.ToString());
            clsWebFormUtil.FillListBox(chblItem, dt, "SKU_ID", "SKU_NAME", true);
            if (chblItem.Items.Count > 0)
            {
                chbAllItems.Checked = true;
                for (int i = 0; i < chblItem.Items.Count; i++)
                {
                    chblItem.Items[i].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
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

    private void ShowReport(int pReprotType)
    {
        string ItemId = null;
        int _value = 0;
        for (int i = 0; i < chblItem.Items.Count; i++)
        {
            if (chblItem.Items[i].Selected == true)
            {
                _value = Convert.ToInt32(chblItem.Items[i].Value.ToString());
                ItemId += _value + ",";
            }
        }

        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;

        ds = _rptInvenController.SelectDateWiseStokReport(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), ItemId, 1);
        var crpReport = new CrpDateWiseStockTaking();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("DateFrom", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("DateTo", DateTime.Parse(txtEndDate.Text));
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", pReprotType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void chblSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadItems();
    }
    protected void chbAllSections_CheckedChanged(object sender, EventArgs e)
    {
        if (chbAllSections.Checked)
        {
            foreach (ListItem li in chblSection.Items)
            {
                li.Selected = true;
            }
        }
        else
        {
            foreach (ListItem li in chblSection.Items)
            {
                li.Selected = false;
            }
        }
        LoadItems();
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSection();
        LoadItems();
        chbAllSections.Checked = false;
        chbAllItems.Checked = false;
    }
}