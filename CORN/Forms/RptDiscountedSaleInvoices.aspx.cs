using System;
using System.Web;
using System.Web.UI;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;

public partial class Forms_RptDiscountedSaleInvoices : System.Web.UI.Page
{
    readonly SkuController SKUCtl = new SkuController();
    readonly RptSaleController report = new RptSaleController();
    BankDiscountController cBank = new BankDiscountController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadDISTRIBUTOR();
            LoadDiscountType();
            ChbDistributorList_SelectedIndexChanged(null, null);
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDISTRIBUTOR()
    {
        DistributorController mController = new DistributorController();
        try
        {
            DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
            clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
            foreach (ListItem li in ChbDistributorList.Items)
            {
                li.Selected = true;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadUser()
    {
        ddUser.Items.Clear();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("USER_ID", typeof(int));
            dt.Columns.Add("USER_NAME", typeof(string));
            DataTable dt2 = new DataTable();
            Distributor_UserController du = new Distributor_UserController();
            foreach(ListItem li in ChbDistributorList.Items)
            {
                if(li.Selected)
                {
                    dt2 = du.SelectDistributorUser(10, Convert.ToInt32(li.Value), int.Parse(Session["CompanyId"].ToString()));
                    foreach(DataRow dr in dt2.Rows)
                    {
                        dt.ImportRow(dr);
                    }
                }
            }
            ddUser.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddUser, dt, 0, 1);
            if (dt.Rows.Count > 0)
            {
                ddUser.SelectedIndex = 0;
            }
            else
            {
                ddUser.SelectedIndex = -1;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void BankDiscount()
    {
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                sbLocationIDs.Append(li.Value);
                sbLocationIDs.Append(",");
            }
        }
        ddlBankDiscount.Items.Clear();
        DataTable dt = new DataTable();
        dt = cBank.GetBankDiscount("", 3, sbLocationIDs.ToString(), Constants.DateNullValue, Constants.DateNullValue);
        clsWebFormUtil.FillListBox(ddlBankDiscount, dt, 0, 1);
    }
    private void LoadDiscountType()
    {
        ddlDiscountType.Items.Clear();
        DataTable dt = new DataTable();
        DiscountTemplateController discountCtrl = new DiscountTemplateController();
        dt = discountCtrl.GetDiscountTemplate(2, Constants.IntNullValue);
        ddlDiscountType.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlDiscountType, dt, "EmployeeDiscountTypeID", "DiscountTypeName");
        Session.Add("dtDiscountType", dt);
        if (dt.Rows.Count > 0)
        {
            ddlDiscountType.SelectedIndex = 0;
        }
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                sbLocationIDs.Append(li.Value);
                sbLocationIDs.Append(",");
            }
        }
        if (ddlType.SelectedItem.Value.ToString() == "2")
        {
            string bankIds = null;
            int _value = 0;
            for (int i = 0; i < ddlBankDiscount.Items.Count; i++)
            {
                if (ddlBankDiscount.Items[i].Selected == true)
                {
                    _value = Convert.ToInt32(ddlBankDiscount.Items[i].Value.ToString());
                    bankIds += _value + ",";
                }
            }
            dt = report.GetBankDiscountReportExcel(bankIds, 4, sbLocationIDs.ToString(), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            if (dt != null)
            {
                string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\BankDiscount.xls";
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
        }
        else
        {
            dt = SKUCtl.SelectDiscountedInvoicesExcel(sbLocationIDs.ToString(), Convert.ToInt32(ddUser.SelectedItem.Value), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), Convert.ToInt32(ddlDiscountType.Value));
            if (dt != null)
            {
                string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\DiscountedInvoicesSales.xls";
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
        }        
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        rowDiscountType.Visible = true;
        lblCashier.Visible = true;
        lblBankDiscount.Visible = false;
        if(ddlType.SelectedItem.Value.ToString() == "2")
        {
            lblCashier.Visible = false;
            lblBankDiscount.Visible = true;
            rowDiscountType.Visible = false;
        }
    }
    private void ShowReport(int ReportType)
    {
        CrystalDecisions.CrystalReports.Engine.ReportClass CrpReport = new CrystalDecisions.CrystalReports.Engine.ReportClass();
        DataSet ds = new DataSet();
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        System.Text.StringBuilder sbLocationNames = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if(li.Selected)
            {
                sbLocationIDs.Append(li.Value);
                sbLocationIDs.Append(",");

                sbLocationNames.Append(li.Text);
                sbLocationNames.Append(",");
            }
        }
        if (ddlType.SelectedItem.Value.ToString()=="2")
        {
            string bankIds = null;
            int _value = 0;
            //string locationName = "";
            for (int i = 0; i < ddlBankDiscount.Items.Count; i++)
            {
                if (ddlBankDiscount.Items[i].Selected == true)
                {
                    _value = Convert.ToInt32(ddlBankDiscount.Items[i].Value.ToString());
                    bankIds += _value + ",";
                    //locationName = locationName + drpDistributor.Items[i].Text + ", ";
                }
            }

            ds = report.GetBankDiscountReport(bankIds, 4, sbLocationIDs.ToString(), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            CrpReport = new CORNBusinessLayer.Reports.CrpBankDiscount();
        }
        else
        {
            ds = SKUCtl.SelectDiscountedInvoices(sbLocationIDs.ToString(), Convert.ToInt32(ddUser.SelectedItem.Value), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text),Convert.ToInt32(ddlDiscountType.Value));
            CrpReport = new CORNBusinessLayer.Reports.CrpDiscountedSaleInvoices();
        }        
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Location", sbLocationNames.ToString());
        CrpReport.SetParameterValue("ReportType", ddlType.SelectedItem.Text);
        CrpReport.SetParameterValue("FromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("ToDate", txtEndDate.Text);        
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", ReportType);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void ChbSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach(ListItem li in ChbDistributorList.Items)
        {
            li.Selected = ChbSelectAll.Checked;
        }
        LoadUser();
        BankDiscount();
    }
    protected void ChbDistributorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUser();
        BankDiscount();
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