using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_rptItemRecipe : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptInventoryController rptInventory = new RptInventoryController();
    readonly SkuController SKUCtl = new SkuController();
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
            LoadItem();
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadItem()
    {
        cblItem.Items.Clear();
        System.Text.StringBuilder sbCategoryIDs = new System.Text.StringBuilder();
        foreach (ListItem li in cblCategory.Items)
        {
            if (li.Selected)
            {
                sbCategoryIDs.Append(li.Value);
                sbCategoryIDs.Append(",");
            }
        }
        DataTable dt = SKUCtl.GetSKUInfo(Constants.IntNullValue, Constants.DateNullValue, sbCategoryIDs.ToString(), 9);
        if (dt.Rows.Count > 0)
        {
            clsWebFormUtil.FillListBox(cblItem, dt, "SKU_ID", "SKU_NAME");
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = true;
            }
            cbItem.Checked = true;
        }
        else
        {
            cbItem.Checked = false;
        }
    }
    private void ShowReport(int reportType)
    {
        System.Text.StringBuilder sbItemIDs = new System.Text.StringBuilder();
        foreach (ListItem li in cblItem.Items)
        {
            if (li.Selected)
            {
                sbItemIDs.Append(li.Value);
                sbItemIDs.Append(",");
            }
        }

        DataTable dtSKU = rptInventory.GetItemRecipe(sbItemIDs.ToString(),Convert.ToInt32(drpDistributor.Value),Convert.ToInt32(rblRate.SelectedValue),cbInActive.Checked == true ? false: true);
        var crpReport = new CrpItemRecipe();
        crpReport.SetDataSource(dtSKU);
        crpReport.Refresh();
        crpReport.SetParameterValue("PriceLable", rblRate.SelectedItem.Text);
        crpReport.SetParameterValue("Location", drpDistributor.Text);
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

    protected void cbCategory_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem li in cblCategory.Items)
        {
            li.Selected = cbCategory.Checked;
        }
        LoadItem();
    }

    protected void cblCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadItem();
        int count = 0;
        foreach (ListItem li in cblCategory.Items)
        {
            if (li.Selected)
            {
                count++;
            }
        }
        if (count == cblCategory.Items.Count)
        {
            cbCategory.Checked = true;
        }
        else
        {
            cbCategory.Checked = false;
        }
    }

    private void LoadCategory()
    {
        try
        {
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 15, Constants.IntNullValue);
            if(_mDt.Rows.Count> 0)
            {
                clsWebFormUtil.FillListBox(cblCategory, _mDt, 0, 3, true);
                foreach (ListItem li in cblCategory.Items)
                {
                    li.Selected = true;
                }
                cbCategory.Checked = true;
            }
            else
            {
                cbCategory.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
}