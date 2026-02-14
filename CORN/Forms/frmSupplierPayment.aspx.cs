using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
public partial class Forms_frmSupplierPayment : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
    readonly LedgerController LController = new LedgerController();
    readonly ChequeBookController _CBController = new ChequeBookController();
    readonly FinancialYearController _yearController = new FinancialYearController();
    readonly VenderEntryController vendor = new VenderEntryController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Add("LastUsedToken", string.Empty);
            if (Session["FranchiseModule"].ToString() == "1")
            {
                rowType.Visible = true;
            }
            this.GetAppSettingDetail();  
            GetFinancialYear();
            DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Issue", "527"));
            DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Clear", "529"));

            DrpStatus.SelectedIndex = 0;
            LoadDistributor();
            GetAdavnce();
            LoadAccountHead();            
            LoadAccountDetail();
            LoadData();
            SelectCreditInvoice();
            LoadReceviedCheque();

            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            LoadCheque(2);
            DrpStatus.SelectedIndex = 0;
            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            Session.Add("dtConfig", dtConfig);
            Session.Add("IsFinanceSetting", IsFinanceSetting);
        }
    }

    private void toggleControls(string pAccountType)
    {
        if (pAccountType == "21")
        {
            lblChequeNo.Visible = false;
            drpCheque.Visible = false;
            lblChequeDate.Visible = false;
            txtStartDate.Visible = false;
            ibtnStartDate.Visible = false;
            DrpStatus.Visible = false;
            lblStatus.Visible = false;
            GrdCO.Visible = true;
            GrdCheque.Visible = false;
            withHeldCol.Visible = false;
        }
        else if (pAccountType == "33")
        {
            lblChequeNo.Visible = false;
            drpCheque.Visible = false;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            DrpStatus.Visible = false;
            lblStatus.Visible = false;
            lblChequeDate.Text = "Transfer Date";
            GrdCO.Visible = true;
            GrdCheque.Visible = false;
            withHeldCol.Visible = true;
        }
        else
        {

            lblChequeNo.Visible = true;
            drpCheque.Visible = true;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            DrpStatus.Visible = true;
            lblStatus.Visible = true;
            lblChequeDate.Text = "Cheque Date";
            GrdCO.Visible = false;
            GrdCheque.Visible = true;
            withHeldCol.Visible = true;
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
    public void LoadCheque(int TYPE)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = _CBController.SelectCheckBook(Constants.IntNullValue, Convert.ToInt32(DrpBankAccount.SelectedItem.Value.ToString()), null, null, null, true, Constants.DateNullValue, TYPE);
            clsWebFormUtil.FillDxComboBoxList(this.drpCheque, dt, 0, 1, true);

            if (dt.Rows.Count > 0)
            {
                drpCheque.SelectedIndex = 0;
            }
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
            if (drpDistributor.Value.ToString() != Constants.IntNullValue.ToString())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            }
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);
        txtTransactionDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        CalendarExtender1.EndDate = System.DateTime.Now;
    }

    private void LoadData()
    {
        int TypeID = 0;
        if(rblType.SelectedItem.Value == "2")
        {
            TypeID = 9;
        }
        DrpVendor.Items.Clear();
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable dtVendor = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, TypeID, Constants.DateNullValue);
        clsWebFormUtil.FillDxComboBoxList(DrpVendor, dtVendor, 0, 1);
        if (dtVendor.Rows.Count > 0)
        {
            DrpVendor.SelectedIndex = 0;
        }
        ltrlAdvance.Text = " (Advance Amount : 0.00)";
        DataTable dtAdvance = (DataTable)Session["dtAdvance"];
        foreach (DataRow dr in dtAdvance.Rows)
        {
            if (dr["SupplierID"].ToString() == DrpVendor.SelectedItem.Value.ToString())
            {
                ltrlAdvance.Text = " (Advance Amount : s" + Convert.ToDecimal(dr["Advance"]).ToString("N2") + ")";
            }
        }
    }

    private void LoadReceviedCheque()
    {
        DateTime CurrentWorkDate = Convert.ToDateTime(txtTransactionDate.Text);
        ChequeEntryController CController = new ChequeEntryController();
        GrdCheque.DataSource = null;
        GrdCheque.DataBind();
        GrdCO.DataSource = null;
        GrdCO.DataBind();
        decimal chqAmount = 0;
        if (DrpAccountType.SelectedItem.Value.ToString() == "21" || DrpAccountType.SelectedItem.Value.ToString() == "33")
        {
            if (drpDistributor.Items.Count > 0)
            {
                DataTable dt = LController.VendorBankCashTransction(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), CurrentWorkDate, CurrentWorkDate);
                Session.Add("dt", dt);
                GrdCO.DataSource = dt;
                GrdCO.DataBind();
                if (dt != null)
                {
                    foreach (DataRow gvr in dt.Rows)
                    {
                        chqAmount += Convert.ToDecimal(gvr["CHEQUE_AMOUNT"]);
                    }
                    lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                }
            }
        }

        else
        {
            if (DrpStatus.SelectedItem.Value.ToString() != Constants.Cheque_Clear.ToString())
            {
                DataTable dt = null;
                if (DrpVendor.Items.Count > 0)
                {
                    dt = CController.SelectVendorChequeEntry(int.Parse(DrpStatus.SelectedItem.Value.ToString()), CurrentWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpVendor.SelectedItem.Value.ToString()), DrpAccountType.SelectedIndex);
                    Session.Add("dt", dt);
                    GrdCheque.DataSource = dt;
                    GrdCheque.DataBind();
                }
                if (dt != null)
                {
                    foreach (DataRow gvr in dt.Rows)
                    {
                        chqAmount += Convert.ToDecimal(gvr["CHEQUE_AMOUNT"]);
                    }
                }
                lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                foreach (GridViewRow gvr in GrdCredit.Rows)
                {
                    CheckBox row = gvr.FindControl("ChbIsAssigned") as CheckBox;
                    if (row.Enabled)
                    {
                        break;
                    }
                    else
                    {
                        row.Enabled = true;
                    }
                }
            }
            else
            {
                ///Load Cash Realization Detail
                if (Convert.ToInt32(DrpStatus.SelectedItem.Value.ToString()) < 530)
                {
                    DataSet ds = CController.SelectVendorChequeEntry(int.Parse(DrpStatus.SelectedItem.Value.ToString()), CurrentWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), DrpAccountType.SelectedIndex);
                    DataTable dt2 = ds.Tables[1];
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            chqAmount += Convert.ToDecimal(dr[2].ToString());
                        }
                        lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                    }
                }
                ////////////////////////////////////////////////
                DataTable dt = CController.SelectVendorChequeEntry(int.Parse(DrpStatus.SelectedItem.Value.ToString()), CurrentWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DrpAccountType.SelectedIndex);
                Session.Add("dt", dt);
                GrdCheque.DataSource = dt;
                GrdCheque.DataBind();
                foreach (GridViewRow gvr in GrdCheque.Rows)
                {
                    LinkButton row = gvr.FindControl("btnEdit") as LinkButton;
                    row.Enabled = false;
                }
                foreach (GridViewRow gvr in GrdCredit.Rows)
                {
                    CheckBox row = gvr.FindControl("ChbIsAssigned") as CheckBox;
                    row.Checked = false;
                    row.Enabled = false;
                }
            }
            if (DrpStatus.SelectedItem.Value.ToString() != Constants.Cheque_Pending.ToString())
            {
                foreach (GridViewRow gvr in GrdCredit.Rows)
                {
                    CheckBox row = gvr.FindControl("ChbIsAssigned") as CheckBox;
                    row.Checked = false;
                    row.Enabled = false;
                }
            }
        }
    }

    private void checkDuplication()
    {
        DataTable dt = (DataTable)Session["dt"];

        DataRow[] foundRows = dt.Select("VENDOR_ID = '" + DrpVendor.SelectedItem.Value.ToString() + "' and CHEQUE_NO='" + drpCheque.SelectedItem.Text + "'");

        if (foundRows.Length > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Cheque No Already exist against this vendor!');", true);
        }
    }

    private void SelectCreditInvoice()
    {
        LedgerController CDC = new LedgerController();
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        int DocumentTypeID = 2;
        if(rblType.SelectedItem.Value == "2")
        {
            DocumentTypeID = 11;
        }
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        if(drpDistributor.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach(DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                if(li.Value.ToString() != Constants.IntNullValue.ToString())
                {
                    sbLocationIDs.Append(li.Value);
                    sbLocationIDs.Append(",");
                }
            }
        }
        else
        {
            sbLocationIDs.Append(drpDistributor.Value.ToString());
        }
        if (DrpVendor.Items.Count > 0)
        {
            DataTable dtCredit = CDC.GetPendingPurchaseInvoices(sbLocationIDs.ToString(), int.Parse(DrpVendor.SelectedItem.Value.ToString()), 1, DocumentTypeID);
            GrdCredit.DataSource = dtCredit;
            GrdCredit.DataBind();
            CheckBox cb = null;
            decimal TotalAmount = 0;
            foreach (GridViewRow gvr in GrdCredit.Rows)
            {
                cb = gvr.Cells[0].FindControl("ChbIsAssigned") as CheckBox;
                cb.Checked = true;
                TotalAmount += Convert.ToDecimal(gvr.Cells[5].Text);
            }
            txtTotalCreditAmount.Text = TotalAmount.ToString("N2");
        }
    }

    private void GetAdavnce()
    {
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        if (drpDistributor.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                if (li.Value.ToString() != Constants.IntNullValue.ToString())
                {
                    sbLocationIDs.Append(li.Value);
                    sbLocationIDs.Append(",");
                }
            }
        }
        else
        {
            sbLocationIDs.Append(drpDistributor.Value.ToString());
        }
        LedgerController CDC = new LedgerController();
        DataTable dtAdvance = CDC.GetPendingPurchaseInvoices(sbLocationIDs.ToString(), Constants.IntNullValue, 2, Constants.IntNullValue);
        Session.Add("dtAdvance", dtAdvance);
        if (DrpVendor.Items.Count > 0)
        {
            foreach (DataRow dr in dtAdvance.Rows)
            {
                if (dr["SupplierID"].ToString() == DrpVendor.SelectedItem.Value.ToString())
                {
                    ltrlAdvance.Text = " (Advance Amount :" + Convert.ToDecimal(dr["Advance"]).ToString("N2") + ")";
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
            DataTable dt = mAccountController.SelectAccountHeadByMapping(Constants.AC_AccountHeadId, Constants.LongNullValue, TypeID, Constants.AC_CashInHandAccountHead, Convert.ToInt32(drpDistributor.Value));
            clsWebFormUtil.FillDxComboBoxList(DrpBankAccount, dt, 0, 4, true);

            if (dt.Rows.Count > 0)
            {
                DrpBankAccount.SelectedIndex = 0;
            }
        }
        else
        {
            DataTable dt = mAccountController.SelectAccountHeadByMapping(Constants.AC_AccountHeadId, Constants.LongNullValue, TypeID, Constants.AC_BankAccountHead,Convert.ToInt32(drpDistributor.Value));
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

    private void CashAdvance(DateTime CurrentWorkDate)
    {
        int DocumentTypeID = 2;
        if(rblType.SelectedItem.Value == "2")
        {
            DocumentTypeID = 11;
        }
        string remarks = "Advance Cash " + txtRemarks.Text;
        DateTime chqDate = Constants.DateNullValue;

        int VoucherType = Constants.CashPayment_Voucher;

        if (DrpAccountType.SelectedItem.Value.ToString() == "33")
        {
            chqDate = Convert.ToDateTime(txtStartDate.Text);
            VoucherType = Constants.Expanse_Voucher;
            remarks = "Advance Online Transfer " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;            
        }
        string MaxDocumentID = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 1);

        Session.Add("VoucherNo", MaxDocumentID);

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        int advanceType = 11;
        if (DrpAccountType.SelectedItem.Value.ToString()== "21")
        { advanceType = 12; }//cash advance 
        if (DrpAccountType.SelectedItem.Value.ToString() == "33")
        { advanceType = 13; } // advance online transfer
        long docNumber = vendor.InsertSupplierAdvance(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpVendor.SelectedItem.Value.ToString()),CurrentWorkDate, advanceType,decimal.Parse(dc.chkNull_0(txtAmount.Text)), int.Parse(Session["UserId"].ToString()));
        long LedgerID = LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentID), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, decimal.Parse(dc.chkNull_0(txtAmount.Text)),CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID,null, int.Parse(Session["UserId"].ToString()), docNumber, "0", int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", chqDate,true);
        if (LedgerID > 0)
        {
            LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentID), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), decimal.Parse(dc.chkNull_0(txtAmount.Text)), 0,CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID,null, int.Parse(Session["UserId"].ToString()),docNumber, "0", int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", chqDate,true);
        }

        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
        {
            InsertGL2(null, CurrentWorkDate,Convert.ToInt64(MaxDocumentID));
        }
    }

    private void ChequeRealization()
    {
        System.Text.StringBuilder sbVoucherNos = new System.Text.StringBuilder();
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        string PayeeName = System.DateTime.Now.ToString("HHmmss");
        decimal GLAmount = 0;
        DateTime CurrentWorkDate = Convert.ToDateTime(txtTransactionDate.Text);
        DataTable dtLocationIDs = new DataTable();
        dtLocationIDs.Columns.Add("LocationID", typeof(int));
        DataRow drLocation;
        if (cbAdvancePayment.Checked)
        {
            drLocation = dtLocationIDs.NewRow();
            drLocation["LocationID"] = drpDistributor.Value;
            dtLocationIDs.Rows.Add(drLocation);
        }
        else
        {
            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    drLocation = dtLocationIDs.NewRow();
                    drLocation["LocationID"] = dr.Cells[8].Text;
                    dtLocationIDs.Rows.Add(drLocation);
                }
            }
        }
        dtLocationIDs = dtLocationIDs.DefaultView.ToTable(true, "LocationID");

        int DocumentTypeID = 2;
        if (rblType.SelectedItem.Value == "2")
        {
            DocumentTypeID = 11;
        }

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
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
            txtWithheldSalesTax.Text = "0";

        LedgerController LController = new LedgerController();
        string MaxDocumentId = "";
        int VoucherType = Constants.Expanse_Voucher;

        decimal OfferAmount = decimal.Parse(txtAmount.Text) + (!string.IsNullOrEmpty(txtWithheldSalesTax.Text) ? Convert.ToDecimal(txtWithheldSalesTax.Text) : 0);
        decimal OfferAmount2 = OfferAmount;
        decimal realizeAmount = 0;
        string remarks = "";
        foreach (DataRow drLoc in dtLocationIDs.Rows)
        {
            GLAmount = 0;
            if (OfferAmount2 != realizeAmount)
            {
                if (DrpAccountType.SelectedItem.Value.ToString() == "18")//Cheque
                {
                    MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drLoc["LocationID"].ToString()), 1);
                    remarks = "Chq# " + drpCheque.SelectedItem.Text + ", " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
                }
                else if (DrpAccountType.SelectedItem.Value.ToString() == "33")//Online
                {
                    MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drLoc["LocationID"].ToString()), 1);
                    remarks = "Online Transfer " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;
                }
                else if (DrpAccountType.SelectedItem.Value.ToString() == "21")//Cash
                {
                    VoucherType = Constants.Cash_Voucher;
                    MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drLoc["LocationID"].ToString()), 1);
                    remarks = "Cash " + txtRemarks.Text;
                }
                Session.Add("VoucherNo", MaxDocumentId);

                DataRow[] drConfig = null;
                DataTable dtConfig = (DataTable)Session["dtConfig"];
                drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
                int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
                string ChequeNo = null;
                if (DrpAccountType.SelectedIndex == 0)
                {
                    ChequeNo = drpCheque.SelectedItem.Text;
                }
                string manualNo = "";
                decimal withHeldAmount = !string.IsNullOrEmpty(txtWithheldSalesTax.Text) ? Convert.ToDecimal(txtWithheldSalesTax.Text) : 0;
                if (!cbAdvancePayment.Checked)
                {
                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true && drLoc["LocationID"].ToString() == dr.Cells[8].Text)
                        {

                            manualNo = dr.Cells[6].Text;
                            if (Convert.ToString(dr.Cells[6].Text) == "opng")
                            {
                                manualNo = "opng";
                            }
                            if (decimal.Parse(dr.Cells[5].Text) >= OfferAmount)
                            {
                                realizeAmount += OfferAmount;
                                long LedgerID = LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drLoc["LocationID"].ToString()), 0, (OfferAmount - withHeldAmount), CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID, ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", ChequeDate, false);
                                if (LedgerID > 0)
                                {
                                    LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drLoc["LocationID"].ToString()), (OfferAmount - withHeldAmount), 0, CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID, ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", ChequeDate, false);
                                    GLAmount += (OfferAmount - withHeldAmount);
                                    OfferAmount = decimal.Parse(dr.Cells[5].Text) - OfferAmount;
                                    LController.UpdatePurchaseMaster(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), int.Parse(drLoc["LocationID"].ToString()), OfferAmount);
                                }
                                
                                break;
                            }
                            else if (decimal.Parse(dr.Cells[5].Text) <= OfferAmount)
                            {
                                realizeAmount += decimal.Parse(dr.Cells[5].Text);
                                long LedgerID = LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drLoc["LocationID"].ToString()), 0, decimal.Parse(dr.Cells[5].Text), CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID, ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", ChequeDate, false);
                                if (LedgerID > 0)
                                {
                                    LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drLoc["LocationID"].ToString()), decimal.Parse(dr.Cells[5].Text), 0, CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID, ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", ChequeDate, false);
                                    GLAmount += decimal.Parse(dr.Cells[5].Text);
                                    OfferAmount = OfferAmount - decimal.Parse(dr.Cells[5].Text);
                                    LController.UpdatePurchaseMaster(long.Parse(dr.Cells[1].Text), int.Parse(drLoc["LocationID"].ToString()), 0);
                                }                                
                            }
                        }
                    }
                }
                if (drpDistributor.Value.ToString() != Constants.IntNullValue.ToString())
                {
                    //Advance Entry
                    if (realizeAmount < decimal.Parse(txtAmount.Text))
                    {
                        int advanceType = 11;
                        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
                        { advanceType = 12; }//cash advance 
                        if (DrpAccountType.SelectedItem.Value.ToString() == "33")
                        { advanceType = 13; } // advance online transfer
                        long docNumber = vendor.InsertSupplierAdvance(int.Parse(drLoc["LocationID"].ToString()), int.Parse(DrpVendor.SelectedItem.Value.ToString()), CurrentWorkDate, advanceType, decimal.Parse(dc.chkNull_0(txtAmount.Text)), int.Parse(Session["UserId"].ToString()));
                        long LedgerID = LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drLoc["LocationID"].ToString()), 0, OfferAmount, CurrentWorkDate, remarks + " (Advance)", DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID, ChequeNo, int.Parse(Session["UserId"].ToString()), docNumber, "0", int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", ChequeDate, true);
                        if (LedgerID > 0)
                        {
                            LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drLoc["LocationID"].ToString()), OfferAmount, 0, CurrentWorkDate, remarks + " (Advance)", DateTime.Now, int.Parse(DrpVendor.SelectedItem.Value.ToString()), DocumentTypeID, ChequeNo, int.Parse(Session["UserId"].ToString()), docNumber, "0", int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "", "", ChequeDate, true);
                        }
                        GLAmount += OfferAmount;
                    }
                }
            }
            else
            {
                break;
            }
            long GLInvoiceNo = 0;
            if(MaxDocumentId.Length > 0)
            {
                GLInvoiceNo = Convert.ToInt64(MaxDocumentId);
            }
            if (Convert.ToBoolean(Session["IsFinanceSetting"]))
            {
                string ChequeNo = null;
                if (DrpAccountType.SelectedIndex == 0)
                {
                    ChequeNo = drpCheque.SelectedItem.Text;
                }
                string VoucherNo = InsertGL(ChequeNo, CurrentWorkDate,Convert.ToInt32(drLoc["LocationID"]),GLAmount, PayeeName,GLInvoiceNo);
                sbVoucherNos.Append(VoucherNo);
                sbVoucherNos.Append(",");

                sbLocationIDs.Append(drLoc["LocationID"].ToString());
                sbLocationIDs.Append(",");
            }
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No(s) : " + sbVoucherNos.ToString() + " saved successfully');", true);
        PrintVoucher2(sbVoucherNos.ToString(), sbLocationIDs.ToString(),PayeeName);
    }

    protected void GrdCheque_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            hfChequeNo.Value = "";
            ChequeEntryController Ccontroller = new ChequeEntryController();
            HFChqueProcessId.Value = GrdCheque.Rows[e.NewEditIndex].Cells[0].Text;
            ddlAccountHead.Value = GrdCheque.Rows[e.NewEditIndex].Cells[13].Text;
            DrpVendor.Value = GrdCheque.Rows[e.NewEditIndex].Cells[1].Text;
            DrpBankAccount.Value = GrdCheque.Rows[e.NewEditIndex].Cells[9].Text;
            LoadCheque(5);
            drpCheque.SelectedIndex = drpCheque.Items.IndexOf(drpCheque.Items.FindByText(GrdCheque.Rows[e.NewEditIndex].Cells[3].Text.ToString()));
            txtStartDate.Text = Convert.ToDateTime(GrdCheque.Rows[e.NewEditIndex].Cells[4].Text).ToString("dd-MMM-yyyy");
            txtReceivedDate.Text = GrdCheque.Rows[e.NewEditIndex].Cells[5].Text;
            txtAmount.Text = GrdCheque.Rows[e.NewEditIndex].Cells[6].Text;
            txtWithheldSalesTax.Text = GrdCheque.Rows[e.NewEditIndex].Cells[14].Text;
            txtRemarks.Text = GrdCheque.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
            ChkIsDiscount.Checked = Convert.ToBoolean(GrdCheque.Rows[e.NewEditIndex].Cells[11].Text);
            txtDiscount.Text = GrdCheque.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
            txtTax.Text = GrdCheque.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
            cbAdvancePayment.Checked = Convert.ToBoolean(GrdCheque.Rows[e.NewEditIndex].Cells[15].Text.Replace("&nbsp;", ""));
            cbAdvancePayment.Enabled = false;
            btnSave.Text = "Update";
            SelectCreditInvoice();
            if (DrpAccountType.SelectedItem.Value.ToString() == "18" && !cbAdvancePayment.Checked)
            {
                DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);
                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    chRelized.Checked = false;
                    foreach (DataRow dbr in dt.Rows)
                    {
                        if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                        {
                            chRelized.Checked = true;
                        }
                    }
                }
            }
            hfChequeNo.Value = GrdCheque.Rows[e.NewEditIndex].Cells[3].Text.ToString();
            DrpStatus.Items.Clear();
            DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Issue", "527"));
            DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Clear", "529"));
            DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Bounce", "530"));
            DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Cancel", "560"));
        }
        catch (Exception)
        {
            cbAdvancePayment.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Invoice not found for selected cheque');", true);
        }
    }

    #region Sel/Index Change

    protected void DrpVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectCreditInvoice();
        LoadReceviedCheque();
        ltrlAdvance.Text = " (Advance Amount : 0.00)";
        DataTable dtAdvance = (DataTable)Session["dtAdvance"];
        foreach(DataRow dr in dtAdvance.Rows)
        {
            if (dr["SupplierID"].ToString() == DrpVendor.SelectedItem.Value.ToString())
            {
                ltrlAdvance.Text = " (Advance Amount :" + Convert.ToDecimal(dr["Advance"]).ToString("N2") + ")";
            }
        }
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        if (drpDistributor.Value.ToString() != Constants.IntNullValue.ToString())
        {
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
        SelectCreditInvoice();
        LoadReceviedCheque();
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            LoadAccountHead();
            LoadAccountDetail();
        }
    }

    protected void DrpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save" && DrpStatus.SelectedItem.Value.ToString() != Constants.Cheque_Clear.ToString())
        {           
            LoadReceviedCheque();
        }
    }
    protected void DrpBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                LoadReceviedCheque();
            }
            LoadCheque(2);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", string.Format("alert('{0}')", ex.Message), true);
        }
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
        string currentToken = hfToken.Value;
        string lastUsedToken = Session["LastUsedToken"] as string;
        if(currentToken == lastUsedToken)
        {
            ClearAll();
            return;
        }
        try
        {
            if (drpDistributor.Value.ToString() == Constants.IntNullValue.ToString() && cbAdvancePayment.Checked)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Advance can no be entered when All Location selected!.');", true);
                return;
            }

            if (txtTransactionDate.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
                return;
            }
            DateTime CurrentWorkDate = Convert.ToDateTime(txtTransactionDate.Text);
            ChequeEntryController CController = new ChequeEntryController();
            DateTime ChequeDate = Convert.ToDateTime(txtStartDate.Text);

            int InvoiceCount = Constants.IntNullValue;
            if (!cbAdvancePayment.Checked)
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
                    if (!cbAdvancePayment.Checked)
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
                    }
                    if (InvoiceCount == Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                    {
                        if (drpDistributor.Value.ToString() == Constants.IntNullValue.ToString() && cbAdvancePayment.Checked)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Advance can no be entered when All Location selected!.');", true);
                            return;
                        }
                        else
                        {
                            CashAdvance(CurrentWorkDate);                            
                            Session.Remove("VoucherNo");
                        }
                    }
                    else if (InvoiceCount != Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                    {
                        ChequeRealization();// used as All cash, online, cheque                        
                        Session.Remove("VoucherNo");
                        SelectCreditInvoice();
                    }
                }
                else if (DrpAccountType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
                {
                    checkDuplication();
                    short chkDiscount = 0;
                    if (ChkIsDiscount.Checked)
                    {
                        chkDiscount = 1;
                    }
                    HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedItem.Value.ToString()),int.Parse(DrpVendor.SelectedItem.Value.ToString()), 0, drpCheque.SelectedItem.Text,txtBankName.Text, ChequeDate, CurrentWorkDate, Constants.DateNullValue,Constants.DateNullValue, Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)),int.Parse(DrpStatus.SelectedItem.Value.ToString()), DateTime.Now, DrpAccountType.SelectedIndex,"", txtRemarks.Text, long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), 1, Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)), chkDiscount,Convert.ToDecimal(dc.chkNull_0(txtTax.Text)),Convert.ToInt32(ddlAccountHead.SelectedItem.Value.ToString()), Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text)),cbAdvancePayment.Checked);
                    if (cbAdvancePayment.Checked)
                    {
                        long docNumber = vendor.InsertSupplierAdvance(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpVendor.SelectedItem.Value.ToString()), CurrentWorkDate, 11, decimal.Parse(dc.chkNull_0(txtAmount.Text)), int.Parse(Session["UserId"].ToString()));
                        CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), docNumber);
                    }
                    else
                    {
                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]));
                            }
                        }
                    }
                    if (DrpStatus.Value.ToString() == "529")
                    {
                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue,Constants.IntNullValue, Constants.LongNullValue, drpCheque.SelectedItem.Text, null,Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, CurrentWorkDate,Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedItem.Value.ToString()),Constants.DateNullValue, "", DrpAccountType.SelectedIndex,txtRemarks.Text, long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), Constants.DecimalNullValue, Constants.ShortNullValue,Constants.DecimalNullValue, Constants.IntNullValue,Convert.ToString(hfChequeNo.Value), Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text)));
                        CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);
                        if (cbAdvancePayment.Checked)
                        {
                            long docNumber = vendor.InsertSupplierAdvance(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpVendor.SelectedItem.Value.ToString()), CurrentWorkDate, 11, decimal.Parse(dc.chkNull_0(txtAmount.Text)), int.Parse(Session["UserId"].ToString()));
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), docNumber);
                        }
                        else
                        {
                            foreach (GridViewRow dr in GrdCredit.Rows)
                            {
                                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                                if (chRelized.Checked == true)
                                {
                                    CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]));
                                }
                            }
                        }
                        ChequeRealization();
                        Session.Remove("VoucherNo");
                        SelectCreditInvoice();
                    }
                    else
                    {
                        PrintChequeVoucher(HFChqueProcessId.Value);
                    }
                }
            }
            else
            {
                DataTable dt = _CBController.SelectCheckBook(Convert.ToInt32(drpCheque.SelectedItem.Value.ToString()), Constants.IntNullValue, null, null, null, true, Constants.DateNullValue, 4);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"].ToString()) == 0 && DrpStatus.SelectedIndex != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Selected cheque is not issued yet');", true);
                        return;
                    }
                }
                if (int.Parse(DrpStatus.SelectedItem.Value.ToString()) == Constants.Cheque_Clear)
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, drpCheque.SelectedItem.Text, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, CurrentWorkDate,Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedItem.Value.ToString()), Constants.DateNullValue, "", DrpAccountType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), Constants.DecimalNullValue, Constants.ShortNullValue, Constants.DecimalNullValue,Constants.IntNullValue, Convert.ToString(hfChequeNo.Value), Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text)));
                    CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);
                    if (cbAdvancePayment.Checked)
                    {
                        long docNumber = vendor.InsertSupplierAdvance(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpVendor.SelectedItem.Value.ToString()), CurrentWorkDate, 11, decimal.Parse(dc.chkNull_0(txtAmount.Text)), int.Parse(Session["UserId"].ToString()));
                        CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), docNumber);
                    }
                    else
                    {
                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]));
                            }
                        }
                    }
                    ChequeRealization();                    
                    Session.Remove("VoucherNo");
                    SelectCreditInvoice();
                }
            }
            GetAdavnce();
            LoadCheque(2);
            ClearAll();
            LoadReceviedCheque();
            LoadPaymentRecieved();
            Session["LastUsedToken"] = currentToken;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
        LoadCheque(2);
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dt"];
        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            switch (ddSearchType.SelectedIndex)
            {
                case 1:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 2:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 3:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 4:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                default:
                    dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                    break;
            }
            GrdCheque.DataSource = dt.DefaultView;
            GrdCheque.DataBind();
        }
        else
        {
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
    }

    #endregion

    private string InsertGL(string ChequeNo, DateTime CurrentWorkDate,int DistributoID,decimal Amount,string PayeeName,long InvoiceNo)
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
            return null;
        }

        DataTable dtVoucher = new DataTable();

        dtVoucher.Columns.Add("ACCOUNT_HEAD_ID", typeof(long));
        dtVoucher.Columns.Add("DEBIT", typeof(decimal));
        dtVoucher.Columns.Add("CREDIT", typeof(decimal));
        dtVoucher.Columns.Add("REMARKS", typeof(string));
        dtVoucher.Columns.Add("Principal_Id", typeof(string));

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

        DataRow dr = dtVoucher.NewRow();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedItem.Value.ToString());
        dr["REMARKS"] = DrpAccountType.SelectedItem.Text + " Paid to " + DrpVendor.SelectedItem.Text;
        dr["DEBIT"] = 0;
        dr["CREDIT"] = Amount - Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text));
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);

        if (Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text)) > 0)
        {
            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.WithHoldingTax + "'");
            int WithHoldingTax = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

            //With Holding Tax Entry
            DataRow dr2 = dtVoucher.NewRow();
            dr2["ACCOUNT_HEAD_ID"] = WithHoldingTax;
            dr2["DEBIT"] = 0;
            dr2["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text));
            dr2["Principal_Id"] = 0;
            dr2["REMARKS"] = " With Holding Tax Paid to " + DrpVendor.SelectedItem.Text;
            dtVoucher.Rows.Add(dr2);
        }

        //Debit Side Entry
        DataRow dr1 = dtVoucher.NewRow();
        dr1["ACCOUNT_HEAD_ID"] = PayableAccount;
        dr1["DEBIT"] = Amount;
        dr1["CREDIT"] = 0;
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpAccountType.SelectedItem.Text + " Paid to " + DrpVendor.SelectedItem.Text;
        dtVoucher.Rows.Add(dr1);
        
        string MaxDocumentId = "";

        //using chequeDate as Transfer Date
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            MaxDocumentId = LController.SelectMaxVoucherId(Constants.CashPayment_Voucher, DistributoID, CurrentWorkDate);
            LController.Add_Voucher(DistributoID, int.Parse(DrpVendor.SelectedItem.Value.ToString()), MaxDocumentId, Constants.CashPayment_Voucher, CurrentWorkDate, Constants.Cash_Relization, PayeeName, "Suppler: " + DrpVendor.SelectedItem.Text + ", " + txtRemarks.Text, Constants.DateNullValue, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.CashPayment, InvoiceNo);            
        }
        else
        {
            MaxDocumentId = LController.SelectMaxVoucherId(Constants.Expanse_Voucher, DistributoID, CurrentWorkDate);
            LController.Add_Voucher(DistributoID, int.Parse(DrpVendor.SelectedItem.Value.ToString()), MaxDocumentId, Constants.Expanse_Voucher, CurrentWorkDate, Constants.Bank_Deposit, PayeeName, "Suppler: " + DrpVendor.SelectedItem.Text + ", " + txtRemarks.Text, ChequeDate, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.ChequePayment, InvoiceNo);            
        }
        return MaxDocumentId;
    }
    private void InsertGL2(string ChequeNo, DateTime CurrentWorkDate,long InvoiceNo)
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

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

        DataRow dr = dtVoucher.NewRow();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedItem.Value.ToString());
        dr["REMARKS"] = DrpAccountType.SelectedItem.Text + " Paid to " + DrpVendor.SelectedItem.Text;
        dr["DEBIT"] = 0;
        dr["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)) - Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text));
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);

        if (Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text)) > 0)
        {
            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.WithHoldingTax + "'");
            int WithHoldingTax = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

            //With Holding Tax Entry
            DataRow dr2 = dtVoucher.NewRow();
            dr2["ACCOUNT_HEAD_ID"] = WithHoldingTax;
            dr2["DEBIT"] = 0;
            dr2["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtWithheldSalesTax.Text));
            dr2["Principal_Id"] = 0;
            dr2["REMARKS"] = " With Holding Tax Paid to " + DrpVendor.SelectedItem.Text;
            dtVoucher.Rows.Add(dr2);
        }

        //Debit Side Entry
        DataRow dr1 = dtVoucher.NewRow();
        dr1["ACCOUNT_HEAD_ID"] = PayableAccount;
        dr1["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr1["CREDIT"] = 0;
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpAccountType.SelectedItem.Text + " Paid to " + DrpVendor.SelectedItem.Text;
        dtVoucher.Rows.Add(dr1);

        string MaxDocumentId = "";

        //using chequeDate as Transfer Date
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            MaxDocumentId = LController.SelectMaxVoucherId(Constants.CashPayment_Voucher, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate);
            LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()),int.Parse(DrpVendor.SelectedItem.Value.ToString()), MaxDocumentId, Constants.CashPayment_Voucher,CurrentWorkDate, Constants.Cash_Relization, Session["VoucherNo"].ToString(),"Suppler: " + DrpVendor.SelectedItem.Text + ", " + txtRemarks.Text,Constants.DateNullValue, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.CashPayment, InvoiceNo);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No : " + MaxDocumentId + " saved successfully');", true);
            PrintVoucher(MaxDocumentId, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()));
        }
        else
        {
            MaxDocumentId = LController.SelectMaxVoucherId(Constants.Expanse_Voucher, Convert.ToInt32(drpDistributor.SelectedItem.Value), CurrentWorkDate);
            LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()),int.Parse(DrpVendor.SelectedItem.Value.ToString()), MaxDocumentId, Constants.Expanse_Voucher,CurrentWorkDate, Constants.Bank_Deposit, Session["VoucherNo"].ToString(),"Suppler: " + DrpVendor.SelectedItem.Text + ", " + txtRemarks.Text, ChequeDate, ChequeNo,dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.ChequePayment, InvoiceNo);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No : " + MaxDocumentId + " saved successfully');", true);
            PrintVoucher(MaxDocumentId, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()));
        }
    }
    private void PrintVoucher(string VoucherNo, int DistributorID)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        CORNBusinessLayer.Reports.crpVoucherViewSupplier CrpReport = new CORNBusinessLayer.Reports.crpVoucherViewSupplier();
        int VoucherType = Constants.IntNullValue;
        string VoucherType2 = "";
        string pVoucherType = "";
        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            pVoucherType = "Bank Voucher";
            VoucherType = 17;
            VoucherType2 = "Bank Payment Voucher";
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            pVoucherType = "Cash Voucher";
            VoucherType = 24;
            VoucherType2 = "Cash Payment Voucher";
        }
        else
        {
            pVoucherType = "Bank Voucher";
            VoucherType = 17;
            VoucherType2 = "Bank Payment Voucher";
        }

        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(DistributorID);
        ds = RptAccountCtl.SelectUnpostVoucherForPrint(DistributorID, VoucherNo, VoucherType, Constants.IntNullValue);
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
    private void PrintVoucher2(string VoucherNo,string DistributorIDs,string PayeeName)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        CORNBusinessLayer.Reports.crpVoucherViewSupplier2 CrpReport = new CORNBusinessLayer.Reports.crpVoucherViewSupplier2();
        int VoucherType = Constants.IntNullValue;
        string VoucherType2 = "";
        string pVoucherType = "";
        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            pVoucherType = "Bank Voucher";
            VoucherType = 17;
            VoucherType2 = "Bank Payment Voucher";
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            pVoucherType = "Cash Voucher";
            VoucherType = 24;
            VoucherType2 = "Cash Payment Voucher";
        }
        else
        {
            pVoucherType = "Bank Voucher";
            VoucherType = 17;
            VoucherType2 = "Bank Payment Voucher";
        }

        DataSet ds = null;
        ds = RptAccountCtl.GetUnpostVoucherForPrint(DistributorIDs, VoucherNo, VoucherType, Constants.IntNullValue, PayeeName);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
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
        txtWithheldSalesTax.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        txtRemarks.Text = "";
        txtTax.Text = "";
        txtDiscount.Text = "";
        ChkIsDiscount.Checked = false;
        cbAdvancePayment.Enabled = true;
        DrpStatus.SelectedIndex = 0;
        DrpStatus.Items.Clear();
        DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Issue", "527"));
        DrpStatus.Items.Add(new DevExpress.Web.ListEditItem("Clear", "529"));
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
        int VendroID = Convert.ToInt32(GrdCO.Rows[e.RowIndex].Cells[0].Text);
        int VoucherNo = Convert.ToInt32(GrdCO.Rows[e.RowIndex].Cells[10].Text);
        int LocationID = Convert.ToInt32(GrdCO.Rows[e.RowIndex].Cells[11].Text);
        DateTime DocumentDate = Convert.ToDateTime(GrdCO.Rows[e.RowIndex].Cells[5].Text);
        if(LController.DeleteSupplierPayment(LocationID,VendroID,VoucherNo,Convert.ToInt32(DrpAccountType.Value),DocumentDate))
        {
            SelectCreditInvoice();
            LoadReceviedCheque();
        }
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
        SelectCreditInvoice();
    }

    protected void cbAdvancePayment_CheckedChanged(object sender, EventArgs e)
    {
        GrdCredit.Enabled = !cbAdvancePayment.Checked;
        LoadReceviedCheque();
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