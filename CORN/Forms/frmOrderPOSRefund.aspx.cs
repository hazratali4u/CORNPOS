using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;

public partial class Forms_frmOrderPOSRefund : System.Web.UI.Page
{
    OrderEntryController oc = new OrderEntryController();
    DataControl dc = new DataControl();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadInvoice(Convert.ToDateTime(Session["CurrentWorkDate"]));
            this.LoadInvoiceDetail();
            DataTable dtCOAConfig = GetCOAConfiguration();
            Session.Add("dtCOAConfig", dtCOAConfig);
            GetAppSettingDetail();
            txtDate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
            txtDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadInvoice(DateTime dtWorkingDate)
    {
        try
        {
            ddlInvoiceNo.Items.Clear();
            OrderEntryController order = new OrderEntryController();
            DataTable dtInvoice = order.SelectPendingBills(Constants.LongNullValue, dtWorkingDate, Convert.ToInt32(Session["DISTRIBUTOR_ID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(ddlServiceType.SelectedItem.Value), 20);
            clsWebFormUtil.FillDxComboBoxList(ddlInvoiceNo, dtInvoice, "SALE_INVOICE_ID", "InvoiceNo", true);
            if (dtInvoice.Rows.Count > 0)
            {
                ddlInvoiceNo.SelectedIndex = 0;
            }
            else
            {
                ddlInvoiceNo.SelectedIndex = -1;
            }
            Session.Add("dtInvoice", dtInvoice);
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Error:   " + EX.Message.ToString() + " ');", true);
        }
    }
    protected void btnRefund_Click(object sender, EventArgs e)
    {
        DataTable dtItems = new DataTable();
        dtItems.Columns.Add("SKU_ID", typeof(int));
        dtItems.Columns.Add("Quantity", typeof(decimal));
        dtItems.Columns.Add("Price", typeof(decimal));
        dtItems.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
        dtItems.Columns.Add("IsDelete", typeof(bool));
        dtItems.Columns.Add("ItemType", typeof(int));
        dtItems.Columns.Add("BRAND_ID", typeof(string));
        dtItems.Columns.Add("ISEXEMPTED", typeof(string));
        dtItems.Columns.Add("IS_Recipe", typeof(string));
        dtItems.Columns.Add("VOID", typeof(bool));
        dtItems.Columns.Add("IS_FREE", typeof(bool));
        dtItems.Columns.Add("I_D_ID", typeof(int));
        dtItems.Columns.Add("A_PRICE", typeof(decimal));
        dtItems.Columns.Add("DEAL_QTY", typeof(decimal));

        DataRow dr;
        decimal GrossAmount = 0;
        decimal Discount = 0;
        decimal GST = 0;
        decimal ServiceCharges = 0;
        decimal AMOUNTDUE = 0;
        decimal DISCOUNTItemWise = 0;

        decimal GrossAmountRef = 0;
        decimal DiscountRef = 0;
        decimal GSTRef = 0;
        decimal ServiceChargesRef = 0;
        decimal DISCOUNTItemWiseRef = 0;
        int CUSTOMER_TYPE_ID = 1;
        int PAYMENT_MODE_ID = 0;
        int DELIVERY_CHANNEL = 0;
        int BANK_ID = 0;

        DataTable dtInvoice = (DataTable)Session["dtInvoice"];
        foreach (DataRow dr2 in dtInvoice.Rows)
        {
            if (dr2["SALE_INVOICE_ID"].ToString() == ddlInvoiceNo.SelectedItem.Value.ToString())
            {
                GST = Convert.ToDecimal(dr2["GST"]);
                AMOUNTDUE = Convert.ToDecimal(dr2["AMOUNTDUE"]);
                Discount = Convert.ToDecimal(dr2["DISCOUNT2"]);
                DISCOUNTItemWise = Convert.ToDecimal(dr2["ITEM_DISCOUNT"]);
                if (int.Parse(dc.chkNull_0(dr2["DISCOUNT_TYPE"].ToString())) == 0)
                {
                    DiscountRef = GrossAmount * decimal.Parse(dc.chkNull_0(dr2["DISCOUNT2"].ToString())) / 100;
                }
                else
                {
                    DiscountRef = decimal.Parse(dc.chkNull_0(dr2["DISCOUNT2"].ToString()));
                }
                CUSTOMER_TYPE_ID = Convert.ToInt32(dr2["CUSTOMER_TYPE_ID"]);
                PAYMENT_MODE_ID = Convert.ToInt32(dr2["PAYMENT_MODE_ID"]);
                DELIVERY_CHANNEL = Convert.ToInt32(dr2["DELIVERY_CHANNEL"]);
                BANK_ID = Convert.ToInt32(dr2["BANK_ID"]);
                break;
            }
        }

        decimal Amount = 0;
        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            TextBox txtReturnQty = (TextBox)gvr.Cells[4].FindControl("txtReturnQty");
            DropDownList ddlType = (DropDownList)gvr.Cells[7].FindControl("ddlType");
            if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) > 0)
            {
                if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) <= Convert.ToDecimal(gvr.Cells[2].Text))
                {
                    Amount = Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) * Convert.ToDecimal(gvr.Cells[3].Text);
                    DISCOUNTItemWiseRef += Convert.ToDecimal(dc.chkNull_0(gvr.Cells[6].Text));
                    GSTRef += (Amount / AMOUNTDUE) * GST;
                    DiscountRef += (Amount / AMOUNTDUE) * DiscountRef;

                    dr = dtItems.NewRow();
                    dr["SKU_ID"] = gvr.Cells[0].Text;
                    dr["Quantity"] = dc.chkNull_0(txtReturnQty.Text);
                    dr["Price"] = gvr.Cells[3].Text;
                    dr["SALE_INVOICE_DETAIL_ID"] = gvr.Cells[5].Text;
                    if (Convert.ToDecimal(txtReturnQty.Text) == Convert.ToDecimal(gvr.Cells[2].Text))
                    {
                        dr["IsDelete"] = true;
                    }
                    else
                    {
                        dr["IsDelete"] = false;
                    }
                    dr["ItemType"] = ddlType.SelectedValue;
                    dr["BRAND_ID"] = gvr.Cells[8].Text;
                    dr["ISEXEMPTED"] = gvr.Cells[9].Text;
                    dr["IS_Recipe"] = gvr.Cells[10].Text;
                    dr["VOID"] = gvr.Cells[11].Text;
                    dr["IS_FREE"] = gvr.Cells[12].Text;
                    dr["I_D_ID"] = gvr.Cells[13].Text;
                    dr["A_PRICE"] = gvr.Cells[3].Text;
                    dr["DEAL_QTY"] = gvr.Cells[14].Text;
                    GrossAmountRef += Amount;
                    dtItems.Rows.Add(dr);
                }
            }
        }
        if (dtItems.Rows.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtAppSettingDetail"];
            bool IsFinanceIntegrate = false;
            if (dt.Rows.Count > 0)
            {
                IsFinanceIntegrate = Convert.ToInt32(dt.Rows[0]["IsFinanceIntegrate"]) == 1 ? true : false;                
            }
            DataTable dtCOAConfig = (DataTable)Session["dtCOAConfig"];
            if (oc.Add_SalesRefund(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), Convert.ToInt64(ddlInvoiceNo.SelectedItem.Value), CUSTOMER_TYPE_ID, PAYMENT_MODE_ID, DELIVERY_CHANNEL, BANK_ID, AMOUNTDUE, Discount, GST, ServiceCharges, DISCOUNTItemWise, GrossAmountRef, DiscountRef, GSTRef, ServiceChargesRef, DISCOUNTItemWiseRef, Convert.ToInt32(Session["UserID"]), Session["LocationWiseRecipe"].ToString(), Convert.ToDateTime(Session["CurrentWorkDate"]), dtItems, IsFinanceIntegrate, dtCOAConfig) > Constants.LongNullValue)
            {
                this.LoadInvoice(Convert.ToDateTime(txtDate.Text));
                this.LoadInvoiceDetail();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Sales Refund Saved successfully!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No item found.');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmOrderPOSRefund.aspx");
    }
    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadInvoice(Convert.ToDateTime(txtDate.Text));
        LoadInvoiceDetail();
    }
    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadInvoiceDetail();
    }
    private void LoadInvoiceDetail()
    {
        GrdPurchase.DataSource = null;
        GrdPurchase.DataBind();
        DataTable dtInvoice = (DataTable)Session["dtInvoice"];
        foreach (DataRow dr in dtInvoice.Rows)
        {
            if (dr["SALE_INVOICE_ID"].ToString() == ddlInvoiceNo.SelectedItem.Value.ToString())
            {
                if (dr["CUSTOMER_TYPE_ID"].ToString() == "1")
                {
                    lblServiceType.InnerText = "Dine In";
                }
                else if (dr["CUSTOMER_TYPE_ID"].ToString() == "2")
                {
                    lblServiceType.InnerText = "Delivery";
                }
                else if (dr["CUSTOMER_TYPE_ID"].ToString() == "3")
                {
                    lblServiceType.InnerText = "Takeaway";
                }
                if (dr["PAYMENT_MODE_ID"].ToString() == "2")
                {
                    lblPaymentMode.InnerText = "Payment Mode: Credit";
                }
                else if (dr["PAYMENT_MODE_ID"].ToString() == "1")
                {
                    lblPaymentMode.InnerText = "Payment Mode: Credit Card";
                }
                else
                {
                    lblPaymentMode.InnerText = "Payment Mode: Cash";
                }
                lblCustomer.InnerText = "Customer Name: " + dr["CUSTOMER_NAME"].ToString();
                lblGrossLabel.InnerText = "G. Amount";
                lblGrossAmount.InnerText = Convert.ToDecimal(dr["AMOUNTDUE"]).ToString("F2");
                lblDiscountLable.InnerText = "Discount";
                lblDiscount.InnerText = Convert.ToDecimal(dr["DISCOUNT"]).ToString("F2");
                lblGSTLabel.InnerText = "GST";
                lblGST.InnerText = Convert.ToDecimal(dr["GST"]).ToString("F2");
                lblSCLabel.InnerText = "S. Charges";
                if (Convert.ToDecimal(dr["SERVICE_CHARGES"]) > 0)
                {
                    lblSC.InnerText = Convert.ToDecimal(dr["SERVICE_CHARGES"]).ToString("F2");
                }
                else
                {
                    lblSC.InnerText = "0";
                }
                lblNetAmountLable.InnerText = "Net Amount";
                lblNetAmount.InnerText = Convert.ToDecimal(dr["NetAmount"]).ToString("F2");
            }
        }
        if (ddlInvoiceNo.Items.Count > 0)
        {
            SkuController sku = new SkuController();
            DataTable dtItem = sku.SpGetPendingBill(Convert.ToInt64(ddlInvoiceNo.SelectedItem.Value), 9, Constants.IntNullValue);
            GrdPurchase.DataSource = dtItem;
            GrdPurchase.DataBind();
        }
    }

    protected void btnDone_Click(object sender, EventArgs e)
    {
        bool IsReturned = false;
        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            TextBox txtReturnQty = (TextBox)gvr.Cells[4].FindControl("txtReturnQty");
            if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) > 0)
            {
                if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) <= Convert.ToDecimal(gvr.Cells[2].Text))
                {
                    IsReturned = true;
                    break;
                }
            }            
        }
        if (IsReturned)
        {
            DataTable dtSummary = new DataTable();
            dtSummary.Columns.Add("SKU_NAME", typeof(string));
            dtSummary.Columns.Add("Quantity", typeof(decimal));
            DataRow drSummary;
            foreach (GridViewRow gvr in GrdPurchase.Rows)
            {
                TextBox txtReturnQty = (TextBox)gvr.Cells[4].FindControl("txtReturnQty");
                if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) > 0)
                {
                    if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) <= Convert.ToDecimal(gvr.Cells[2].Text))
                    {
                        drSummary = dtSummary.NewRow();
                        drSummary["SKU_NAME"] = gvr.Cells[1].Text;
                        drSummary["Quantity"] = dc.chkNull_0(txtReturnQty.Text);
                        dtSummary.Rows.Add(drSummary);
                    }
                }
            }
            gvSummary.DataSource = dtSummary;
            gvSummary.DataBind();

            decimal GrossAmount = 0;
            decimal GST = 0;
            decimal AMOUNTDUE = 0;
            decimal DISCOUNTItemWise = 0;

            decimal GrossAmountRef = 0;
            decimal DiscountRef = 0;
            decimal GSTRef = 0;
            decimal DISCOUNTItemWiseRef = 0;

            DataTable dtInvoice = (DataTable)Session["dtInvoice"];
            foreach (DataRow dr2 in dtInvoice.Rows)
            {
                if (dr2["SALE_INVOICE_ID"].ToString() == ddlInvoiceNo.SelectedItem.Value.ToString())
                {
                    GST = Convert.ToDecimal(dr2["GST"]);
                    AMOUNTDUE = Convert.ToDecimal(dr2["AMOUNTDUE"]);
                    DISCOUNTItemWise = Convert.ToDecimal(dr2["ITEM_DISCOUNT"]);
                    if (int.Parse(dc.chkNull_0(dr2["DISCOUNT_TYPE"].ToString())) == 0)
                    {
                        DiscountRef = GrossAmount * decimal.Parse(dc.chkNull_0(dr2["DISCOUNT2"].ToString())) / 100;
                    }
                    else
                    {
                        DiscountRef = decimal.Parse(dc.chkNull_0(dr2["DISCOUNT2"].ToString()));
                    }                    
                    break;
                }
            }
            decimal Amount = 0;
            foreach (GridViewRow gvr in GrdPurchase.Rows)
            {
                TextBox txtReturnQty = (TextBox)gvr.Cells[4].FindControl("txtReturnQty");
                if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) > 0)
                {
                    if (Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) <= Convert.ToDecimal(gvr.Cells[2].Text))
                    {
                        Amount = Convert.ToDecimal(dc.chkNull_0(txtReturnQty.Text)) * Convert.ToDecimal(gvr.Cells[3].Text);
                        DISCOUNTItemWiseRef += Convert.ToDecimal(dc.chkNull_0(gvr.Cells[6].Text));
                        GSTRef += (Amount / AMOUNTDUE) * GST;
                        DiscountRef += (Amount / AMOUNTDUE) * DiscountRef;
                        GrossAmountRef += Amount;
                    }
                }
            }
            lblRefundGross.Text = "Gross Amount: " + GrossAmountRef.ToString("F2");
            lblRefundDiscount.Text = "Discount: " + DiscountRef.ToString("F2");
            lblRefundGST.Text = "GST: " + GSTRef.ToString("F2");
            lblRefundTotal.Text = "Refund Amount:" + (GrossAmountRef + GSTRef - DiscountRef).ToString("F2");
            string script = "$('#dvSummary').lightbox_me({ centered: true });";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowLightbox", script, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No item found.');", true);
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

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        LoadInvoice(Convert.ToDateTime(txtDate.Text));
        LoadInvoiceDetail();
    }
}