using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;

public partial class Forms_rptMealWiseReport : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            DistributorType();
            LoadAssingned();
           
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads Location Types
    /// </summary>
    private void DistributorType()
    {
        var dController = new DistributorController();
        DataTable dt = dController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(ddDistributorType, dt, 0, 2);
    }
    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        if (ddDistributorType.Items.Count > 0)
        {
            drpDistributor.Items.Clear();
            var mUserController = new UserController();
            DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), int.Parse(ddDistributorType.SelectedValue), 1, int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 1);
            if (dt.Rows.Count > 0)
            {
                drpDistributor.SelectedIndex = 0;
            }
        }
    }
    
    protected void ddDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAssingned();
    }

    private void ShowReport(int reportType)
    {
        DataSet ds = null;
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument crpReport = new ReportDocument();
        ds = _rptSaleCtl.GethourlySaleDetail(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), null, SpParamType());
        crpReport = new CrpHourlySale();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("fromDate", txtStartDate.Text);
        crpReport.SetParameterValue("todate", txtEndDate.Text);
        crpReport.SetParameterValue("PrintedBy", Session["UserName"].ToString());
        crpReport.SetParameterValue("ReportTitle", "Meal Time Wise Sales Report");

        crpReport.SetParameterValue("CompanyName", drpDistributor.SelectedItem.Text);

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
    private int SpParamType()
    {
        int intReportType = 1;
        bool flagLunch = false;
        bool flagDinner = false;
        bool flagMidNight = false;
        foreach(DevExpress.Web.ListEditItem listEditItem in cblMealTime.Items)
        {
            if(listEditItem.Selected)
            {
                if(listEditItem.Value.ToString() == "1")
                {
                    flagLunch = true;
                }
                if (listEditItem.Value.ToString() == "2")
                {
                    flagDinner= true;
                }
                if (listEditItem.Value.ToString() == "3")
                {
                    flagMidNight = true;
                }
            }
        }
        if(flagLunch && flagDinner && flagMidNight)
        {
            intReportType = 8;
        }
        else if(flagLunch && flagDinner)
        {
            intReportType = 7;
        }
        else if (flagLunch && flagMidNight)
        {
            intReportType = 6;
        }
        else if (flagDinner && flagMidNight)
        {
            intReportType = 5;
        }
        else if (flagLunch)
        {
            intReportType = 4;
        }
        else if (flagDinner)
        {
            intReportType = 3;
        }
        else if (flagMidNight)
        {
            intReportType = 2;
        }

        return intReportType;
    }
}