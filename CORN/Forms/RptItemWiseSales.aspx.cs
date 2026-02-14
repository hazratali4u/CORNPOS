using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Forms_RptItemWiseSales : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
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
            LoadAssingned();
            LoadCashier();
            LoadCategory();
            LoadSKU();
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
        var mUserController = new DistributorController();
        DataTable dt = mUserController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }
    private void LoadCashier()
    {
        string location = string.Empty;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                location += li.Value;
                location += ",";
            }
        }
        ddlCashier.Items.Clear();
        Distributor_UserController UController = new Distributor_UserController();
        DataTable dtEmployee = UController.GetDistributorUser(1, location);
        if (dtEmployee.Rows.Count > 0)
        {
            ddlCashier.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString(CultureInfo.InvariantCulture)));
            clsWebFormUtil.FillDxComboBoxList(ddlCashier, dtEmployee, "USER_ID", "USER_NAME");
            ddlCashier.SelectedIndex = 0;
        }
        else
        {
            ddlCashier.Items.Add(new DevExpress.Web.ListEditItem("No Record found.", "-1"));
            ddlCashier.SelectedIndex = 0;
        }
    }
    private void LoadCategory()
    {
        try
        {
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 15, Constants.IntNullValue);
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
    private void LoadSKU()
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

        DataTable dt = SKUCtl.GetSKUInfo(Constants.IntNullValue,Constants.DateNullValue, sbCategoryIDs.ToString(), 7);
        if (dt.Rows.Count > 0)
        {
            clsWebFormUtil.FillListBox(cblItem, dt, 0, 1);
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
    protected void ddDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAssingned();
    }
    protected void rbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        cblItem.Enabled = true;
        cbItem.Enabled = true;
        cbCategory.Enabled = true;
        cblCategory.Enabled = true;

        if (rbReportType.SelectedValue == "0")
        {
            itemdealRow.Visible = true;
        }
        else if (rbReportType.SelectedValue == "1")
        {
            itemdealRow.Visible = false;
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = true;
            }
            foreach (ListItem li in cblCategory.Items)
            {
                li.Selected = true;
            }
            cblItem.Enabled = false;
            cbItem.Checked = true;
            cbItem.Enabled = false;
            cbCategory.Enabled = false;
            cbCategory.Checked = true;
            cblCategory.Enabled = false;
        }
        else if (rbReportType.SelectedValue == "2")
        {
            itemdealRow.Visible = false;
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = true;
            }
            foreach (ListItem li in cblCategory.Items)
            {
                li.Selected = true;
            }
            cblItem.Enabled = false;
            cbItem.Checked = true;
            cbItem.Enabled = false;
            cbCategory.Checked = true;            
        }

        rowCashier.Visible = false;
        if(rbReportType.SelectedItem.Value == "0")
        {
            rowCashier.Visible = true;
        }
    }
    private void ShowReport(int reportType)
    {
        System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
        System.Text.StringBuilder sbDistributorNames = new System.Text.StringBuilder();
        System.Text.StringBuilder sbItemIDs = new System.Text.StringBuilder();
        System.Text.StringBuilder sbServiceIDs = new System.Text.StringBuilder();
        System.Text.StringBuilder sbServiceNames = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if(li.Selected)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");

                sbDistributorNames.Append(li.Text);
                sbDistributorNames.Append(",");
            }
        }
        foreach (ListItem li in cblItem.Items)
        {
            if (li.Selected)
            {
                sbItemIDs.Append(li.Value);
                sbItemIDs.Append(",");
            }
        }
        foreach (ListItem li in cblService.Items)
        {
            if (li.Selected)
            {
                sbServiceIDs.Append(li.Value);
                sbServiceIDs.Append(",");

                sbServiceNames.Append(li.Text);
                sbServiceNames.Append(",");
            }
        }
        if (rbReportType.SelectedValue == "0")
        {
            DataSet ds = _rptSaleCtl.GetItemWiseSales(sbDistributorIDs.ToString(), sbItemIDs.ToString(), sbServiceIDs.ToString(), Convert.ToInt32(ddlCashier.SelectedItem.Value), DateTime.Parse(txtStartDate.Text),DateTime.Parse(txtEndDate.Text),chkIncludeDeal.Checked == true ? 5 : 0);
            var crpReport = new CrpRegionWiseSale();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("fromDate", txtStartDate.Text);
            crpReport.SetParameterValue("todate", txtEndDate.Text);
            crpReport.SetParameterValue("ReportTitle", "Item Wise Sales Report");
            crpReport.SetParameterValue("ServiceName", sbServiceNames.ToString());
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", reportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (rbReportType.SelectedValue == "1")
        {
            DataSet ds = _rptSaleCtl.GetItemWiseSales(sbDistributorIDs.ToString(), sbItemIDs.ToString(), sbServiceIDs.ToString(), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text),DateTime.Parse(txtEndDate.Text),1);
            var crpReport = new CrpRegionWiseDealSale();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("fromDate", txtStartDate.Text);
            crpReport.SetParameterValue("todate", txtEndDate.Text);
            crpReport.SetParameterValue("ReportTitle", "Deal Wise Sales Report");
            crpReport.SetParameterValue("ServiceName", sbServiceNames.ToString());
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", reportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (rbReportType.SelectedValue == "2")
        {
            DataSet ds = _rptSaleCtl.GetCategorySales(sbDistributorIDs.ToString(),sbServiceIDs.ToString(), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            var crpReport = new CrpCategoryWiseSale();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("fromDate", txtStartDate.Text);
            crpReport.SetParameterValue("todate", txtEndDate.Text);
            crpReport.SetParameterValue("Location", sbDistributorNames.ToString());
            crpReport.SetParameterValue("ReportTitle", "Category Wise Sales Report");
            crpReport.SetParameterValue("ServiceName", sbServiceNames.ToString());
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", reportType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        if (rbReportType.SelectedItem.Value == "0" && chkIncludeDeal.Checked)
        {
            System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
            System.Text.StringBuilder sbDistributorNames = new System.Text.StringBuilder();
            System.Text.StringBuilder sbItemIDs = new System.Text.StringBuilder();
            foreach (ListItem li in ChbDistributorList.Items)
            {
                if (li.Selected)
                {
                    sbDistributorIDs.Append(li.Value);
                    sbDistributorIDs.Append(",");

                    sbDistributorNames.Append(li.Text);
                    sbDistributorNames.Append(",");
                }
            }
            foreach (ListItem li in cblItem.Items)
            {
                if (li.Selected)
                {
                    sbItemIDs.Append(li.Value);
                    sbItemIDs.Append(",");
                }
            }
            DataTable dt = _rptSaleCtl.GetdtItemWiseSales(sbDistributorIDs.ToString(), sbItemIDs.ToString(), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), 2);
            if (dt != null)
            {
                string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\Exported.xls";
                DataSetToExcel.exportToExcel(dt, path);
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
                }
            }
            else
            {
                Response.Write("Time Expired !");
            }
        }
        else
        {
            ShowReport(1);
        }
    }

    protected void cbCategory_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem li in cblCategory.Items)
        {
            li.Selected = cbCategory.Checked;
        }
        LoadSKU();
    }
    
    protected void cblCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSKU();
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

    protected void ChbDistributorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashier();
        int count = 0;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                count++;
            }
        }
        if (count == ChbDistributorList.Items.Count)
        {
            ChbSelectAll.Checked = true;
        }
        else
        {
            ChbSelectAll.Checked = false;
        }
    }
}