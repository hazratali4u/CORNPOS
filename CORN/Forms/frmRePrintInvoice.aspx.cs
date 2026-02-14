using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Drawing;
using System.IO;
using QRCoder;
using System.Linq;

public partial class Forms_frmRePrintInvoice : System.Web.UI.Page
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
                Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("../Login.aspx");
            }

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtFromDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtToDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

            hfCompanyName.Value = Session["COMPANY_NAME"].ToString();
            lblUserName.Text = Session["UserName"].ToString();
            
            hfUserId.Value = HttpContext.Current.Session["UserID"].ToString();//for default user add in orderbooker dropdown

            DataSet ds = _gCtl.SelectDataForPosLoad(Constants.IntNullValue,
                int.Parse(Session["DISTRIBUTOR_ID"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString())
                , int.Parse(Session["RoleID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                hfIsCoverTable.Value = ds.Tables[0].Rows[0]["ISCOVERTABLE"].ToString();
                hfLocationName.Value = ds.Tables[0].Rows[0]["DISTRIBUTOR_NAME"].ToString();
                hfAddress.Value = ds.Tables[0].Rows[0]["ADDRESS1"].ToString();
                hfPhoneNo.Value = ds.Tables[0].Rows[0]["CONTACT_NUMBER"].ToString();
                hfRegNo.Value = ds.Tables[0].Rows[0]["GST_NUMBER"].ToString();
                hfTaxAuthorityLabel2.Value = ds.Tables[0].Rows[0]["TAX_AUTHORITY2"].ToString();
                hfSTRN.Value = ds.Tables[0].Rows[0]["STRN"].ToString();
                hfFacebkId.Value = ds.Tables[0].Rows[0]["FACEBOOK"].ToString();
                hfTaxAuthorityLabel.Value = ds.Tables[0].Rows[0]["TAX_AUTHORITY"].ToString();
                hfServiceCharges.Value = ds.Tables[0].Rows[0]["SERVICE_CHARGES"].ToString();
                hfServiceChargesLabel.Value = ds.Tables[0].Rows[0]["ServiceChargesLabel"].ToString();
                if (ds.Tables[0].Rows[0]["SHOW_LOGO"].ToString() == "True")
                {
                    imgLogo.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                    imgLogo2.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                }
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                hfPOSFee.Value = ds.Tables[0].Rows[0]["POS_FEE"].ToString();
                hfQRCodeImageName.Value = ds.Tables[0].Rows[0]["QRCodeImageName"].ToString();
            }
            else
            {
                // Server.Transfer("Home.aspx");
                Response.Redirect("Home.aspx");
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                hfCompanyEmail.Value = ds.Tables[1].Rows[0]["EMAIL_ADDRESS"].ToString();
            }
           
            if (ds.Tables[4].Rows.Count > 0)//sLIP notes
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    sb.Append(dr["SLIP_NOTE"]);
                    sb.Append("<br /> &nbsp;");
                }
                ltrlSlipNote.Text = sb.ToString();
            }
            if (ds.Tables[6].Rows.Count > 0)
            {
                hfGSTCalculation.Value = ds.Tables[6].Rows[0]["GSTCalculation"].ToString();
                hfBillFormat.Value = ds.Tables[6].Rows[0]["BillFormat"].ToString();
                hfTaxAuthority.Value = ds.Tables[6].Rows[0]["TaxAuthority"].ToString();
                Session.Add("TaxIntegration", hfTaxAuthority.Value);
                hfShowModifirPriceOnBills.Value = ds.Tables[6].Rows[0]["ShowModifirPriceOnBills"].ToString();
                hfEatIn.Value = ds.Tables[6].Rows[0]["EatIn"].ToString();
                hfHideBillNo.Value = ds.Tables[6].Rows[0]["HideBillNo"].ToString();
                hfInvoiceFormat.Value = ds.Tables[6].Rows[0]["InvoiceFormat"].ToString();
                hfTaxInvoiceLable.Value = ds.Tables[6].Rows[0]["TaxAuthorityLabel"].ToString();
            }
        }
    }
    
    #region Bills

    [WebMethod]
    [ScriptMethod]
    public static string SelectPendingBills(DateTime FromDate, DateTime ToDate)
    {
        var mSkuController = new OrderEntryController();
        DataTable dtSkus = mSkuController.SelectPendingBills(FromDate, ToDate, int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), 6);
        return GetJson(dtSkus);
    }
    
    [WebMethod]
    public static string GetPendingBill(long saleInvoiceMasterId)
    {
        var mSkuController = new SkuController();
        DataTable dt = mSkuController.SpGetPendingBill(saleInvoiceMasterId,3,Constants.IntNullValue);
        if (HttpContext.Current.Session["TaxIntegration"].ToString() != "0")
        {
            string InvoiceNumberFBR = dt.Rows[0]["InvoiceNumberFBR"].ToString();
            string InvoiceNumberPRA = dt.Rows[0]["InvoiceNumberPRA"].ToString();
            dt.Columns.Add("QRCodePRA", typeof(string));
            dt.Columns.Add("QRCode", typeof(string));

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(InvoiceNumberPRA, QRCodeGenerator.ECCLevel.Q);
            Bitmap bitmap = qrCode.GetGraphic(20);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] byteImage = ms.ToArray();

            QRCodeGenerator.QRCode qrCodeFBR = qrGenerator.CreateQrCode(InvoiceNumberFBR, QRCodeGenerator.ECCLevel.Q);
            Bitmap bitmapfbr = qrCodeFBR.GetGraphic(20);
            MemoryStream msfbr = new MemoryStream();
            bitmapfbr.Save(msfbr, System.Drawing.Imaging.ImageFormat.Png);
            byte[] byteImageFBR = msfbr.ToArray();
            if (InvoiceNumberPRA.Length > 0)
            {
                dt.Rows[0]["QRCodePRA"] = Convert.ToBase64String(byteImage);
            }
            else
            {
                dt.Rows[0]["QRCodePRA"] = "";
            }
            dt.Rows[0]["QRCode"] = Convert.ToBase64String(byteImageFBR);
            dt.AcceptChanges();
        }
        return GetJson(dt);
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

    protected void lnkExit_OnClick(object sender, EventArgs e)
    {

        Response.Redirect("Home.aspx");

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
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            throw;

        }
    }
    
}