using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
public partial class Forms_frmCustomerReceipt : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
    readonly LedgerController LController = new LedgerController();
    readonly ChequeBookController _CBController = new ChequeBookController();
    readonly FinancialYearController _yearController = new FinancialYearController();
    readonly LedgerController _ledgerCtl = new LedgerController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetAppSettingDetail();
            GetFinancialYear();
            LoadDistributor();
            LoadAccountHead();            
            LoadAccountDetail();
            LoadData();
            SelectCreditInvoice();
            LoadReceviedCheque();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            Session.Add("dtConfig", dtConfig);
            Session.Add("IsFinanceSetting", IsFinanceSetting);
        }
    }

    private void toggleControls(string pAccountType)
    {
        if (pAccountType == "21" || pAccountType == "34")
        {
            lblChequeNo.Visible = false;
            txtChequeNo.Visible = false;
            lblChequeDate.Visible = false;
            txtStartDate.Visible = false;
            ibtnStartDate.Visible = false;
        }
        else if (pAccountType == "33")
        {
            lblChequeNo.Visible = false;
            txtChequeNo.Visible = false;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            lblChequeDate.Text = "Transfer Date";
        }
        else
        {
            lblChequeNo.Visible = true;
            txtChequeNo.Visible = true;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            lblChequeDate.Text = "Cheque Date";
        }
    }

    #region Load
    public void GetFinancialYear()
    {
        try
        {
            DataTable dt = _yearController.SelectFinancialYear(null, null, null, null, Constants.ShortNullValue, false, true, 1);
            DateTime StartDate = Convert.ToDateTime(dt.Rows[0]["dtStart"].ToString());
            DateTime EndDate = Convert.ToDateTime(dt.Rows[0]["dtEnd"].ToString());
            CalendarExtender1.StartDate = StartDate;
            CalendarExtender1.EndDate = EndDate;
            if (EndDate >= (DateTime)Session["CurrentWorkDate"])
            {
                txtTransactionDate.Text = ((DateTime)Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
            }
            else
            {
                txtTransactionDate.Text = (EndDate).ToString("dd-MMM-yyyy");
            }

            txtTransactionDate.Attributes.Add("readonly", "readonly");
        }
        catch (IndexOutOfRangeException)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please Start a financial year')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", string.Format("alert('{0}')", ex.Message), true);
        }
    }

    private void LoadPaymentRecieved()
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
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            ChequeEntryController CController = new ChequeEntryController();
            LedgerController LController = new LedgerController();
            if (drpDistributor.Items.Count > 0)
            {
                DataSet dsReceived = LController.SelectBankCashTransction(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, 21, CurrentWorkDate);
                DataTable dtRealized = dsReceived.Tables[2];
                if (dtRealized.Rows.Count > 0)
                {
                    lblAmount.Text = string.Format("{0:0,0.00}", Convert.ToDecimal(dtRealized.Rows[0][0].ToString()));

                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);

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
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            txtTransactionDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
            CalendarExtender1.EndDate = CurrentWorkDate;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            return;
        }
    }

    private void LoadData()
    {
        int TypeID = 2;
        if (ddlRecieptType.SelectedItem.Value.ToString() == "1")
        {
            TypeID = 1;
        }
        else if (ddlRecieptType.SelectedItem.Value.ToString() == "4" || ddlRecieptType.SelectedItem.Value.ToString() == "5")
        {
            TypeID = 4;
        }
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        DataTable dtCredit = _ledgerCtl.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, Constants.LongNullValue, TypeID);
        clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dtCredit, 0, 1, true);

        if (dtCredit.Rows.Count > 0)
        {
            ddlCustomer.SelectedIndex = 0;
        }
    }

    private void LoadReceviedCheque()
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
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            ChequeEntryController CController = new ChequeEntryController();
            GrdCO.DataSource = null;
            GrdCO.DataBind();
            decimal chqAmount = 0;
            if (drpDistributor.Items.Count > 0)
            {
                string slipNo = "";
                int TypeID = Constants.IntNullValue;
                if (ddlRecieptType.SelectedItem.Value.ToString() == "2")
                {
                    TypeID = 1;
                    slipNo = "FranchiseReceipt";
                }
                DataTable dt = LController.SelectBankCashTransction(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), CurrentWorkDate, CurrentWorkDate, TypeID, slipNo);
                if (TypeID != 1)
                {
                    DataTable newDt = new DataTable();
                    DataRow[] dt1 = dt.Select("SLIP_NO <> 'FranchiseReceipt'");

                    if (dt1 != null && dt1.Length > 0)
                    {
                        newDt = dt1.CopyToDataTable();
                    }

                    Session.Add("dt", newDt);
                    GrdCO.DataSource = newDt;
                    GrdCO.DataBind();
                }
                else
                {
                    Session.Add("dt", dt);
                    GrdCO.DataSource = dt;
                    GrdCO.DataBind();
                }
                if (dt != null)
                {
                    foreach (DataRow gvr in dt.Rows)
                    {
                        chqAmount += Convert.ToDecimal(gvr["Balance"]);
                    }
                    lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }

    private void checkDuplication()
    {
        DataTable dt = (DataTable)Session["dt"];

        DataRow[] foundRows = dt.Select("VENDOR_ID = '" + ddlCustomer.SelectedItem.Value.ToString() + "' and CHEQUE_NO='" + txtChequeNo.Text + "'");

        if (foundRows.Length > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Cheque No Already exist against this vendor!');", true);
        }
    }

    private void SelectCreditInvoice()
    {
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        if (ddlRecieptType.SelectedItem.Value.ToString() != "3" && ddlRecieptType.SelectedItem.Value.ToString() != "4")
        {
            LedgerController CDC = new LedgerController();
            int areadid = 0;
            if (ddlRecieptType.SelectedItem.Value.ToString() == "2")
            {
                areadid = 3;
            }
            else if (ddlRecieptType.SelectedItem.Value.ToString() == "5")
            {
                areadid = 5;
            }
            if (ddlCustomer.Items.Count > 0)
            {
                DataTable dtCredit = CDC.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), areadid);

                if (ddlRecieptType.Value.ToString() != "2" && ddlRecieptType.Value.ToString() != "5")
                {
                    GrdCredit.DataSource = dtCredit;
                    GrdCredit.DataBind();
                    CheckBox cb = null;
                    decimal TotalAmount = 0;
                    foreach (GridViewRow gvr in GrdCredit.Rows)
                    {
                        cb = gvr.Cells[0].FindControl("ChbIsAssigned") as CheckBox;
                        cb.Checked = true;
                        TotalAmount += Convert.ToDecimal(gvr.Cells[4].Text);
                    }
                    txtTotalCreditAmount.Text = TotalAmount.ToString("N2");
                }
                else
                {
                    if (dtCredit.Rows.Count > 0)
                    {
                        txtTotalCreditAmount.Text = dtCredit.Rows[0]["CURRENT_CREDIT_AMOUNT"]
                            .ToString();
                    }
                    else
                    {
                        txtTotalCreditAmount.Text = "0.00";
                    }
                }
            }
        }
    }

    private void LoadAccountHead()
    {
        int TypeID = 2;
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            TypeID = 4;
        }

        AccountHeadController mAccountController = new AccountHeadController();
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            DataTable dt = mAccountController.SelectAccountHeadByMapping(Constants.AC_AccountHeadId, Constants.LongNullValue, TypeID, Constants.AC_CashInHandAccountHead,Convert.ToInt32(drpDistributor.Value));
            clsWebFormUtil.FillDxComboBoxList(DrpBankAccount, dt, 0, 4, true);

            if (dt.Rows.Count > 0)
            {
                DrpBankAccount.SelectedIndex = 0;
            }
        }
        else
        {
            DataTable dt = mAccountController.SelectAccountHeadByMapping(Constants.AC_AccountHeadId, Constants.LongNullValue, TypeID, Constants.AC_BankAccountHead, Convert.ToInt32(drpDistributor.Value));
            clsWebFormUtil.FillDxComboBoxList(DrpBankAccount, dt, 0, 4, true);

            if (dt.Rows.Count > 0)
            {
                DrpBankAccount.SelectedIndex = 0;
            }
        }

    }

    private void LoadAccountDetail()
    {
        int TypeID = 1;
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            TypeID = 5;
        }
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dtHead = mAccountController.SelectAccountHeadLocation(Constants.AC_AccountHeadId, Convert.ToInt32(drpDistributor.Value), TypeID);
        clsWebFormUtil.FillDxComboBoxList(this.ddlAccountHead, dtHead, "ACCOUNT_HEAD_ID", "ACCOUNT_NAME", true);

        if (dtHead.Rows.Count > 0)
        {
            ddlAccountHead.SelectedIndex = 0;
        }
    }

    #endregion

    private long RoyaltyAdvance(DateTime CurrentWorkDate)
    {
        string remarks = "Royalty Cash " + txtRemarks.Text;
        int DocumentTypeID = Constants.Document_FranchiseSale;
        if (ddlRecieptType.SelectedItem.Value.ToString() == "4")
        {
            remarks = "Advance Cash " + txtRemarks.Text;
            DocumentTypeID = Constants.Document_SaleInvoice;
        }
        DateTime chqDate = Constants.DateNullValue;
        int VoucherType = Constants.CashPayment_Voucher;
        if (DrpAccountType.SelectedItem.Value.ToString() == "33")
        {
            chqDate = Convert.ToDateTime(txtStartDate.Text);
            VoucherType = Constants.Expanse_Voucher;
            if (ddlRecieptType.SelectedItem.Value.ToString() == "4")
            {
                remarks = "Advance Online Transfer " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
            }
            else
            {
                remarks = "Royalty Online Transfer " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
            }
        }
        string MaxDocumentID = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
        Session.Add("VoucherNo", MaxDocumentID);
        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];
        long ledgerId = 0;

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        if (PayableAccount == 0)
        {
            dtConfig = GetCOAConfiguration();
            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
            PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        }
        string SlipNo = "Realty";
        if (ddlRecieptType.SelectedItem.Value.ToString() == "4")
        {
            SlipNo = "Advance";
        }
        ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, decimal.Parse(dc.chkNull_0(txtAmount.Text)), CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase, null, int.Parse(Session["UserId"].ToString()), 0, "0", DocumentTypeID, SlipNo, chqDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");
        LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), decimal.Parse(dc.chkNull_0(txtAmount.Text)), 0, CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase, null, int.Parse(Session["UserId"].ToString()), 0, "0", DocumentTypeID, SlipNo, chqDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");
        return ledgerId;
    }

    private long AdvanceReturn(DateTime CurrentWorkDate)
    {
        string remarks = "Advance Cash " + txtRemarks.Text;
        int DocumentTypeID = Constants.Document_SaleInvoice;
        
        DateTime chqDate = Constants.DateNullValue;
        int VoucherType = Constants.CashPayment_Voucher;
        if (DrpAccountType.SelectedItem.Value.ToString() == "33")
        {
            chqDate = Convert.ToDateTime(txtStartDate.Text);
            VoucherType = Constants.Expanse_Voucher;
            remarks = "Advance Retrun Online Transfer " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        string MaxDocumentID = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
        Session.Add("VoucherNo", MaxDocumentID);
        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];
        long ledgerId = 0;

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        if (PayableAccount == 0)
        {
            dtConfig = GetCOAConfiguration();
            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
            PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        }
        string SlipNo = "Advance Return";

        ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), decimal.Parse(dc.chkNull_0(txtAmount.Text)), 0, CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase, null, int.Parse(Session["UserId"].ToString()), 0, "0", DocumentTypeID, SlipNo, chqDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");
        LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, decimal.Parse(dc.chkNull_0(txtAmount.Text)), CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase, null, int.Parse(Session["UserId"].ToString()), 0, "0", DocumentTypeID, SlipNo, chqDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");
        return ledgerId;
    }

    private long ChequeRealization(DateTime CurrentWorkDate)
    {
        DateTime ChequeDate = Constants.DateNullValue;
        try
        {
            if (DrpAccountType.SelectedItem.Value.ToString() == "18" || DrpAccountType.SelectedItem.Value.ToString() == "33")
            {
                ChequeDate = DateTime.Parse(txtStartDate.Text);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
            return 0;
        }

        LedgerController LController = new LedgerController();
        string MaxDocumentId = "";
        int VoucherType = Constants.Expanse_Voucher;

        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        decimal realizeAmount = 0;

        string remarks = "";


        if (DrpAccountType.SelectedItem.Value.ToString() == "18")//Cheque
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Chq# " + txtChequeNo.Text + ", " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "33")//Online
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Online Transfer " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "34")//Credit Card
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Credit Card " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "21")//Cash
        {
            VoucherType = Constants.Cash_Voucher;
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Cash " + txtRemarks.Text;
        }
        Session.Add("VoucherNo", MaxDocumentId);

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];
        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        string ChequeNo = null;
        if (DrpAccountType.SelectedIndex == 0)
        {
            ChequeNo = txtChequeNo.Text;
        }
        string manualNo = "";
        long ledgerId = 0;
        foreach (GridViewRow dr in GrdCredit.Rows)
        {
            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
            if (chRelized.Checked == true)
            {
                manualNo = dr.Cells[1].Text;
                if (Convert.ToString(dr.Cells[1].Text) == "opng")
                {
                    manualNo = "opng";
                }
                if (decimal.Parse(dr.Cells[4].Text) >= OfferAmount)
                {
                    realizeAmount += OfferAmount;
                    ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, OfferAmount,
                    CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0,
                    ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Convert.ToInt32(GrdCredit.Rows[dr.RowIndex].Cells[5].Text), "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

                    LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), Convert.ToInt32(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, 0,
                    CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0,
                    ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Convert.ToInt32(GrdCredit.Rows[dr.RowIndex].Cells[5].Text), "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

                    OfferAmount = decimal.Parse(dr.Cells[4].Text) - OfferAmount;

                    LController.UpdateLedgerInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, Convert.ToInt32(GrdCredit.Rows[dr.RowIndex].Cells[5].Text));

                    break;
                }
                else if (decimal.Parse(dr.Cells[4].Text) <= OfferAmount)
                {
                    realizeAmount += decimal.Parse(dr.Cells[4].Text);
                    ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, decimal.Parse(dr.Cells[4].Text),
                    CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase,
                     ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Convert.ToInt32(GrdCredit.Rows[dr.RowIndex].Cells[5].Text), "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

                    LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), Convert.ToInt32(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), decimal.Parse(dr.Cells[4].Text), 0,
                    CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase,
                     ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Convert.ToInt32(GrdCredit.Rows[dr.RowIndex].Cells[5].Text), "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

                    OfferAmount = OfferAmount - decimal.Parse(dr.Cells[4].Text);
                    LController.UpdateLedgerInvoice(long.Parse(dr.Cells[1].Text), int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, Convert.ToInt32(GrdCredit.Rows[dr.RowIndex].Cells[5].Text));
                }
            }
        }

        //Advance Entry
        if (realizeAmount < decimal.Parse(txtAmount.Text))
        {
            ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, OfferAmount,
                                  CurrentWorkDate, remarks + " (Advance)", DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase,
                                  ChequeNo, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

            LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), Convert.ToInt32(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, 0,
                                  CurrentWorkDate, remarks + " (Advance)", DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase,
                                  ChequeNo, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");
        }

        return ledgerId;
    }
    private long ChequeRealization2(DateTime CurrentWorkDate)
    {
        DateTime ChequeDate = Constants.DateNullValue;
        try
        {
            if (DrpAccountType.SelectedItem.Value.ToString() == "18" || DrpAccountType.SelectedItem.Value.ToString() == "33")
            {
                ChequeDate = DateTime.Parse(txtStartDate.Text);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
            return 0;
        }

        LedgerController LController = new LedgerController();
        string MaxDocumentId = "";
        int DocumentTypeID = Constants.Document_FranchiseSale;
        int VoucherType = Constants.Expanse_Voucher;

        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        string remarks = "";


        if (DrpAccountType.SelectedItem.Value.ToString() == "18")//Cheque
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Chq# " + txtChequeNo.Text + ", " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "33")//Online
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Online Transfer " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "34")//Credit Card
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Credit Card " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "21")//Cash
        {
            VoucherType = Constants.Cash_Voucher;
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Cash " + txtRemarks.Text;
        }
        Session.Add("VoucherNo", MaxDocumentId);

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];
        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        string ChequeNo = null;
        if (DrpAccountType.SelectedIndex == 0)
        {
            ChequeNo = txtChequeNo.Text;
        }
        string manualNo = "";
        //foreach (GridViewRow dr in GrdCredit.Rows)
        //{
        long ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, OfferAmount,
        CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0,
        ChequeNo, int.Parse(Session["UserId"].ToString()), 0, manualNo, DocumentTypeID, "FranchiseReceipt", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

        LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), Convert.ToInt32(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, 0,
        CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0,
        ChequeNo, int.Parse(Session["UserId"].ToString()), 0, manualNo, DocumentTypeID, "FranchiseReceipt", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

        //LController.UpdateLedgerInvoice(Convert.ToInt64(ddlVoucher.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, Constants.IntNullValue);

        //}

        return ledgerId;
    }

    #region Sel/Index Change
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Session["dtVendor"] != null)
        //{
        //    DataTable dt = (DataTable)Session["dtVendor"];
        //    DataRow[] foundRows = dt.Select("VENDOR_ID  = '" + ddlCustomer.SelectedValue + "'");
        //    if (foundRows.Length > 0)
        //    {
        //        hfVendorType.Value=Convert.ToString(foundRows[0]["vendorType"]);
        //    }
        //}
        SelectCreditInvoice();
        LoadReceviedCheque();
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
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

        if (CurrentWorkDate != Constants.DateNullValue)
        {
            txtTransactionDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
            CalendarExtender1.EndDate = CurrentWorkDate;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            return;
        }
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            LoadAccountDetail();
            LoadAccountHead();
        }
        LoadData();
        SelectCreditInvoice();
        LoadReceviedCheque();
    }

    protected void DrpAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAccountHead();
        toggleControls(DrpAccountType.SelectedItem.Value.ToString());
        SelectCreditInvoice();
        LoadReceviedCheque();
    }

    #endregion

    #region Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTransactionDate.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
                return;
            }
            DateTime CurrentWorkDate = Convert.ToDateTime(txtTransactionDate.Text);
            ChequeEntryController CController = new ChequeEntryController();
            DateTime ChequeDate = Convert.ToDateTime(txtStartDate.Text);

            int InvoiceCount = Constants.IntNullValue;
            if (ddlRecieptType.SelectedItem.Value.ToString() == "1")
            {
                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        InvoiceCount++;
                        break;
                    }
                }
                if (InvoiceCount == Constants.IntNullValue)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select an invoice');", true);
                    return;
                }
            }
            if (btnSave.Text == "Save")
            {
                if (DrpAccountType.SelectedIndex != 0)//on Cash Realization check Invoice Selection
                {
                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            InvoiceCount++;
                            break;
                        }
                    }
                    if (InvoiceCount == Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                    {
                        long ledgerId = 0;
                        if (ddlRecieptType.Value.ToString() == "2")
                        {
                            ledgerId = ChequeRealization2(CurrentWorkDate);
                        }
                        else if (ddlRecieptType.Value.ToString() == "5")
                        {
                            ledgerId = AdvanceReturn(CurrentWorkDate);
                        }
                        else
                        {
                            ledgerId = RoyaltyAdvance(CurrentWorkDate);
                        }
                        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                        {
                            LedgerController lController = new LedgerController();
                            long voucherNo = lController.SelectVoucherNoByLedgerId(ledgerId);
                            InsertGL2(null, CurrentWorkDate, voucherNo);
                        }
                        Session.Remove("VoucherNo");
                    }
                    else if (InvoiceCount != Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                    {
                        var ledgerId = ChequeRealization(CurrentWorkDate);// used as All cash, online, cheque
                        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                        {
                            string ChequeNo = null;
                            if (DrpAccountType.SelectedIndex == 0)
                            {
                                ChequeNo = txtChequeNo.Text;
                            }

                            LedgerController lController = new LedgerController();
                            long voucherNo = lController.SelectVoucherNoByLedgerId(ledgerId);
                            InsertGL2(ChequeNo, CurrentWorkDate, voucherNo);
                        }
                        Session.Remove("VoucherNo");
                        SelectCreditInvoice();
                    }
                }
                else if (DrpAccountType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
                {
                    short chkDiscount = 0;
                    if (ChkIsDiscount.Checked)
                    {
                        chkDiscount = 1;
                    }
                    HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, Convert.ToInt32(ddlCustomer.SelectedItem.Value), txtChequeNo.Text, txtBankName.Text, ChequeDate,CurrentWorkDate, Constants.DateNullValue, Constants.DateNullValue, Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)), Constants.Cheque_Clear, DateTime.Now, DrpAccountType.SelectedIndex, "", txtRemarks.Text, long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), 2, Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)), chkDiscount,Convert.ToDecimal(dc.chkNull_0(txtTax.Text)),Convert.ToInt32(ddlAccountHead.SelectedItem.Value.ToString()), Constants.DecimalNullValue,false);
                    if (ddlRecieptType.SelectedItem.Value.ToString() == "3" || ddlRecieptType.SelectedItem.Value.ToString() == "4")
                    {
                        var ledgerId = RoyaltyAdvance(CurrentWorkDate);
                        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                        {
                            LedgerController lController = new LedgerController();
                            long voucherNo = lController.SelectVoucherNoByLedgerId(ledgerId);
                            InsertGL2(null, CurrentWorkDate, voucherNo);
                        }
                        Session.Remove("VoucherNo");
                    }
                    else if (ddlRecieptType.SelectedItem.Value.ToString() == "5")
                    {
                        var ledgerId = AdvanceReturn(CurrentWorkDate);
                        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                        {
                            LedgerController lController = new LedgerController();
                            long voucherNo = lController.SelectVoucherNoByLedgerId(ledgerId);
                            InsertGL2(null, CurrentWorkDate, voucherNo);
                        }
                        Session.Remove("VoucherNo");
                    }
                    else
                    {
                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                            }
                        }

                        long ledgerId = 0;
                        if (ddlRecieptType.Value.ToString() == "2")
                        {
                            ledgerId = ChequeRealization2(CurrentWorkDate);
                        }
                        else
                        {
                            ledgerId = ChequeRealization(CurrentWorkDate);// used as All cash, online, cheque
                        }
                        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                        {
                            LedgerController lController = new LedgerController();
                            long voucherNo = lController.SelectVoucherNoByLedgerId(ledgerId);
                            InsertGL(CurrentWorkDate, voucherNo);
                        }
                        txtChequeNo.Text = "";
                        Session.Remove("VoucherNo");
                        SelectCreditInvoice();
                    }
                }
            }

            ClearAll();
            SelectCreditInvoice();
            LoadReceviedCheque();
            LoadPaymentRecieved();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dt"];
        switch (ddSearchType.SelectedIndex)
        {
            case 1:
                dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 2:
                dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            default:
                dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                break;
        }
        GrdCO.DataSource = dt.DefaultView;
        GrdCO.DataBind();
    }

    #endregion

    private void InsertGL(DateTime CurrentWorkDate, long voucherNo)
    {

        DataTable dtVoucher = new DataTable();

        dtVoucher.Columns.Add("ACCOUNT_HEAD_ID", typeof(long));
        dtVoucher.Columns.Add("DEBIT", typeof(decimal));
        dtVoucher.Columns.Add("CREDIT", typeof(decimal));
        dtVoucher.Columns.Add("REMARKS", typeof(string));
        dtVoucher.Columns.Add("Principal_Id", typeof(string));

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int CreditSaleReceivable = Convert.ToInt32(drConfig[0]["VALUE"].ToString());


        DataRow dr = dtVoucher.NewRow();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedItem.Value.ToString());
        dr["REMARKS"] = DrpBankAccount.SelectedItem.Text + " Paid to " + ddlCustomer.SelectedItem.Text;
        dr["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr["CREDIT"] = 0;
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);

        //Debit Side Entry

        DataRow dr1 = dtVoucher.NewRow();
        dr1["ACCOUNT_HEAD_ID"] = CreditSaleReceivable;
        dr1["DEBIT"] = 0;
        dr1["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpBankAccount.SelectedItem.Text + " Paid to " + ddlCustomer.SelectedItem.Text;
        dtVoucher.Rows.Add(dr1);
        string Remakrs = "Customer: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text;

        var slipNo = "CustReceipt";

        if (ddlRecieptType.SelectedItem.Value.ToString() == "2")
        {
            Remakrs = "Franchise: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text;
            slipNo = "FranchiseReceipt";
        }
        string MaxDocumentId = LController.SelectMaxVoucherId(Constants.Bank_Voucher, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate);

        LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), int.Parse(ddlCustomer.Value.ToString()),
            MaxDocumentId, Constants.Bank_Voucher, CurrentWorkDate, Constants.Bank_Deposit,
            Session["VoucherNo"].ToString(), Remakrs, Constants.DateNullValue, txtChequeNo.
            Text, dtVoucher, Convert.ToInt32(Session["UserID"]), slipNo, Constants.DateNullValue,
            true, Constants.ChequePayment, voucherNo);
    }

    private void InsertGL2(string ChequeNo, DateTime CurrentWorkDate, long voucherNo)
    {
        DateTime ChequeDate = Constants.DateNullValue;
        try
        {
            if (DrpAccountType.SelectedItem.Value.ToString() == "18" || DrpAccountType.SelectedItem.Value.ToString() == "33")
            {
                ChequeDate = DateTime.Parse(txtStartDate.Text);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
            return;
        }
        DataTable dtVoucher = new DataTable();
        dtVoucher.Columns.Add("ACCOUNT_HEAD_ID", typeof(long));
        dtVoucher.Columns.Add("DEBIT", typeof(decimal));
        dtVoucher.Columns.Add("CREDIT", typeof(decimal));
        dtVoucher.Columns.Add("REMARKS", typeof(string));
        dtVoucher.Columns.Add("Principal_Id", typeof(string));
        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];
        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int CreditSaleReceivable = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

        DataRow dr = dtVoucher.NewRow();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedItem.Value.ToString());
        dr["REMARKS"] = DrpAccountType.SelectedItem.Text + " received from " + ddlCustomer.SelectedItem.Text;
        if(ddlRecieptType.SelectedItem.Value.ToString()== "5")
        {
            dr["REMARKS"] = DrpAccountType.SelectedItem.Text + " retruned to " + ddlCustomer.SelectedItem.Text;
        }
        
        if(ddlRecieptType.SelectedItem.Value.ToString() == "5")
        {
            dr["DEBIT"] = 0;
            dr["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        }
        else
        {
            dr["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
            dr["CREDIT"] = 0;
        }
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);

        //Debit Side Entry
        DataRow dr1 = dtVoucher.NewRow();
        dr1["ACCOUNT_HEAD_ID"] = CreditSaleReceivable;
        if(ddlRecieptType.SelectedItem.Value.ToString() == "5")
        {
            dr1["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
            dr1["CREDIT"] = 0;
        }
        else
        {
            dr1["DEBIT"] = 0;
            dr1["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        }
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpAccountType.SelectedItem.Text + " received from " + ddlCustomer.SelectedItem.Text;
        if(ddlRecieptType.SelectedItem.Value.ToString() == "5")
        {
            dr1["REMARKS"] = DrpAccountType.SelectedItem.Text + " retruned to " + ddlCustomer.SelectedItem.Text;
        }
        dtVoucher.Rows.Add(dr1);

        string MaxDocumentId = "";

        string Remakrs = "Customer: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text;
        var slipNo = "CustReceipt";
        if (ddlRecieptType.SelectedItem.Value.ToString() == "2")
        {
            Remakrs = "Franchise: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text;
            slipNo = "FranchiseReceipt";
        }
        else if (ddlRecieptType.SelectedItem.Value.ToString() == "5")
        {
            Remakrs = "Customer Return: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text;
            slipNo = "CustomerReturn";
        }

        int VoucherTypeID = Constants.Bank_Voucher;
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            if (ddlRecieptType.SelectedItem.Value.ToString() == "5")
            {
                VoucherTypeID = Constants.CashPayment_Voucher;
            }
            else
            {
                VoucherTypeID = Constants.Cash_Voucher;
            }
        }
        else
        {
            if (ddlRecieptType.SelectedItem.Value.ToString() == "5")
            {
                VoucherTypeID = 17;
            }
        }
        
        //using chequeDate as Transfer Date
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            MaxDocumentId = LController.SelectMaxVoucherId(VoucherTypeID, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate);
            LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), int.Parse(ddlCustomer.SelectedItem.Value.ToString()), MaxDocumentId, VoucherTypeID, CurrentWorkDate, Constants.Cash_Relization, Session["VoucherNo"].ToString(), Remakrs, Constants.DateNullValue, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), slipNo, Constants.DateNullValue, true, Constants.CashPayment, voucherNo);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No : " + MaxDocumentId + " saved successfully');", true);
            PrintVoucher(MaxDocumentId);
        }
        else
        {
            MaxDocumentId = LController.SelectMaxVoucherId(VoucherTypeID, Convert.ToInt32(drpDistributor.SelectedItem.Value), CurrentWorkDate);
            LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), int.Parse(ddlCustomer.SelectedItem.Value.ToString()), MaxDocumentId, VoucherTypeID, CurrentWorkDate, Constants.Bank_Deposit, Session["VoucherNo"].ToString(), Remakrs, ChequeDate, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), slipNo, Constants.DateNullValue, true, Constants.ChequePayment, voucherNo);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No : " + MaxDocumentId + " saved successfully');", true);
            PrintVoucher(MaxDocumentId);
        }
    }

    private void PrintVoucher(string VoucherNo)
    {

        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        CORNBusinessLayer.Reports.crpVoucherViewSupplier CrpReport = new CORNBusinessLayer.Reports.crpVoucherViewSupplier();
        int VoucherType = Constants.IntNullValue;

        string VoucherType2 = "";
        string pVoucherType = "";

        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            if(ddlRecieptType.SelectedItem.Value.ToString() == "5")
            {
                pVoucherType = "Cash Payment";
                VoucherType = 24;
                VoucherType2 = "Cash Payment Voucher";
            }
            else
            {
                pVoucherType = "Cash Receipt";
                VoucherType = 14;
                VoucherType2 = "Cash Receipt Voucher";
            }
        }
        else 
        {
            if(ddlRecieptType.SelectedItem.Value.ToString() == "5")
            {
                pVoucherType = "Bank Payment";
                VoucherType = 17;
                VoucherType2 = "Bank Payment Voucher";
            }
            else
            {
                pVoucherType = "Bank Receipt";
                VoucherType = 15;
                VoucherType2 = "Bank Receipt Voucher";
            }
        }

        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ds = RptAccountCtl.SelectUnpostVoucherForPrint(int.Parse(drpDistributor.SelectedItem.Value.ToString()), VoucherNo, VoucherType, Constants.IntNullValue);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("DISTRIBUTOR_NAME", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
        CrpReport.SetParameterValue("VoucherType", pVoucherType);
        CrpReport.SetParameterValue("VoucherSubType", VoucherType2);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    private void PrintChequeVoucher(string ChequeProcessId)
    {

        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        CORNBusinessLayer.Reports.crpChequeVoucher CrpReport = new CORNBusinessLayer.Reports.crpChequeVoucher();


        string VoucherType2 = "";
        string pVoucherType = "";

        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            pVoucherType = "Bank Voucher";
            VoucherType2 = "Bank Payment Voucher";
        }


        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ds = RptAccountCtl.SelectUnpostVoucherForPrint(int.Parse(drpDistributor.SelectedItem.Value.ToString()), null, Convert.ToInt32(ChequeProcessId), 0);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("DISTRIBUTOR_NAME", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
        CrpReport.SetParameterValue("VoucherType", pVoucherType);
        CrpReport.SetParameterValue("VoucherSubType", VoucherType2);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    private void ClearAll()
    {
        txtAmount.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        txtRemarks.Text = "";
        txtTax.Text = "";
        txtDiscount.Text = "";
        txtChequeNo.Text = "";
        ChkIsDiscount.Checked = false;
    }

    private DataTable GetCOAConfiguration()
    {
        try
        {
            COAMappingController _cController = new COAMappingController();
            DataTable dt = _cController.SelectCOAConfiguration(5, Constants.ShortNullValue, Constants.LongNullValue, "Level 4");
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", string.Format("alert('Error Occured: \n{0}');", ex), true);
            return null;
        }
    }

    private bool GetFinanceConfig()
    {
        try
        {
            DataTable dt = (DataTable)Session["dtAppSettingDetail"];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["IsFinanceIntegrate"]) == 1 ? true : false;
            }
            return false;
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Error in Financial Setting!');", true);
            throw;
        }
    }
    protected void GrdCO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();
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

        int TYPE_ID = Constants.IntNullValue;
        if (ddlRecieptType.SelectedItem.Value.ToString() == "2")
            TYPE_ID = 1;
        CController.DeleteChequeEntry(CurrentWorkDate, long.Parse(GrdCO.Rows[e.RowIndex].Cells[2].Text), DrpAccountType.SelectedItem.Value.ToString(), Convert.ToDecimal(GrdCO.Rows[e.RowIndex].Cells[3].Text), TYPE_ID, int.Parse(GrdCO.Rows[e.RowIndex].Cells[0].Text));
        ddlCustomer_SelectedIndexChanged(null, null);
    }
    protected void ddlRecieptType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtTotalCreditAmount.Text = "";
        btnAddCustomer.Visible = false;
        LoadData();
        SelectCreditInvoice();
        LoadReceviedCheque();
        if (ddlRecieptType.SelectedItem.Value.ToString() == "4")
        {
            btnAddCustomer.Visible = true;
        }
        if (ddlRecieptType.SelectedItem.Value.ToString() == "2")
        {
            Panel1.Visible = false;
        }
        else
        {
            Panel1.Visible = true;
        }
        DrpAccountType.Items.Clear();
        if (ddlRecieptType.SelectedItem.Value.ToString() == "5")
        {
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Cheque Payment", "18"));
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Cash Payment", "21"));
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Online Payment", "33"));
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Credit Card Payment", "34"));
        }
        else
        {
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Cheque Receipt", "18"));
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Cash Receipt", "21"));
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Online Receipt", "33"));
            DrpAccountType.Items.Add(new DevExpress.Web.ListEditItem("Credit Card Receipt", "34"));
        }
    }

    protected void btnSaveCustomer_Click(object sender, EventArgs e)
    {
        mPopUpCustomer.Show();
        if (txtCustomerName.Text.Length > 0 && txtPrimaryContact.Text.Length > 0)
        {
            long CustomerID = CustomerDataController.InsertCustomer2(Convert.ToInt32(drpDistributor.SelectedItem.Value), "", txtCNIC.Text, DateTime.Now.ToShortDateString(), txtPrimaryContact.Text, txtEmail.Text, txtCustomerName.Text, txtAddress.Text, null, 0, "", txtOtherContact.Text, 0, 0, 0, 0, 0);
            if (CustomerID > 0)
            {
                LoadData();
                mPopUpCustomer.Hide();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Primary Contact Number aleady exist!.');", true);                
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Customer Name and Primary Contact are required!');", true);
        }
    }

    protected void btnCancelCustomer_Click(object sender, EventArgs e)
    {
        txtCustomerName.Text = "";
        txtPrimaryContact.Text = "";
        txtAddress.Text = "";
        txtOtherContact.Text = "";
        txtEmail.Text = "";
        txtCustomerName.Text = "";
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