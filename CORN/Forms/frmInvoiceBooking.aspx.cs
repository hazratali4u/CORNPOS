using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web;
using CORNBusinessLayer.Reports;

public partial class Forms_frmInvoiceBooking : System.Web.UI.Page
{
    readonly RoleManagementController mController = new RoleManagementController();
    readonly PurchaseController _mPurchaseCtrl = new PurchaseController();
    readonly LedgerController CDC = new LedgerController();
    readonly DataControl _dc = new DataControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            Session.Remove("dtGridData");
            this.GetAppSettingDetail();
            LoadDISTRIBUTOR();
            LoadAccountHead();            
            LoadSupplier();
            LoadGridData();
            LoadGrid("");
            contentBox.Visible = false;
            lookupBox.Visible = true;
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            DoEmptyTextBox();
            txtDiscount.Attributes.Add("autocomplete", "off");
            txtGross.Attributes.Add("autocomplete", "off");
            txtGst.Attributes.Add("autocomplete", "off");
            txtAdvanceTax.Attributes.Add("autocomplete", "off");
            txtInvDate.Attributes.Add("autocomplete", "off");
            txtNetAmount.Attributes.Add("autocomplete", "off");
            txtNetAmount.Attributes.Add("readonly", "readonly");
            txtInvDate.Attributes.Add("readonly", "readonly");
            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtInvDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            CreatTable();
            LoadDetailGrid();
        }
    }
    private void CreatTable()
    {
        DataTable _purchaseSkus = new DataTable();
        _purchaseSkus.Columns.Add("ACCOUNT_HEAD", typeof(string));
        _purchaseSkus.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("ACCOUNT_HEAD_ID", typeof(long));

        Session.Add("PurchaseSKUS", _purchaseSkus);

    }
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            if (decimal.Parse(_dc.chkNull_0(txtDetailNetAmount.Text)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Net Amount');", true);
                txtDetailNetAmount.Focus();
                return;
            }

            DataTable _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

            if (btnAdd_Item.Text == "Add")
            {
                if (CheckDublicateSku())
                {
                    DataRow dr = _purchaseSkus.NewRow();
                    dr["ACCOUNT_HEAD_ID"] = long.Parse(ddlAccountHead.Value.ToString());
                    dr["ACCOUNT_HEAD"] = ddlAccountHead.SelectedItem.Text;
                    dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtDetailNetAmount.Text));
                    _purchaseSkus.Rows.Add(dr);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlAccountHead.SelectedItem.Text + " Already Exists ');", true);
                    return;
                }
            }
            else if (btnAdd_Item.Text == "Update")
            {
                DataRow dr = _purchaseSkus.Rows[Convert.ToInt32(_rowNo2.Value)];
                dr["ACCOUNT_HEAD_ID"] = long.Parse(ddlAccountHead.Value.ToString());
                dr["ACCOUNT_HEAD"] = ddlAccountHead.SelectedItem.Text;
                dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtDetailNetAmount.Text));
            }
            Session.Add("PurchaseSKUS", _purchaseSkus);
            txtDetailNetAmount.Text = "";
            LoadDetailGrid();
            btnAdd_Item.Text = "Add";
            ddlAccountHead.Enabled = true;
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }
    }
    public void GetDebitDetailForUpdate(string p_ContractType, long p_purchase_Master_ID)
    {
        CreatTable();

        var purchaseController = new PurchaseController();
        var purchaseDetail = purchaseController.SelectInvoiceBookingDetail(p_ContractType, p_purchase_Master_ID,1);

        DataTable _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        foreach (DataRow item in purchaseDetail.Rows)
        {
            DataRow dr = _purchaseSkus.NewRow();
            dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(item["ACCOUNT_HEAD_ID"].ToString());
            dr["ACCOUNT_HEAD"] = item["ACCOUNT_HEAD"].ToString();
            dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(item["AMOUNT"].ToString()));

            _purchaseSkus.Rows.Add(dr);
        }

        LoadDetailGrid();
    }
    private void LoadDetailGrid()
    {
        DataTable _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        if (_purchaseSkus != null)
        {
            grdCOA.DataSource = _purchaseSkus;
            grdCOA.DataBind();
        }
    }
    private bool CheckDublicateSku()
    {
        try
        {
            DataTable _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
            DataRow[] foundRows = _purchaseSkus.Select("ACCOUNT_HEAD_ID  = '" + ddlAccountHead.SelectedItem.Value + "'");

            if (foundRows.Length == 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadAccountHead()
    {
        int TypeID = 1;
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            TypeID = 5;
        }

        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dtHead = mAccountController.SelectAccountHeadLocation(Constants.AC_AccountHeadId, Convert.ToInt32(ddDistributorId.Value), TypeID);
        clsWebFormUtil.FillDxComboBoxList(this.ddlAccountHead, dtHead, "ACCOUNT_HEAD_ID", "ACCOUNT_DETAIL", true);

        if (dtHead.Rows.Count > 0)
        {
            ddlAccountHead.SelectedIndex = 0;
        }
    }

    public void DoEmptyTextBox()
    {
        txtDiscount.Attributes.Add("value", "");
        txtGross.Attributes.Add("value", "");
        txtGst.Attributes.Add("value", "");
        txtAdvanceTax.Attributes.Add("value", "");
        txtInvDate.Attributes.Add("value", "");
        txtNetAmount.Attributes.Add("value", "");
    }

    private void LoadDISTRIBUTOR()
    {
        DistributorController mController = new DistributorController();
        DataTable dtDistributor = mController.SelectDistributorInfo(Constants.IntNullValue,
            int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddDistributorId, dtDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");
        if (dtDistributor.Rows.Count > 0)
        {
            ddDistributorId.SelectedIndex = 0;
        }
    }


    private void LoadSupplier()
    {
        DrpSupplier.Items.Clear();
        if(Session["FranchiseModule"].ToString() == "1")
        {
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            clsWebFormUtil.FillDxComboBoxList(DrpSupplier, dtVendors, "VendorID", "VendorName");
        }
        else
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillDxComboBoxList(this.DrpSupplier, m_dt, 0, 1);
        }
        if (DrpSupplier.Items.Count > 0)
        {
            DrpSupplier.SelectedIndex = 0;
        }
        else
        {
            DrpSupplier.SelectedIndex = -1;
        }
    }
    private void LoadGridData()
    {
        int TypeID = 24;
        if (Session["FranchiseModule"].ToString() == "1")
        {
            TypeID = 27;
        }
        DataTable dt = new DataTable();
        dt = _mPurchaseCtrl.SelectPurchaseDocumentNo(TypeID, Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
        Session.Add("dtGridData", dt);
    }
    protected void LoadGrid(string pType)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();

        if (ddDistributorId.Items.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtGridData"];
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR SUPPLIER LIKE '%" + txtSearch.Text + "%'  OR ORDER_NUMBER LIKE '%" + txtSearch.Text + "%'";
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR SUPPLIER LIKE '%" + txtSearch.Text + "%'  OR ORDER_NUMBER LIKE '%" + txtSearch.Text + "%'";
                }
                else
                {
                    dt.DefaultView.RowFilter = null;
                }
                if (dt.Rows.Count > 0)
                {
                    Grid_users.PageIndex = 0;
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
        }
    }

    protected void ddDistributorId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            LoadAccountHead();
        }
        drpContractType_SelectedIndexChanged(null, null);
    }

    protected void Grid_users_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow gvr = Grid_users.Rows[e.NewEditIndex];
            hfDocumentID.Value = gvr.Cells[1].Text;
            try
            {
                ddDistributorId.Value = gvr.Cells[2].Text;
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Relavent location is inactive');", true);
                return;
            }

            if (gvr.Cells[17].Text == "Franchise")
            {
                drpContractType.SelectedIndex = 1;
                drpContractType_SelectedIndexChanged(null,null);
            }
            else
            {
                drpContractType.SelectedIndex = 0;
                drpContractType_SelectedIndexChanged(null, null);
            }

            try
            {
                if (Session["FranchiseModule"].ToString() == "1")
                {
                    DataTable dtVendors = (DataTable)Session["dtVendors"];
                    foreach (DataRow dr in dtVendors.Rows)
                    {
                        if (dr["SupplierLocationID"].ToString() == gvr.Cells[8].Text && dr["VendorType"].ToString() == "2")
                        {
                            DrpSupplier.Value = dr["VendorID"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    DrpSupplier.Value = gvr.Cells[8].Text;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Relevant Supplier/Franchise is inactive');", true);
                return;
            }

            txtInvNo.Text = gvr.Cells[5].Text;
            try
            {
                txtInvDate.Text = Convert.ToDateTime(gvr.Cells[6].Text).ToString("dd-MMM-yyyy");
            }
            catch (Exception)
            {
            }
            txtGross.Text = gvr.Cells[9].Text;
            txtDiscount.Text = gvr.Cells[12].Text;
            txtGst.Text = gvr.Cells[11].Text;
            txtNetAmount.Text = gvr.Cells[13].Text;
            txtRemarks.Text = gvr.Cells[14].Text.Replace("&nbsp;", "");
            txtAdvanceTax.Text = gvr.Cells[18].Text;
            DrpSupplier.Enabled = false;
            ddDistributorId.Enabled = false;
            drpContractType.Enabled = false;

            btnSave.Text = "Update";
            contentBox.Visible = true;
            lookupBox.Visible = false;
            searchBox.Visible = false;
            searchBtn.Visible = false;
            btnActive.Visible = false;
            btnCancel.Visible = true;
            btnSave.Visible = true;
            btnAdd.Visible = false;

            GetDebitDetailForUpdate(gvr.Cells[17].Text, long.Parse(hfDocumentID.Value));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + ex.Message.ToString() + "')", true);
        }
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
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                DataControl _dc = new DataControl();
                lblErrorMsg.Text = "";
                lblErrorMsg.Visible = false;
                DataTable dtConfig = GetCOAConfiguration();
                bool IsFinanceSetting = GetFinanceConfig();

                if (grdCOA.Rows.Count > 0)
                {
                    decimal t_amount = 0;
                    string s;
                    foreach (GridViewRow dr2 in grdCOA.Rows)
                    {

                        s = dr2.Cells[1].Text;
                        t_amount += decimal.Parse(s);


                    }
                    if (t_amount != decimal.Parse(txtNetAmount.Text))
                    {


                        lblErrorMsg.Text = "Amount Not Matching";
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Amount Not Matching');", true);
                        return;

                    }

                    if (drpContractType.SelectedIndex == 0)
                    {
                        int VendorID = int.Parse(DrpSupplier.SelectedItem.Value.ToString());
                        int DocumentTypeID = 2;
                        if (Session["FranchiseModule"].ToString() == "1")
                        {
                            DataTable dtVendors = (DataTable)Session["dtVendors"];
                            foreach (DataRow dr in dtVendors.Rows)
                            {
                                if (dr["VendorID"].ToString() == DrpSupplier.SelectedItem.Value.ToString())
                                {
                                    VendorID = Convert.ToInt32(dr["SupplierLocationID"]);
                                    if (dr["VendorType"].ToString() == "2")
                                    {
                                        DocumentTypeID = 11;
                                    }
                                    break;
                                }
                            }
                        }


                        if (btnSave.Text == "Save")
                        {
                            long PurchaseID = _mPurchaseCtrl.InsertInvoiceBooking(
                                Convert.ToInt32(ddDistributorId.SelectedItem.Value),
                                txtInvNo.Text, DocumentTypeID, Convert.ToDateTime(txtInvDate.Text),
                                Convert.ToInt32(ddDistributorId.SelectedItem.Value),
                                VendorID, Convert.ToDecimal(txtGross.Text), false, 0, "InvoiceBooking",
                                Convert.ToInt32(Session["UserId"].ToString()),
                                VendorID, Convert.ToDecimal(_dc.chkNull_0(txtGst.Text)), Convert.ToDecimal(_dc.chkNull_0(txtAdvanceTax.Text)),
                                Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text)),
                                Convert.ToDecimal(txtNetAmount.Text), txtRemarks.Text,
                                DrpSupplier.SelectedItem.Text, dtConfig, IsFinanceSetting,
                                grdCOA);
                            ShowSupplierInvoiceBookingPopUp("Supplier", PurchaseID);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                        }
                        else if (btnSave.Text == "Update")
                        {
                            _mPurchaseCtrl.UpdateInvoiceBooking(long.Parse(hfDocumentID.Value), 
                                int.Parse(ddDistributorId.SelectedItem.Value.ToString()), txtInvNo.Text,
                                DocumentTypeID, Convert.ToDateTime(txtInvDate.Text), 
                                int.Parse(ddDistributorId.SelectedItem.Value.ToString()), 
                                VendorID, Convert.ToDecimal(txtGross.Text), false, 0, "InvoiceBooking", 
                                int.Parse(Session["UserId"].ToString()), VendorID, 
                                Convert.ToDecimal(_dc.chkNull_0(txtGst.Text)), Convert.ToDecimal(_dc.chkNull_0(txtAdvanceTax.Text)),
                                decimal.Parse(_dc.chkNull_0(txtDiscount.Text)), 
                                Convert.ToDecimal(txtNetAmount.Text), txtRemarks.Text,
                                DrpSupplier.SelectedItem.Text, dtConfig, IsFinanceSetting,
                                grdCOA);
                            ShowSupplierInvoiceBookingPopUp("Supplier", Convert.ToInt64(hfDocumentID.Value));
                        }
                    }
                    else
                    {
                        if (btnSave.Text == "Save")
                        {
                            long PurchaseID = _mPurchaseCtrl.InsertOrUpdateFranchiseInvoiceBooking(0, 
                                Convert.ToInt32(ddDistributorId.SelectedItem.Value), txtInvNo.Text,
                                Convert.ToDateTime(txtInvDate.Text), 
                                Convert.ToInt32(DrpSupplier.SelectedItem.Value), 
                                Convert.ToDecimal(txtGross.Text), 0,
                                Convert.ToInt32(Session["UserId"].ToString()),
                                Convert.ToInt32(DrpSupplier.SelectedItem.Value),
                                Convert.ToDecimal(_dc.chkNull_0(txtGst.Text)), Convert.ToDecimal(_dc.chkNull_0(txtAdvanceTax.Text)),
                                Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text)),
                                Convert.ToDecimal(txtNetAmount.Text), txtRemarks.Text,
                                DrpSupplier.SelectedItem.Text, dtConfig, IsFinanceSetting,
                                grdCOA);
                            RoyaltyAdvance(Convert.ToDateTime(txtInvDate.Text), PurchaseID, dtConfig);
                            ShowSupplierInvoiceBookingPopUp("Franchise", PurchaseID);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                        }
                        else if (btnSave.Text == "Update")
                        {
                            long PurchaseID = _mPurchaseCtrl.InsertOrUpdateFranchiseInvoiceBooking(
                                long.Parse(hfDocumentID.Value), 
                                Convert.ToInt32(ddDistributorId.SelectedItem.Value), txtInvNo.Text,
                                Convert.ToDateTime(txtInvDate.Text), 
                                Convert.ToInt32(DrpSupplier.SelectedItem.Value),
                                Convert.ToDecimal(txtGross.Text), 0, 
                                Convert.ToInt32(Session["UserId"].ToString()),
                                Convert.ToInt32(DrpSupplier.SelectedItem.Value),
                                Convert.ToDecimal(_dc.chkNull_0(txtGst.Text)), Convert.ToDecimal(_dc.chkNull_0(txtAdvanceTax.Text)),
                                Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text)), 
                                Convert.ToDecimal(txtNetAmount.Text), txtRemarks.Text,
                                DrpSupplier.SelectedItem.Text, dtConfig, IsFinanceSetting,
                                grdCOA);

                            LedgerController LController = new LedgerController();
                            LController.DeleteCustomerLedger(long.Parse(hfDocumentID.Value), 24, int.Parse(ddDistributorId.Value.ToString()), long.Parse(DrpSupplier.Value.ToString()), 10, "RoyaltyBooking", 13);
                            RoyaltyAdvance(Convert.ToDateTime(txtInvDate.Text), long.Parse(hfDocumentID.Value), dtConfig);
                            ShowSupplierInvoiceBookingPopUp("Franchise", PurchaseID);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        }
                    }
                    LoadGridData();
                    LoadGrid("");
                    ClearControls();
                    CreatTable();
                    LoadDetailGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Please add Details in Grid');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            }
        }
    }
    private long RoyaltyAdvance(DateTime CurrentWorkDate, long savedId, DataTable dtConfig)
    {
        string remarks = "Royalty Cash " + txtRemarks.Text;
        int DocumentTypeID = Constants.Document_FranchiseSale;
        DateTime chqDate = Constants.DateNullValue;
        int VoucherType = Constants.CreditSale;
        LedgerController LController = new LedgerController();
        string MaxDocumentID = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(ddDistributorId.SelectedItem.Value.ToString()), 0);
        //Session.Add("VoucherNo", MaxDocumentID);
        DataRow[] drConfig = null;
        long ledgerId = 0;

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        if (PayableAccount == 0)
        {
            dtConfig = GetCOAConfiguration();
            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
            PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        }
        string SlipNo = "RoyaltyBooking";

        DataControl _dc = new DataControl();

        ledgerId = LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), PayableAccount,
            int.Parse(ddDistributorId.SelectedItem.Value.ToString()), decimal.Parse(_dc.chkNull_0(txtNetAmount.Text)),
            0, CurrentWorkDate, remarks,
            DateTime.Now, int.Parse(DrpSupplier.SelectedItem.Value.ToString()), 
            Constants.Document_Purchase, null, int.Parse(Session["UserId"].ToString()), savedId,
            savedId.ToString(), DocumentTypeID, SlipNo, chqDate, 13, "");

        LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID),
            long.Parse(ddlAccountHead.SelectedItem.Value.ToString()),
            int.Parse(ddDistributorId.SelectedItem.Value.ToString()), 0,
            decimal.Parse(_dc.chkNull_0(txtNetAmount.Text)), CurrentWorkDate, remarks, DateTime.Now, int.Parse(DrpSupplier.SelectedItem.Value.ToString()), 
            Constants.Document_Purchase, null, int.Parse(Session["UserId"].ToString()), savedId, savedId.ToString(), DocumentTypeID,
            SlipNo, chqDate, 13, "");

        return ledgerId;
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        this.LoadGrid("filter");
    }

    protected void btnActive_Click(object sender, EventArgs e)
    {
        UserController UController = new UserController();
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in Grid_users.Rows)
            {
                var chRelized2 = (CheckBox)dr2.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized2.Checked)
                {
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select record first');", true);
                return;
            }
            bool flag = false;
            foreach (GridViewRow dr in Grid_users.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked)
                {
                    if (dr.Cells[17].Text == "Franchise")
                    {
                        if (Convert.ToString(dr.Cells[15].Text) == "Active")
                        {
                            UController.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), 0, 21);
                            flag = true;
                        }
                        else
                        {
                            UController.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), 1, 21);
                            flag = true;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dr.Cells[15].Text) == "Active")
                        {
                            UController.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), 0, 20);
                            flag = true;
                        }
                        else
                        {
                            UController.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), 1, 20);
                            flag = true;
                        }
                    }
                }
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
            }
            LoadGridData();
            this.LoadGrid("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        contentBox.Visible = true;
        lookupBox.Visible = false;
        searchBox.Visible = false;
        searchBtn.Visible = false;
        btnActive.Visible = false;
        btnCancel.Visible = true;
        btnSave.Visible = true;
        btnAdd.Visible = false;
        //DoEmptyTextBox();
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ddDistributorId.SelectedIndex = 0;
        LoadSupplier();
        ClearControls();
        CreatTable();
        LoadDetailGrid();
        contentBox.Visible = false;
        lookupBox.Visible = true;
        searchBox.Visible = true;
        searchBtn.Visible = true;
        btnActive.Visible = true;
        btnCancel.Visible = false;
        btnSave.Visible = false;
        btnAdd.Visible = true;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddDistributorId.SelectedIndex = 0;
        LoadSupplier();
        ClearControls();
    }

    protected void ClearControls()
    {
        try
        {
            txtInvNo.Text = "";
            txtDiscount.Text = "";
            txtGross.Text = "";
            txtGst.Text = "";
            txtAdvanceTax.Text = "";
            txtNetAmount.Text = "";
            txtRemarks.Text = "";
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = "";

            DrpSupplier.Enabled = true;
            ddDistributorId.Enabled = true;
            drpContractType.Enabled = true;
            btnSave.Text = "Save";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    private void LoadBranchFranchise()
    {
        DrpSupplier.Items.Clear();
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 4);
        clsWebFormUtil.FillDxComboBoxList(DrpSupplier, dt, "CUSTOMER_ID", "CUSTOMER_NAME", true);

        if (dt.Rows.Count > 0)
        {
            DrpSupplier.SelectedIndex = 0;
        }
    }
    protected void drpContractType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpContractType.SelectedIndex == 0)
        {
            LoadSupplier();
            loadType.InnerText = "Supplier";
        }
        else if (drpContractType.SelectedIndex == 1)
        {
            LoadBranchFranchise();
            loadType.InnerText = "Franchise";
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

    protected void grdCOA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtMaster = (DataTable)this.Session["PurchaseSKUS"];
            if (dtMaster.Rows.Count > 0)
            {
                dtMaster.Rows.RemoveAt(e.RowIndex);
                Session.Add("PurchaseSKUS", dtMaster);
                LoadDetailGrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdCOA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _rowNo2.Value = e.NewEditIndex.ToString();
        ddlAccountHead.Value = grdCOA.Rows[e.NewEditIndex].Cells[2].Text;
        txtDetailNetAmount.Text = grdCOA.Rows[e.NewEditIndex].Cells[1].Text;
        btnAdd_Item.Text = "Update";
        ddlAccountHead.Enabled = false;
    }

    public void ShowSupplierInvoiceBookingPopUp(string p_ContractType,long PurchaseMasterID)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataTable dt = mController.SelectReportTitle(int.Parse(ddDistributorId.SelectedItem.Value.ToString()));
        CrpInvoiceBooking CrpReport = new CrpInvoiceBooking();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectInvoiceBookingDetail(p_ContractType, PurchaseMasterID, 2);

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        //CrpReport.SetParameterValue("DocumentType", "Transfer Out Document");
        //CrpReport.SetParameterValue("Principal", "");
        //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        //CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}