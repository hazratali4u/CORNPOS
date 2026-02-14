using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;

public partial class Forms_RptConsumptionMonthWise : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly SkuController _orderEntryController = new SkuController();
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
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }


    private void LoadDistributor()
    {
        drpDistributor.DataSource = null;
        drpDistributor.DataBind();

        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
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
        string d_id = null;
        int _value = 0;
        int count = 0;
        string locationName = "";
        for (int i = 0; i < drpDistributor.Items.Count; i++)
        {
            if (drpDistributor.Items[i].Selected == true)
            {
                _value = Convert.ToInt32(drpDistributor.Items[i].Value.ToString());
                d_id += _value + ",";
                locationName = string.Join(", ", drpDistributor.Items[i].Text);
                count++;
            }
        }

        if (count == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Location');", true);
            return;
        }

        DataTable dt = _dPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
        DataSet ds;

        var startDate = DateTime.Parse("1-" + txtStartDate.Text);
        var endDate = DateTime.Parse("1-" + txtEndDate.Text);

        var lastDayOfMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);

        endDate = DateTime.Parse(lastDayOfMonth + "-" + txtEndDate.Text);

        ds = _orderEntryController.GetConsumptionMonthWise(d_id,
            startDate,
            endDate,
            int.Parse(RbReportType.SelectedValue),
            int.Parse(drpSubType.Value.ToString()));

        var crpReport = new ReportDocument();

        crpReport = new CrpMonthWiseConsumption();
        
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", locationName);
        crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        crpReport.SetParameterValue("ReportName", drpSubType.Value.ToString() == "1" ? 
            "Month Wise Consumption (By Amount)" : "Month Wise Consumption (By Qty)");
        crpReport.SetParameterValue("COMPANY", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Username", Session["UserName"].ToString());
        crpReport.SetParameterValue("MainType", RbReportType.SelectedValue.ToString());
        crpReport.SetParameterValue("SubType", drpSubType.Value.ToString());

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", pReprotType);

        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}
