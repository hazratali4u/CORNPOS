using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_RptItemWiseSales : System.Web.UI.Page
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
            LoadAssingned();
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
        drpDistributor.Items.Clear();
        var mUserController = new UserController();
        DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, int.Parse(Session["CompanyId"].ToString()));

        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 1);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = _rptSaleCtl.GetSalesRawData(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(Session["UserId"].ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), 0);
        if (dt != null)
        {
            string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\SalesRawData.xls";
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
}