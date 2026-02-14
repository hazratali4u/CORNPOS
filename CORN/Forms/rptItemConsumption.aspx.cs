using System;
using System.Web;
using System.Web.UI;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;

public partial class Forms_rptItemConsumption : System.Web.UI.Page
{
    readonly SkuController SKUCtl = new SkuController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        LoadSKUConsumed1();
        if (!Page.IsPostBack)
        {
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");

            LoadDistributor();
            LoadSKUConsumed();
        }

    }
    private void LoadSKUConsumed1()
    {
        ddlReportType.Items[6].Attributes.Add("style", "display:none");
        if (Rblconsumtion.SelectedValue == "1" || Rblconsumtion.SelectedValue == "2" || Rblconsumtion.SelectedValue == "4")
        {
            ddlReportType.Items[0].Attributes.Add("style", "display:block");
            ddlReportType.Items[1].Attributes.Add("style", "display:block");
            ddlReportType.Items[2].Attributes.Add("style", "display:none");
            ddlReportType.Items[3].Attributes.Add("style", "display:none");
            ddlReportType.Items[4].Attributes.Add("style", "display:none");
        }
        else
        {
            ddlReportType.Items[2].Attributes.Add("style", "display:block");
            ddlReportType.Items[3].Attributes.Add("style", "display:block");
            ddlReportType.Items[4].Attributes.Add("style", "display:block");
            ddlReportType.Items[0].Attributes.Add("style", "display:none");
            ddlReportType.Items[1].Attributes.Add("style", "display:none");
        }
        if (Rblconsumtion.SelectedValue == "2")
        {
            ddlReportType.Items[6].Attributes.Add("style", "display:block");
        }
    }
    private void LoadSKUConsumed()
    {
        ddlSKU.Items.Clear();
        ddlReportType.Items[6].Attributes.Add("style", "display:none");
        if(Rblconsumtion.SelectedValue == "2")
        {
            ddlReportType.Items[6].Attributes.Add("style", "display:block");
        }
        if (Rblconsumtion.SelectedValue == "1" || Rblconsumtion.SelectedValue == "2" || Rblconsumtion.SelectedValue == "4")
        {
            ddlReportType.Items[0].Attributes.Add("style", "display:block");
            ddlReportType.Items[1].Attributes.Add("style", "display:block");
            ddlReportType.Items[2].Attributes.Add("style", "display:none");
            ddlReportType.Items[3].Attributes.Add("style", "display:none");
            ddlReportType.Items[4].Attributes.Add("style", "display:none");

            if (Rblconsumtion.SelectedValue != "2")
            {
                ddlReportType.Items[5].Attributes.Add("style", "display:none");
            }
            else
            {
                ddlReportType.Items[5].Attributes.Add("style", "display:block");
            }
            ddlReportType.SelectedIndex = 0;
        }
        else
        {
            ddlReportType.Items[2].Attributes.Add("style", "display:block");
            ddlReportType.Items[3].Attributes.Add("style", "display:block");
            ddlReportType.Items[4].Attributes.Add("style", "display:block");

            ddlReportType.Items[0].Attributes.Add("style", "display:none");
            ddlReportType.Items[1].Attributes.Add("style", "display:none");
            ddlReportType.Items[5].Attributes.Add("style", "display:none");

            ddlReportType.SelectedIndex = 2;
        }

        if (Rblconsumtion.SelectedValue == "2" && ddlReportType.SelectedValue == "6")
        {
            itemRow.Visible = false;
        }
        else
        {
            itemRow.Visible = true;
        }

        int value = 3;
        if (Rblconsumtion.SelectedValue == "1")
        {
            value = 2;
        }
        else if (Rblconsumtion.SelectedValue == "3")
        {
            value = 4;
        }
        else if (Rblconsumtion.SelectedValue == "4")
        {
            value = 0;//Has Modifiers
        }
        DataTable dt = SKUCtl.SelectSkuConsumption(value);

        if (dt.Rows.Count > 0)
        {
            ddlSKU.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
            if(Rblconsumtion.SelectedValue == "4")
            {
                clsWebFormUtil.FillDxComboBoxList(ddlSKU, dt, 0, 2);
            }
            else
            {
                clsWebFormUtil.FillDxComboBoxList(ddlSKU, dt, 0, 1);
            }
            ddlSKU.SelectedIndex = 0;
        }
    }

    private void showReport(int Type)
    {
        try
        {
            int SKUID = 0;
            try
            {
                SKUID = Convert.ToInt32(ddlSKU.SelectedItem.Value);
            }
            catch (Exception)
            {
                SKUID = 0;
            }

            if (SKUID == 0 && Rblconsumtion.SelectedValue != "5")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No Item found.');", true);
                return;
            }
            DataSet ds = new DataSet();
            int reportType = Convert.ToInt32(ddlReportType.SelectedValue);
            if (reportType == 4)
                reportType = 2;
            ds = SKUCtl.SelectSKUConsumption(Convert.ToInt32(ddlLocation.SelectedItem.Value), SKUID, reportType,Convert.ToInt32(Rblconsumtion.SelectedValue),DateTime.Parse(txtStartDate.Text),DateTime.Parse(txtEndDate.Text));
            dvMsg.Visible = false;

            if (ds.Tables["spSelectSKU_Consumption"].Rows.Count > 0)
            {
                ReportDocument CrpReport = new ReportDocument();

                if (ddlReportType.SelectedValue == "1")
                {
                     CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumption();
                } 
                else if (ddlReportType.SelectedValue == "3")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumptionItemWise();
                }
                else if (ddlReportType.SelectedValue == "2")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumptionSummary();
                }
                else if (ddlReportType.SelectedValue == "4")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumptionSummaryCategory();
                }
                else if (ddlReportType.SelectedValue == "5")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumptionDateWiseRaw();
                }
                else if (ddlReportType.SelectedValue == "6")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsmpFinishCategoryWise();
                }
                else if (ddlReportType.SelectedValue == "7")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumptionSalesPurchase();
                }
                else
                {
                     CrpReport = new CORNBusinessLayer.Reports.CrpSKUConsumptionSummaryDateWise();
                }
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                CrpReport.SetParameterValue("Location", ddlLocation.SelectedItem.Text);
                try
                {
                    CrpReport.SetParameterValue("SKU_NAME", ddlSKU.SelectedItem.Text);
                }
                catch (Exception ex)
                {
                    CrpReport.SetParameterValue("SKU_NAME", "");
                }
                CrpReport.SetParameterValue("ReportType", Rblconsumtion.SelectedItem.Text);
                CrpReport.SetParameterValue("FromDate",DateTime.Parse(txtStartDate.Text));
                CrpReport.SetParameterValue("ToDate", DateTime.Parse(txtEndDate.Text));
                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", Type);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            else
            {
                dvMsg.Visible =true;
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
    }
    
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }

    protected void Rblconsumtion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlReportType.Enabled = true;
        if (Rblconsumtion.SelectedValue == "5")
        {
            ddlReportType.SelectedIndex = 0;
            ddlReportType.Enabled = false;
        }
        else
        {
            LoadSKUConsumed();
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

        ddlLocation.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSKU.Enabled = true;
        if(ddlReportType.SelectedValue == "7")
        {
            ddlSKU.Enabled = false;
            ddlSKU.SelectedIndex = 0;
        }
        if (Rblconsumtion.SelectedValue == "2" && ddlReportType.SelectedValue == "6")
        {
            itemRow.Visible = false;
        }
        else
        {
            itemRow.Visible = true;
        }        
    }
}