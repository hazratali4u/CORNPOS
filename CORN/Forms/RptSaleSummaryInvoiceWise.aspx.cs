using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_RptCustomerWiseSale : System.Web.UI.Page
{
    readonly SkuController SKUCtl = new SkuController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!IsPostBack)
        {
            this.LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");
            }
        }
        string strInvoiceNo2 = null;
        string[] strInvoiceNo = null;
        if (txtInvoiceNo.Text.Trim().Length > 0)
        {
            if (txtInvoiceNo.Text.Contains(","))
            {
                strInvoiceNo = txtInvoiceNo.Text.Split(',');
                foreach (string s in strInvoiceNo)
                {
                    strInvoiceNo2 += s;
                    strInvoiceNo2 += ",";
                }
            }
            else
            {
                strInvoiceNo2 = txtInvoiceNo.Text;
            }
        }
        CORNBusinessLayer.Reports.CrpInvoiceWiseSaleSummary CrpReport = new CORNBusinessLayer.Reports.CrpInvoiceWiseSaleSummary();
        DataSet ds = new DataSet();
        ds = SKUCtl.SelectServiceWiseSales(2, sbDistributorIDs.ToString(), Constants.IntNullValue, Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), strInvoiceNo2);

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("FromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("ToDate", txtEndDate.Text);
        CrpReport.SetParameterValue("ReportType", "Invoice Wise Sale Summary");
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);


    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sbDistributorIDs = new System.Text.StringBuilder();
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");
            }
        }
        string strInvoiceNo2 = null;
        string[] strInvoiceNo = null;
        if (txtInvoiceNo.Text.Trim().Length > 0)
        {
            if (txtInvoiceNo.Text.Contains(","))
            {
                strInvoiceNo = txtInvoiceNo.Text.Split(',');
            }
            else
            {
                strInvoiceNo[0] = txtInvoiceNo.Text;
            }

            foreach (string s in strInvoiceNo)
            {
                strInvoiceNo2 += s;
                strInvoiceNo2 += ",";
            }
        }
        DataTable dt = SKUCtl.SelectServiceWiseSalesExcel(2, sbDistributorIDs.ToString(), Constants.IntNullValue, Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), strInvoiceNo2);
        if (dt != null)
        {
            string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\InvoiceWiseSaleSummary.xls";
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