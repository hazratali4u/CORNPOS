using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

public partial class Forms_rptVoidOrderInvoice : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    readonly DistributorController _dController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }


    private void LoadDistributor()
    {
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReportExcel();
    }

    private void ShowReport(int pReprotType)
    {
        DataTable dt = _dPrint.SelectReportTitle(Constants.IntNullValue);
        DataSet ds;
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        System.Text.StringBuilder sbLocationNamess = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if(li.Selected)
            {
                sbLocationIDs.Append(li.Value);
                sbLocationIDs.Append(",");

                sbLocationNamess.Append(li.Text);
                sbLocationNamess.Append(",");
            }
        }
        if (RbReportType.SelectedIndex == 0)
        {
            ds = _rptSaleCtl.PrintVoidOrderSaleReport(sbLocationIDs.ToString(), DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"),Convert.ToInt32(Session["UserID"]),0, int.Parse(rdoSubType.SelectedValue));
            ReportDocument crpReport = new ReportDocument();
            crpReport = new CrpVoidInvoiceReport();
            if (rdoSubType.SelectedValue == "1")
            {
                crpReport = new CrpBillInvoiceWiseReportVoid();
            }
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            if (rdoSubType.SelectedValue == "1")
            {
                crpReport.SetParameterValue("CompanyName", sbLocationNamess.ToString());
                crpReport.SetParameterValue("ReportType", "VOID INVOICE");
            }
            else
            {
                crpReport.SetParameterValue("Location", sbLocationNamess.ToString());
                crpReport.SetParameterValue("ReportName", "Void Invoice Report");
                crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
                crpReport.SetParameterValue("FROM_DATE", txtStartDate.Text);
                crpReport.SetParameterValue("TO_DATE", txtEndDate.Text);
            }
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", pReprotType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (RbReportType.SelectedIndex == 1)
        {
            DataTable dt1 = _dPrint.SelectReportTitle(Constants.IntNullValue);
            ds = _rptSaleCtl.PrintVoidOrderSaleReport(sbLocationIDs.ToString(), DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"),Convert.ToInt32(Session["UserID"]),1, int.Parse(rdoSubType.SelectedValue));
            ReportDocument crpReport = new ReportDocument();
            crpReport = new CrpVoidOrderReport();
            if (rdoSubType.SelectedValue == "1")
            {
                crpReport = new CrpBillInvoiceWiseReportVoid();
            }
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            if (rdoSubType.SelectedValue == "1")
            {
                crpReport.SetParameterValue("CompanyName", sbLocationNamess.ToString());
                crpReport.SetParameterValue("ReportType", "VOID ORDER");
            }
            else
            {
                crpReport.SetParameterValue("Location", sbLocationNamess.ToString());
                crpReport.SetParameterValue("ReportName", "Void Orders Report");
                crpReport.SetParameterValue("COMPANY", dt1.Rows[0]["COMPANY_NAME"].ToString());
                crpReport.SetParameterValue("FromDate",txtStartDate.Text);
                crpReport.SetParameterValue("ToDate", txtEndDate.Text);
            }
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", pReprotType);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    private void ShowReportExcel()
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

        DataTable dt = _dPrint.SelectReportTitle(Constants.IntNullValue);
        DataTable dt2 = new DataTable();
        if (RbReportType.SelectedIndex == 0)
        {
            dt2 = _rptSaleCtl.PrintVoidOrderSaleReportExcel(sbLocationIDs.ToString(),DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"),Convert.ToInt32(Session["UserID"]),0, int.Parse(rdoSubType.SelectedValue));
            
        }
        else if (RbReportType.SelectedIndex == 1)
        {
            DataTable dt1 = _dPrint.SelectReportTitle(Constants.IntNullValue);
            dt2 = _rptSaleCtl.PrintVoidOrderSaleReportExcel(sbLocationIDs.ToString(),DateTime.Parse(txtStartDate.Text + " 00:00:00"),DateTime.Parse(txtEndDate.Text + " 23:59:59"),Convert.ToInt32(Session["UserID"]),1, int.Parse(rdoSubType.SelectedValue));            
        }

        if (dt2 != null)
        {
            string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\VoidOrderVoidInvoiceReport.xls";
            DataSetToExcel.exportToExcel(dt2, path);
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