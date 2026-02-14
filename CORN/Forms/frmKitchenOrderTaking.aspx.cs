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

public partial class Forms_frmKitchenOrderTaking : System.Web.UI.Page
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

                //Session["Cust_TYPE_KDS"] = "0";

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
                HttpContext.Current.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("../Login.aspx");
            }

            //lblUserName.Text = Session["UserName"].ToString();
            DateTime date = DateTime.Parse(Session["CurrentWorkDate"].ToString());
            lblDateTime.Text = date.ToString("dd-MMMM-yyyy");
            //hfCurrentWorkDate.Value = date.ToString("dd-MMM-yyyy");

            DataSet ds = _gCtl.SelectDataForPosLoad(Constants.IntNullValue,
               int.Parse(Session["DISTRIBUTOR_ID"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.IntNullValue);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["SHOW_LOGO"].ToString() == "True")
                {
                    //imgLogo.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                }
                string url = HttpContext.Current.Request.Url.AbsoluteUri;

            }
            else
            {
                Response.Redirect("Home.aspx");
                // Server.Transfer("Level.aspx?LevelID=27&LevelType=1");
            }

            if (Session["Cust_TYPE_KDS"] != null && !string.IsNullOrEmpty(Session["Cust_TYPE_KDS"].ToString()))
            {
                ddlCustomerType.SelectedValue = Session["Cust_TYPE_KDS"].ToString();
            }
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static string GetPendingAndCompeletdOrdersCount()
    {
        var mSkuController = new OrderEntryController();
        DataTable dtCount = mSkuController.SELECT_PendingAndCompletedOrdersCount(DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()));
        return GetJson(dtCount);
    }

    [WebMethod]
    [ScriptMethod]
    public static string LoadSaleForce()
    {
        SaleForceController mDController = new SaleForceController();
        DataTable dt = mDController.SelectSaleForceAssignedArea(Constants.IntNullValue, int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), Constants.IntNullValue, int.Parse(HttpContext.Current.Session["companyId"].ToString()), Constants.IntNullValue);
        return GetJson(dt);
    }

    #region Bills

    [WebMethod]
    [ScriptMethod]
    public static string SelectPendingBills()
    {
        var mSkuController = new OrderEntryController();

        var customerTypeId = Constants.IntNullValue;

        if (HttpContext.Current.Session["Cust_TYPE_KDS"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Cust_TYPE_KDS"].ToString()))
        {
            if (HttpContext.Current.Session["Cust_TYPE_KDS"].ToString() == "0")
                customerTypeId = Constants.IntNullValue;
            else
                customerTypeId = int.Parse(HttpContext.Current.Session["Cust_TYPE_KDS"].ToString());
        }

        DataTable dtSkus = mSkuController.SelectPendingBills(Constants.LongNullValue,
            DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()),
            int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()),
            int.Parse(HttpContext.Current.Session["UserID"].ToString()),
            customerTypeId,
            3);

        HttpContext.Current.Session.Add("dtDetail", dtSkus);
        DataView view = new DataView(dtSkus);
        DataTable distinctValues = view.ToTable(true, "SERVICE_TYPE", "ORDER_NO", "TABLE_NO2", "ORDER_DATE_TIME",
            "ORDER_TIME", "ORDER_BOOKER_NAME", "INVOICE_ID", "REMARKS", "HideKDSMarkCompleteBtn", "covertable");
        return GetJson(distinctValues);
    }

    [WebMethod]
    public static string GetPendingBill()
    {
        DataTable dtDetail = (DataTable)HttpContext.Current.Session["dtDetail"];
        return GetJson(dtDetail);
    }

    #endregion

    [WebMethod]
    [ScriptMethod]
    public static void UpdateInvoice(string InvoiceId)
    {
        try
        {
            if (IsDayClosed())
            {
                UserController UserCtl = new UserController();

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(HttpContext.Current.Session["User_Log_ID"]), Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                HttpContext.Current.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("../Login.aspx");
            }

            // int tableId = int.Parse(HttpContext.Current.Session["TableId"].ToString());

            int userId = int.Parse(HttpContext.Current.Session["UserID"].ToString());
            //DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());


            OrderEntryController.Update_kitchenInvoice(
                long.Parse(InvoiceId), userId, Constants.DateNullValue, distributerId);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static void UpdateInvoicePaid(string InvoiceId)
    {
        try
        {
            if (IsDayClosed())
            {
                UserController UserCtl = new UserController();

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(HttpContext.Current.Session["User_Log_ID"]), Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                HttpContext.Current.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("../Login.aspx");
            }

            // int tableId = int.Parse(HttpContext.Current.Session["TableId"].ToString());

            int userId = int.Parse(HttpContext.Current.Session["UserID"].ToString());
            DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());


            OrderEntryController.Update_Invoice_AS_Paid(
                long.Parse(InvoiceId), distributerId);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static void InvoiceCallBack(string InvoiceId)
    {
        try
        {
            if (IsDayClosed())
            {
                UserController UserCtl = new UserController();

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(HttpContext.Current.Session["User_Log_ID"]), Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                HttpContext.Current.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("../Login.aspx");
            }
            int userId = int.Parse(HttpContext.Current.Session["UserID"].ToString());
            DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());
            OrderEntryController.InvoiceCallback(long.Parse(InvoiceId), distributerId);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static void UpdateItemAsReady(string SALE_INVOICE_DETAIL_ID, string isReady)
    {
        try
        {
            if (IsDayClosed())
            {
                UserController UserCtl = new UserController();

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(HttpContext.Current.Session["User_Log_ID"]), Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                HttpContext.Current.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("../Login.aspx");
            }

            // int tableId = int.Parse(HttpContext.Current.Session["TableId"].ToString());


            OrderEntryController.Update_Item_AS_Ready(
                long.Parse(SALE_INVOICE_DETAIL_ID), bool.Parse(isReady));

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkExit_OnClick(object sender, EventArgs e)
    {
        //Server.Transfer("Level.aspx?LevelID=27&LevelType=1");

        UserController UserCtl = new UserController();

        UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
        Session.Clear();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("../Login.aspx");
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
    protected void ddlCustomerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Cust_TYPE_KDS"] = null;
        Session.Add("Cust_TYPE_KDS", ddlCustomerType.SelectedValue);
        ddlCustomerType.SelectedValue = Session["Cust_TYPE_KDS"].ToString();
        Response.Redirect(Request.RawUrl);
    }
}