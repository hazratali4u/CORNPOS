using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using Newtonsoft.Json;
using System.Linq;

public partial class Forms_frmOrderTaking : System.Web.UI.Page
{
    DataControl DC = new DataControl();
    
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
            try
            {
                lblLicense.Text = Session["LicenseMessage"].ToString();
            }
            catch (Exception)
            {
            }
            this.GetAppSettingDetail();
            txtCustomerDOB.Attributes.Add("readonly", "readonly");            
            hfCompanyName.Value = Session["COMPANY_NAME"].ToString();
            lblUserName.Text = Session["UserName"].ToString();
            DateTime date = DateTime.Parse(Session["CurrentWorkDate"].ToString());
            lblDateTime.Text = date.ToString("dd-MMMM-yyyy");
            hfCurrentWorkDate.Value = date.ToString("dd-MMM-yyyy");
            hfLocationID.Value = Session["DISTRIBUTOR_ID"].ToString();
            hfUserId.Value = HttpContext.Current.Session["UserID"].ToString();//for default user add in orderbooker dropdown
            hfUserType.Value = HttpContext.Current.Session["UserType"].ToString();
            hfItemWiseGST.Value = HttpContext.Current.Session["ItemWiseGST"].ToString();
            hfShowParentCategory.Value = HttpContext.Current.Session["ShowParentCategory"].ToString();
            hfPrintInvoiceFromWS.Value = HttpContext.Current.Session["PrintInvoiceFromWS"].ToString();
            hfHiddenReports.Value = HttpContext.Current.Session["HiddenReports"].ToString();
            DataSet ds = _gCtl.SelectDataForPosLoad(int.Parse(hfUserId.Value),int.Parse(Session["DISTRIBUTOR_ID"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.IntNullValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hfSalesTax.Value = ds.Tables[0].Rows[0]["GST"].ToString();
                hfSalesTaxCreditCard.Value = ds.Tables[0].Rows[0]["GST_CREDIT_CARD"].ToString();
                Session.Add("GSTCardRate", hfSalesTaxCreditCard.Value);
                hfIsCoverTable.Value = ds.Tables[0].Rows[0]["ISCOVERTABLE"].ToString();
                hfServiceCharges.Value = ds.Tables[0].Rows[0]["SERVICE_CHARGES"].ToString();
                hfLocationName.Value = ds.Tables[0].Rows[0]["DISTRIBUTOR_NAME"].ToString();
                hfAddress.Value = ds.Tables[0].Rows[0]["ADDRESS1"].ToString();
                hfPhoneNo.Value = ds.Tables[0].Rows[0]["CONTACT_NUMBER"].ToString();
                hfRegNo.Value = ds.Tables[0].Rows[0]["GST_NUMBER"].ToString();
                hfFacebkId.Value = ds.Tables[0].Rows[0]["FACEBOOK"].ToString();
                Session.Add("MessageSMS", "Testing");
                if (ds.Tables[0].Rows[0]["SHOW_LOGO"].ToString() == "True")
                {
                    imgLogo.Src = "../Pics/" + ds.Tables[0].Rows[0]["PIC"].ToString();
                }
                hfPrintKOT.Value = ds.Tables[0].Rows[0]["PrintKOT"].ToString();
                Session.Add("hfPrintKOT", hfPrintKOT.Value);
                hfPrintKOTDelivery.Value = ds.Tables[0].Rows[0]["PrintKOTDelivery"].ToString();
                hfPrintKOTTakeaway.Value = ds.Tables[0].Rows[0]["PrintKOTTakeaway"].ToString();
                hfServiceChargesType.Value = ds.Tables[0].Rows[0]["SERVICE_CHARGES_TYPE"].ToString();
                hfServiceChargesValue.Value = ds.Tables[0].Rows[0]["SERVICE_CHARGES_VALUE"].ToString();
                hfTaxAuthorityLabel2.Value = ds.Tables[0].Rows[0]["TAX_AUTHORITY2"].ToString();
                hfTaxAuthorityLabel.Value = ds.Tables[0].Rows[0]["TAX_AUTHORITY"].ToString();
                Session.Add("hfServiceChargesType", hfServiceChargesType.Value);
                hfIsDeliveryCharges.Value = ds.Tables[0].Rows[0]["IsDeliveryCharges"].ToString();
                hfDELIVERY_CHARGES_TYPE.Value = ds.Tables[0].Rows[0]["DELIVERY_CHARGES_TYPE"].ToString();
                hfDELIVERY_CHARGES_VALUE.Value = ds.Tables[0].Rows[0]["DELIVERY_CHARGES_VALUE"].ToString();
                hfAutoPromotion.Value = ds.Tables[0].Rows[0]["AutoPromotion"].ToString();
                hfServiceChargesLabel.Value = ds.Tables[0].Rows[0]["ServiceChargesLabel"].ToString();
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
            }
            else
            {
                Response.Redirect("Home.aspx");
                // Server.Transfer("Level.aspx?LevelID=27&LevelType=1");
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                hfCompanyEmail.Value = ds.Tables[1].Rows[0]["EMAIL_ADDRESS"].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                hfCan_DineIn.Value = ds.Tables[2].Rows[0]["Can_DineIn"].ToString();
                hfCan_Delivery.Value = ds.Tables[2].Rows[0]["Can_Delivery"].ToString();
                hfCan_TakeAway.Value = ds.Tables[2].Rows[0]["Can_TakeAway"].ToString();
                hfDefaultServiceType.Value = ds.Tables[2].Rows[0]["DefaultServiceType"].ToString();
                hfIsSplitBill.Value = ds.Tables[2].Rows[0]["IsSplitBill"].ToString();
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                hfReport.Value = GetJson(ds.Tables[3]);
            }
            if(ds.Tables[6].Rows.Count>0)
            {
                hfGSTCalculation.Value = ds.Tables[6].Rows[0]["GSTCalculation"].ToString();
                hfServiceChargesCalculation.Value = ds.Tables[6].Rows[0]["ServiceChargesCalculation"].ToString();
                Session.Add("GSTCalculation", hfGSTCalculation.Value);
                if(hfShowParentCategory.Value == "1")
                {
                    dvParentCategory.Visible = true;
                }
                hfCustomerInfoOnBill.Value = ds.Tables[6].Rows[0]["CustomerInfoOnBill"].ToString();                
                hfDiscountAuthentication.Value = ds.Tables[6].Rows[0]["DiscountAuthentication"].ToString();
                hfBillFormat.Value = ds.Tables[6].Rows[0]["BillFormat"].ToString();
                Session.Add("BillFormat", hfBillFormat.Value);
                hfCustomerAdvance.Value = ds.Tables[6].Rows[0]["CustomerAdvance"].ToString();
                hfCanVoidOrder.Value = ds.Tables[6].Rows[0]["CanVoidOrder"].ToString();
                hfTakeawayTokenIDMandatory.Value = ds.Tables[6].Rows[0]["TakeawayTokenIDMandatory"].ToString();
                hfShowModifirPriceOnBills.Value = ds.Tables[6].Rows[0]["ShowModifirPriceOnBills"].ToString();
                hfEatIn.Value = ds.Tables[6].Rows[0]["EatIn"].ToString();
                hfOwnOrderBookerDataOnTab.Value = ds.Tables[6].Rows[0]["OwnOrderBookerDataOnTab"].ToString();
                hfServiceChargesOnTakeaway.Value = ds.Tables[6].Rows[0]["ServiceChargesOnTakeaway"].ToString();
                Session.Add("EatIn", hfEatIn.Value);
                hfInvoiceFormat.Value = ds.Tables[6].Rows[0]["InvoiceFormat"].ToString();
                hfKOTFormat.Value = ds.Tables[6].Rows[0]["KOTFormat"].ToString();
                hfCustomerMandatoryOnPOS.Value = ds.Tables[6].Rows[0]["CustomerMandatoryOnPOS"].ToString();
                hfOrderNOInPendingBills.Value = ds.Tables[6].Rows[0]["OrderNOInPendingBills"].ToString();
                hfDailySalesReportColumns.Value = ds.Tables[6].Rows[0]["DailySalesReportColumns"].ToString();
                hfHidePrintInvoiceButton.Value = ds.Tables[6].Rows[0]["HidePrintInvoiceButton"].ToString();
                hfPartialPayment.Value = ds.Tables[6].Rows[0]["PartialPayment"].ToString();
                hfPendigBillRefreshTime.Value = ds.Tables[6].Rows[0]["PendigBillRefreshTime"].ToString();
                Session.Add("CallCenter", ds.Tables[6].Rows[0]["CallCenter"].ToString());
                if (hfAutoPromotion.Value == "1")
                {
                    PromotionController or = new PromotionController();
                    SKUGroupController gController = new SKUGroupController();
                    DataTable dtPromotion = or.GetPromotion(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), Convert.ToDateTime(HttpContext.Current.Session["CurrentWorkDate"]));
                    hftblPromotion.Value = GetJson(dtPromotion);
                    DataTable dtGroup = gController.GetSKUGroupDetail();
                    hftblGroupDetail.Value = GetJson(dtGroup);
                }
            }
            if (ds.Tables[7].Rows.Count > 0)
            {
                hfEmployeeDiscountType.Value = GetJson(ds.Tables[7]);
            }
            if (ds.Tables[9].Rows.Count > 0)
            {
                hfPaymentModes.Value = GetJson(ds.Tables[9]);
            }
            // get printers list product section wise ... [by Adnan Aslam 2017-09-14]
            try
            {
                SkuController _skuController = new SkuController();
                ProductSectionRequest request = new ProductSectionRequest();
                ProductSectionResponse response = _skuController.GetProductSectionsWithPrinters(request);
                if (response.IsException == false)
                    Session["SectionPrinterList"] = response.ProductSectionList;
            }
            catch { }

            #region Stock Validation

            DataTable dt = (DataTable)Session["dtAppSettingDetail"];
            if (dt.Rows.Count > 0)
            {
                bool ClosingStockStatus = false;
                if(dt.Rows[0]["ShowClosingStockStatus"].ToString() == "1")
                {
                    ClosingStockStatus = true;
                }
                hfStockStatus.Value = Convert.ToString(ClosingStockStatus);
                bool IsFinanceIntegrate = Convert.ToInt32(dt.Rows[0]["IsFinanceIntegrate"]) == 1 ? true : false;
                DataTable dtCOAConfig = GetCOAConfiguration();
                HttpContext.Current.Session.Add("dtCOAConfig", dtCOAConfig);
                HttpContext.Current.Session.Add("IsFinanceIntegrate", IsFinanceIntegrate);
            }

            #endregion

            if(Session["IsKOTServiceInstalled"].ToString() == "1")
            {
                hfBookingType.Value = "0";
            }
            else
            {
                hfBookingType.Value = "1";
            }
        }
    }
    //=======  Page Load #Stock Validation region==============\\

    [WebMethod]
    [ScriptMethod]
    public static string SelectPendingBills(string customerType)
    {
        int customerTypeId = -1;

        if (customerType == "Dine In")
        {
            customerTypeId = 1;
        }
        else if (customerType == "Delivery")
        {
            customerTypeId = 2;
        }
        else if (customerType == "Takeaway")
        {
            customerTypeId = 3;
        }

        var mSkuController = new OrderEntryController();
        DataSet ds = mSkuController.SelectPendingBillsDataset(Constants.LongNullValue, DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), int.Parse(HttpContext.Current.Session["UserID"].ToString()), customerTypeId, Convert.ToBoolean(HttpContext.Current.Session["hfPrintKOT"]), 17);
        return JsonConvert.SerializeObject(ds, Formatting.None);
    }
    public string GetConfigValue(int Code, DataTable dt, DataRow[] dr)
    {
        try
        {
            dr = dt.Select("CODE = '" + Code + "' ");
            return dr[0][2].ToString();
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + EX + "');", true);
            return null;
        }
    }

    private DataTable GetCOAConfiguration()
    {
        try
        {
            COAMappingController _cController = new COAMappingController();
            DataTable dtCOAConfig = _cController.SelectCOAConfiguration(5, Constants.ShortNullValue, Constants.LongNullValue, "Level 4");

            if (dtCOAConfig.Rows.Count > 0)
            {
                return dtCOAConfig;
            }

            return null;
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Plz Configure Financial Integration Settings');", true);

            return null;
        }
    }

    protected void lnkExit_OnClick(object sender, EventArgs e)
    {
        // Server.Transfer("Level.aspx?LevelID=27&LevelType=1");
        UserController UserCtl = new UserController();

        UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
        Session.Clear();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("../Login.aspx");
    }

    private bool IsDayClosed()
    {
        DistributorController DistrCtl = new DistributorController();
        try
        {
            DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), 3);
            if (dtDayClose.Rows.Count > 0)
            {
                if (Convert.ToDateTime(Session["CurrentWorkDate"]) == Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
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

    public void GetAppSettingDetail()
    {
        try
        {
            AppSettingDetail _cController = new AppSettingDetail();
            DataTable dtAppSetting = _cController.GetAppSettingDetail(1);
            if (dtAppSetting.Rows.Count > 0)
            {
                Session.Add("dtAppSettingDetail", dtAppSetting);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }
}