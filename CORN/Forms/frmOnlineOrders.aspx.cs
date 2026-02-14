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

public partial class Forms_frmOnlineOrders : System.Web.UI.Page
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
            lblUserName.Text = Session["UserName"].ToString();
            DateTime date = DateTime.Parse(Session["CurrentWorkDate"].ToString());
            lblDateTime.Text = date.ToString("dd-MMMM-yyyy");
            hfCurrentWorkDate.Value = date.ToString("dd-MMM-yyyy");
            DataSet ds = _gCtl.SelectDataForPosLoad(Constants.IntNullValue,int.Parse(Session["DISTRIBUTOR_ID"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.IntNullValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["SHOW_LOGO"].ToString() == "True")
                {
                    imgLogo.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                    imgLogo2.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                    imgLogo22.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                }
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                hfInvoiceFooterType.Value = ds.Tables[0].Rows[0]["InvoiceFooterType"].ToString();
                hfFacebkId.Value = ds.Tables[0].Rows[0]["FACEBOOK"].ToString();
                hfAddress.Value = ds.Tables[0].Rows[0]["ADDRESS1"].ToString();
                hfPhoneNo.Value = ds.Tables[0].Rows[0]["CONTACT_NUMBER"].ToString();
                hfTaxAuthorityLabel.Value = ds.Tables[0].Rows[0]["TAX_AUTHORITY"].ToString();
                hfTaxAuthorityLabel2.Value = ds.Tables[0].Rows[0]["TAX_AUTHORITY2"].ToString();
                hfRegNo.Value = ds.Tables[0].Rows[0]["GST_NUMBER"].ToString();
                hfSTRN.Value = ds.Tables[0].Rows[0]["STRN"].ToString();
                hfSalesTaxCreditCard.Value = ds.Tables[0].Rows[0]["GST_CREDIT_CARD"].ToString();
                hfAutoPromotion.Value = ds.Tables[0].Rows[0]["AutoPromotion"].ToString();
                hfServiceChargesCalculation.Value = ds.Tables[6].Rows[0]["ServiceChargesCalculation"].ToString();
                hfSalesTax.Value = ds.Tables[0].Rows[0]["GST"].ToString();
                hfSalesTaxCreditCard.Value = ds.Tables[0].Rows[0]["GST_CREDIT_CARD"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                hfCompanyEmail.Value = ds.Tables[1].Rows[0]["EMAIL_ADDRESS"].ToString();
            }
            if(ds.Tables[6].Rows.Count > 0)
            {
                hfProvisionalBillHeaderFormat.Value = ds.Tables[6].Rows[0]["ProvisionalBillHeaderFormat"].ToString();
                hfShowNTNOnProvissionalBill.Value = ds.Tables[6].Rows[0]["ShowNTNOnProvissionalBill"].ToString();
                hfPendigBillRefreshTime.Value = ds.Tables[6].Rows[0]["PendigBillRefreshTime"].ToString();
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
    }
        
    #region Bills

    [WebMethod]
    [ScriptMethod]
    public static string SelectPendingBills()
    {
        var mSkuController = new OrderEntryController();
        DataTable dtSkus = mSkuController.SelectPendingBills(Constants.LongNullValue,DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()),Constants.IntNullValue, Constants.IntNullValue, 18);
        return GetJson(dtSkus);
    }

    #endregion

    [WebMethod]
    [ScriptMethod]
    public static string GetOrderData(string InvoiceId)
    {
        DataTable dtReturn = new DataTable();
        try
        {
            OrderEntryController order = new OrderEntryController();
            dtReturn = order.UpdateOrder(long.Parse(InvoiceId), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 3);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return GetJson(dtReturn);
    }

    [WebMethod]
    [ScriptMethod]
    public static string UpdateOrder(string InvoiceId)
    {
        DataTable dtReturn = new DataTable();
        try
        {
            int userId = int.Parse(HttpContext.Current.Session["UserID"].ToString());
            OrderEntryController order = new OrderEntryController();
            dtReturn = order.UpdateOrder(long.Parse(InvoiceId), userId, Constants.IntNullValue, Constants.IntNullValue, 2);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return GetJson(dtReturn);
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

    protected void lnkExit_OnClick(object sender, EventArgs e)
    {
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
}