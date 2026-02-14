using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
/// <summary>
/// Form For Voucher View Report
/// </summary>
public partial class Forms_rptVoucherSummary : System.Web.UI.Page
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
            this.VoucherType();
            btnView.Attributes.Add("onclick", "return ValidateForm();");
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

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

    /// <summary>
    /// Loads Voucher Types To VoucherType Combo
    /// </summary>
    private void VoucherType()
    {
        DrpVoucherSubType.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        DrpVoucherSubType.Items.Add(new DevExpress.Web.ListEditItem("Cash Receipt Voucher", "14"));
        DrpVoucherSubType.Items.Add(new DevExpress.Web.ListEditItem("Cash Payment Voucher", "24"));
        DrpVoucherSubType.Items.Add(new DevExpress.Web.ListEditItem("Bank Receipt Voucher", "15"));
        DrpVoucherSubType.Items.Add(new DevExpress.Web.ListEditItem("Bank Payment Voucher", "17"));
        DrpVoucherSubType.Items.Add(new DevExpress.Web.ListEditItem("Journal Voucher", "16"));
        DrpVoucherSubType.SelectedIndex = 0;
    }

    /// <summary>
    /// Loads Vouchers To Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnView_Click(object sender, EventArgs e)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        crpVoucherViewSummary CrpReport = new crpVoucherViewSummary();

        DataSet ds = null;
        
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ds = RptAccountCtl.SelectUnpostVoucherForPrint(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Convert.ToInt32(DrpVoucherSubType.Value), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DISTRIBUTOR_NAME", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
        CrpReport.SetParameterValue("VoucherType", DrpVoucherSubType.SelectedItem.Text);
        CrpReport.SetParameterValue("DateFrom", Convert.ToDateTime(txtStartDate.Text));
        CrpReport.SetParameterValue("ToDate", Convert.ToDateTime(txtEndDate.Text));
        
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        RptAccountController RptAccountCtl = new RptAccountController();        
        DataTable dt = null;
        dt = RptAccountCtl.SelectUnpostVoucherForPrintExcel(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Convert.ToInt32(DrpVoucherSubType.Value), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
        if (dt != null)
        {
            string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\VoucherSummary.xls";
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