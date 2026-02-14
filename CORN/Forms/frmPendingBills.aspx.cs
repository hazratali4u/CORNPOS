using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Linq;

public partial class Forms_frmPendingBills : System.Web.UI.Page
{
    private readonly GeoHierarchyController _gCtl = new GeoHierarchyController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            if (IsDayClosed())
            {
                UserController UserCtl = new UserController();

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
                HttpContext.Current.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("../Login.aspx");
            }            
        }
    }    
    
    #region Bills

    [WebMethod]
    [ScriptMethod]
    public static string GetPendingTables()
    {
        var mTableController = new OrderEntryController();
        DataTable dtTables = mTableController.SelectPendingBills(Constants.LongNullValue,
        DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), Constants.IntNullValue, Constants.IntNullValue, 1);
        return GetJson(dtTables);
    }

    #endregion

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

    private static bool IsDayClosed()
    {
        DistributorController DistrCtl = new DistributorController();
        try
        {
            DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(HttpContext.Current.Session["DISTRIBUTOR_ID"]), 3);
            if (dtDayClose.Rows.Count > 0)
            {
                if (Convert.ToDateTime(HttpContext.Current.Session["CurrentWorkDate"]) == Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
                {
                    return false;
                }
            }
            return true;
        }
        catch (Exception)
        {

            throw;
        }
    }
}