using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.IO;

public partial class Forms_frmRollbackInvoice : System.Web.UI.Page
{
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
            this.GetAppSettingDetail();
            LoadDistributor();
            LoadRollBackReason();
            if (Session["TaxIntegration"].ToString() != "0")
            {
                TaxAuthorityController objTax = new TaxAuthorityController();
                DataTable dtTaxAuthority = objTax.GetTaxAuthority(Constants.IntNullValue, 2, int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
                Session.Add("dtTaxAuthority", dtTaxAuthority);
            }
            if (Session["RollBackType"].ToString() == "0")
            {
                DataTable dtFinance = (DataTable)Session["dtAppSettingDetail"];
                if (dtFinance.Rows.Count > 0)
                {
                    bool IsFinanceIntegrate = Convert.ToInt32(dtFinance.Rows[0]["IsFinanceIntegrate"]) == 1 ? true : false;
                    if (IsFinanceIntegrate)
                    {
                        DataTable dtCOAConfig = GetCOAConfiguration();
                        HttpContext.Current.Session.Add("dtCOAConfig", dtCOAConfig);
                    }
                    HttpContext.Current.Session.Add("IsFinanceIntegrate", IsFinanceIntegrate);
                }
            }

            DateTime CurrentWorkDate = Constants.DateNullValue;
            DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
            foreach (DataRow dr in dtLocationInfo.Rows)
            {
                if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
                {
                    if (dr["MaxDayClose"].ToString().Length > 0)
                    {
                        CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                        break;
                    }
                }
            }

            if (CurrentWorkDate != null && CurrentWorkDate != Constants.DateNullValue)
            {
                txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");
            }
        }
    }

    #region Load

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);


        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }

        Session.Add("dtLocationInfo", dt);
    }

    private void LoadRollBackReason()
    {
        FranchiseSaleInvoiceController franchiseController = new FranchiseSaleInvoiceController();
        DataTable rollBackDt = franchiseController.SelectROLLBACK_REASON(2);
        clsWebFormUtil.FillDxComboBoxList(this.ddlRollBackReason, rollBackDt, "REASON_ID", "REASON_DESC");

        if (rollBackDt.Rows.Count > 0)
        {
            ddlRollBackReason.SelectedIndex = 0;
        }
    }
    private void LoadRollbackDocument()
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
            {
                if (dr["MaxDayClose"].ToString().Length > 0)
                {
                    CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                    break;
                }
            }
        }

        var or = new OrderEntryController();
        DataTable dtOrder = or.SelectRollBackDocument(Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.IntNullValue,
            Constants.IntNullValue, int.Parse(DrpDocumentType.SelectedValue), CurrentWorkDate);
        GrdOrder.DataSource = dtOrder;
        GrdOrder.DataBind();
    }

    #endregion

    #region Click

    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (IsDayClosed())
        {
            UserController UserCtl = new UserController();

            UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }

        var ord = new OrderEntryController();
        foreach (GridViewRow dr in GrdOrder.Rows)
        {
            var chbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
            if (chbInvoice.Checked == true)
            {
                bool IsFinanceIntegrate = (bool)Session["IsFinanceIntegrate"];
                DataTable dtCOAConfig = (DataTable)Session["dtCOAConfig"];
                if (ord.UpdateRollBackDocument(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(DrpDocumentType.SelectedValue), int.Parse(DrpLenged.SelectedValue), Convert.ToInt32(Session["UserID"]), Convert.ToInt16(ddlRollBackReason.SelectedItem.Value), Session["RollBackType"].ToString(), Convert.ToInt32(dr.Cells[11].Text), Convert.ToDateTime(Session["CurrentWorkDate"]), Convert.ToInt32(dr.Cells[12].Text), Convert.ToInt32(dr.Cells[13].Text), Convert.ToInt32(dr.Cells[14].Text), Convert.ToInt32(dr.Cells[15].Text), Convert.ToDecimal(dr.Cells[9].Text), Convert.ToDecimal(dr.Cells[6].Text), Convert.ToDecimal(dr.Cells[8].Text), Convert.ToDecimal(dr.Cells[7].Text), Convert.ToDecimal(dr.Cells[16].Text), IsFinanceIntegrate, dtCOAConfig))
                {
                    //if (Session["TaxIntegration"].ToString() == "1" || Session["TaxIntegration"].ToString() == "2")//FBR or PRA
                    //{
                    //    PostFBRRollbackDocument(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                    //}
                    //else if (Session["TaxIntegration"].ToString() == "4")
                    //{
                    //    PostSRBRollbackDocument(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                    //}
                }
            }
        }
        LoadRollbackDocument();
    }
    private void PostFBRRollbackDocument(long InvoiceID)
    {
        var ord = new OrderEntryController();
        DataTable dtTaxAuthority = (DataTable)Session["dtTaxAuthority"];
        if (dtTaxAuthority.Rows.Count > 0)
        {
            var mSkuController = new SkuController();
            DataTable dtDetail = mSkuController.SpGetPendingBill(InvoiceID, 4, Convert.ToInt32(Session["UserID"]));
            InvoiceFBR objInvoice = new InvoiceFBR();
            List<InvoiceFBRDetail> lstItems = new List<InvoiceFBRDetail>();
            int TotalQty = 0;
            double GrossValue = 0;
            double NetAmount = 0;
            double TotalTax = 0;
            double TaxRate = 0;
            decimal Discount = 0;
            int PaymentMode = 1;

            if (dtDetail.Rows.Count > 0)
            {
                TotalTax = Convert.ToDouble(dtDetail.Rows[0]["GST"]);
                TaxRate = Convert.ToDouble(dtDetail.Rows[0]["GST"]) / Convert.ToDouble(dtDetail.Rows[0]["AMOUNTDUE"]) * 100;

                if (decimal.Parse(dtDetail.Rows[0]["DISCOUNT"].ToString()) > 0)
                {
                    if (int.Parse(dtDetail.Rows[0]["DISCOUNT_TYPE"].ToString()) == 0)
                    {
                        Discount = decimal.Parse(dtDetail.Rows[0]["AMOUNTDUE"].ToString()) * decimal.Parse(dtDetail.Rows[0]["DISCOUNT"].ToString()) / 100;
                    }
                    else
                    {
                        Discount = decimal.Parse(dtDetail.Rows[0]["AMOUNTDUE"].ToString());
                    }
                }
            }
            foreach (DataRow drDetail in dtDetail.Rows)
            {
                InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                ObjInvoiceDetail.ItemCode = drDetail["SKU_ID"].ToString();
                ObjInvoiceDetail.ItemName = drDetail["SKU_NAME"].ToString();
                ObjInvoiceDetail.Quantity = Convert.ToInt32(drDetail["QTY"]);
                ObjInvoiceDetail.SaleValue = Convert.ToDouble(drDetail["AMOUNT"]);
                if (HttpContext.Current.Session["ItemWiseGST"].ToString() == "1")
                {
                    ObjInvoiceDetail.TaxCharged = Convert.ToDouble(drDetail["ItemWiseGST"]);
                    ObjInvoiceDetail.TaxRate = Convert.ToDouble(drDetail["GSTPER"]);
                }
                else
                {
                    ObjInvoiceDetail.TaxCharged = Convert.ToDouble(drDetail["AMOUNT"]) * TaxRate / 100;
                    ObjInvoiceDetail.TaxRate = TaxRate;
                }
                ObjInvoiceDetail.TotalAmount = Convert.ToDouble(drDetail["AMOUNT"]) + ObjInvoiceDetail.TaxCharged;
                ObjInvoiceDetail.PCTCode = "10101";
                ObjInvoiceDetail.FurtherTax = 0;
                ObjInvoiceDetail.InvoiceType = 3;//1=New,2=Debit,3=Credit
                ObjInvoiceDetail.Discount = Discount / dtDetail.Rows.Count;
                ObjInvoiceDetail.RefUSIN = InvoiceID.ToString();
                lstItems.Add(ObjInvoiceDetail);
                TotalQty += ObjInvoiceDetail.Quantity;
                GrossValue += ObjInvoiceDetail.SaleValue;
                NetAmount += ObjInvoiceDetail.TotalAmount;
                TotalTax += ObjInvoiceDetail.TaxCharged;
            }
            if (int.Parse(dtDetail.Rows[0]["PAYMENT_MODE_ID"].ToString()) == 0)
            {
                PaymentMode = 1;
            }
            else if (int.Parse(dtDetail.Rows[0]["PAYMENT_MODE_ID"].ToString()) == 1)
            {
                PaymentMode = 2;
            }
            else
            {
                PaymentMode = 6;//Cheque For Credit invoices
            }

            objInvoice.Items = lstItems;
            objInvoice.InvoiceNumber = string.Empty;
            objInvoice.POSID = dtTaxAuthority.Rows[0]["POSID"].ToString();
            objInvoice.USIN = InvoiceID.ToString() + "Rtrn";
            objInvoice.DateTime = DateTime.Now;
            objInvoice.BuyerNTN = "";
            objInvoice.BuyerCNIC = "";
            objInvoice.BuyerName = "";
            objInvoice.BuyerPhoneNumber = "";
            objInvoice.PaymentMode = PaymentMode;//1=Cash,2=Card,3=Gift Voucher,4=Loyality Card,5=Mixed,6=Cheque
            objInvoice.TotalSaleValue = GrossValue;
            objInvoice.TotalQuantity = TotalQty;
            objInvoice.TotalBillAmount = NetAmount;
            objInvoice.TotalTaxCharged = TotalTax;
            objInvoice.Discount = Discount;
            objInvoice.FurtherTax = 0;
            objInvoice.InvoiceType = 3;
            objInvoice.RefUSIN = InvoiceID.ToString();

            try
            {
                HttpClient Client = new HttpClient();

                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dtTaxAuthority.Rows[0]["Token"].ToString());
                var content = new StringContent(JsonConvert.SerializeObject(objInvoice), Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpResponseMessage response = Client.PostAsync(dtTaxAuthority.Rows[0]["FBRURL"].ToString(), content).Result;

                string InvoiceNumberFBR = string.Empty;
                string CodeFBR = string.Empty;
                if (response.IsSuccessStatusCode)
                {
                    string responseFBR = response.Content.ReadAsStringAsync().Result;
                    InvoiceNumberFBR = JObject.Parse(responseFBR)["InvoiceNumber"].ToString();
                    CodeFBR = JObject.Parse(responseFBR)["Code"].ToString();

                    ord.UpdateInvoiceNumberRollBackTaxAuthority(InvoiceID, InvoiceNumberFBR);
                }
            }
            catch (Exception)
            {

            }
        }
    }
    private void PostSRBRollbackDocument(long InvoiceID)
    {
        var ord = new OrderEntryController();
        DataTable dtTaxAuthority = (DataTable)Session["dtTaxAuthority"];
        if (dtTaxAuthority.Rows.Count > 0)
        {
            var mSkuController = new SkuController();
            DataTable dtDetail = mSkuController.SpGetPendingBill(InvoiceID, 4, Convert.ToInt32(Session["UserID"]));
            double TotalTax = 0;
            double TaxRate = 0;
            double TotalAmount = 0;
            if (dtDetail.Rows.Count > 0)
            {
                TotalTax = Convert.ToDouble(dtDetail.Rows[0]["GST"]);
                TaxRate = Convert.ToDouble(dtDetail.Rows[0]["GST"]) / Convert.ToDouble(dtDetail.Rows[0]["AMOUNTDUE"]) * 100;
                TotalAmount= Convert.ToDouble(dtDetail.Rows[0]["AMOUNTDUE"]);
            }

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(dtTaxAuthority.Rows[0]["FBRURL"].ToString());
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.Method = "POST";
            using (var streamWriter = new StreamWriter(myReq.GetRequestStream()))
            {
                string json = "{'posId':" + dtTaxAuthority.Rows[0]["POSID"].ToString() + ",'name':'Demo Taxpayer', 'ntn':'4694034', 'invoiceId':'" + InvoiceID+ "', 'invoiceDateTime':'" + Convert.ToDateTime(HttpContext.Current.Session["CurrentWorkDate"]).ToString("yyyy-mm-dd") + "', 'rateValue':'" + TaxRate.ToString() + "', 'saleValue':'" + TotalAmount.ToString() + "', 'taxAmount':'" + TotalTax.ToString() + "', 'consumerName':'N/A', 'consumerNTN':'N/A', 'address':'N/A', 'tariffCode':'N/A', 'extraInf':'N/A', 'TransType':'Live','invoiceType':2, 'pos_user' : '" + dtTaxAuthority.Rows[0]["POSUser"].ToString() + "', 'pos_pass':'" + dtTaxAuthority.Rows[0]["POSPassword"].ToString() + "'}";
                json = json.Replace("'", "\"");
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)myReq.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                if (JObject.Parse(result)["resCode"].ToString() == "00")
                {
                    ord.UpdateInvoiceNumberRollBackTaxAuthority(InvoiceID, JObject.Parse(result)["srbInvoceId"].ToString());
                }
            }
        }
    }
    protected void btnGetOrder_Click(object sender, EventArgs e)
    {
        if (IsDayClosed())
        {
            UserController UserCtl = new UserController();

            UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }

        GrdOrder.Visible = true;
        GrdCheque.Visible = false;
        LoadRollbackDocument();

    }
    #endregion
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
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDistributor.Items.Count > 0)
        {
            DateTime CurrentWorkDate = Constants.DateNullValue;
            DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
            foreach (DataRow dr in dtLocationInfo.Rows)
            {
                if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
                {
                    if (dr["MaxDayClose"].ToString().Length > 0)
                    {
                        CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                        break;
                    }
                }
            }

            txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");
        }
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