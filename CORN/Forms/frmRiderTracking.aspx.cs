using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// From to Assign, UnAssign Forms To Users
/// </summary>
public partial class frmRiderTracking : System.Web.UI.Page
{
   readonly SkuController mController = new SkuController();
    readonly DataControl dc = new DataControl();

    /// <summary>
    /// Page_Load Function Populates All Combos and ListBox On The Page
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
            LoadLocations();
            LoadOrderBooker();

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtFromDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtToDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Categories
    /// </summary>
    private void LoadOrderBooker()
    {
        SaleForceController mDController = new SaleForceController();
        DataTable dt = mDController.SelectSaleForceAssignedArea(Constants.SALES_FORCE_DELIVERYMAN,
            int.Parse(ddlLocation.SelectedItem.Value.ToString())
            , Constants.IntNullValue, int.Parse(HttpContext.Current.Session["companyId"].ToString()),
            Constants.IntNullValue);

        clsWebFormUtil.FillDxComboBoxList(ddlOrderBooker, dt, "USER_ID", "USER_NAME", false);

        if (dt.Rows.Count > 0)
        {
            ddlOrderBooker.SelectedIndex = 0;
            hfRiderID.Value = ddlOrderBooker.SelectedItem.Value.ToString();
        }

    }

    private void LoadLocations()
    {
        DistributorController mDController = new DistributorController();
        DataTable dt = mDController.SelectDistributor(Constants.IntNullValue);

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", false);

        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
            hflocationID.Value = ddlLocation.SelectedItem.Value.ToString();
        }
    }

    [WebMethod]
    public static string ddlLocation_Changed(string locationId)
    {
        SaleForceController mDController = new SaleForceController();
        DataTable dt = mDController.SelectSaleForceAssignedArea(Constants.SALES_FORCE_DELIVERYMAN,int.Parse(locationId), Constants.IntNullValue, int.Parse(HttpContext.Current.Session["companyId"].ToString()),Constants.IntNullValue);
        return GetJson(dt);
    }

    [WebMethod]
    public static string GetRiderTrackingDetails(int orderBookerId, int locationId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        SaleForceController mDController = new SaleForceController();
        DataTable dt = mDController.SelectSaleForceTrackingDetails(orderBookerId, locationId, fromDate, toDate);
        return GetJson(dt);
    }

    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }
}