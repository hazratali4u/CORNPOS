using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Globalization;
using System.Web;
using System.IO.Ports;
using CORNDatabaseLayer.Classes;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmOrderPOSNew : System.Web.UI.Page
{
    readonly SkuController SKUCtl = new SkuController();
    readonly DataControl _dc = new DataControl();
    readonly CustomerDataController _cc = new CustomerDataController();

    DataTable _purchaseSkus;

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
            ddlLocation.Focus();
            LoadLocations();
            LoadCustomers();
            CreatTable();
            LoadGridData();
            LoadLookupGrid("");
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            mPopUpLocation.Hide();
        }
    }

    private void CreatTable()
    {
        _purchaseSkus = new DataTable();
        _purchaseSkus.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
        _purchaseSkus.Columns.Add("SALE_INVOICE_ID", typeof(long));
        _purchaseSkus.Columns.Add("CAT_ID", typeof(int));
        _purchaseSkus.Columns.Add("I_D_ID", typeof(int));
        _purchaseSkus.Columns.Add("SKU_ID", typeof(int));
        _purchaseSkus.Columns.Add("SKU_Code", typeof(string));
        _purchaseSkus.Columns.Add("SKU_Name", typeof(string));
        _purchaseSkus.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSkus.Columns.Add("PRICE", typeof(decimal));
        _purchaseSkus.Columns.Add("Quantity", typeof(decimal));
        _purchaseSkus.Columns.Add("SalePercentAmount", typeof(decimal));
        _purchaseSkus.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("DISCOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("DISCOUNT_PERCENT", typeof(decimal));
        _purchaseSkus.Columns.Add("UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("IS_DELETED", typeof(bool));

        Session.Add("PurchaseSKUS", _purchaseSkus);
    }

    #region Load

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

    private void LoadGird()
    {
        GrdPurchase.DataSource = null;
        GrdPurchase.DataBind();
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus != null)
        {
            GrdPurchase.DataSource = _purchaseSkus;
            GrdPurchase.DataBind();
            decimal totalValue = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["Quantity"].ToString()));
            decimal totalAmount = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["AMOUNT"].ToString()));
            decimal totalDiscount = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["DISCOUNT"].ToString()));

            decimal grossAmount = 0;
            foreach (DataRow item in _purchaseSkus.Rows)
            {
                grossAmount = grossAmount + (decimal.Parse(item["Quantity"].ToString()) * 
                    decimal.Parse(item["PRICE"].ToString()));
            }
            var extraDiscount = txtExtraDiscount.Text;

            if (string.IsNullOrEmpty(extraDiscount))
                extraDiscount = "0";

            txtTotalQuantity.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalValue);
            txtTotalAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", grossAmount);
            txtTotalNetAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalAmount - decimal.Parse(extraDiscount));
            txtTotalDiscount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalDiscount);
        }
    }
    private void LoadLocations()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue,
            int.Parse(this.Session["UserId"].ToString()),
            int.Parse(this.Session["CompanyId"].ToString()), 1);

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);

        LoadSkuDetail();
    }

    private void LoadCustomers()
    {
        CustomerDataController mController = new CustomerDataController();
        DataTable dt = mController.UspSelectCustomer(int.Parse(ddlLocation.Value.ToString()),"SaleInvoiceForm", "", DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dt, "CUSTOMER_ID", "CUSTOMER_NAME", true);
        if (dt.Rows.Count > 0)
        {
            ddlCustomer.SelectedIndex = 0;
        }
    }
    private void LoadSkuDetail()
    {
        hfInventoryType.Value = "0";
        var mSkuController = new SkuController();
        if (ddlLocation.SelectedIndex > -1)
        {
            int type = 5;
            DataTable dtItemsOnSaleInvoiceForm = (DataTable)Session["dtAppSettingDetail"];
            if (dtItemsOnSaleInvoiceForm.Rows.Count > 0)
            {
                if(dtItemsOnSaleInvoiceForm.Rows[0]["ItemsType"].ToString() == "2")
                {
                    type = 8;
                }
            }

            DataTable dtSkus = mSkuController.SelectSkusforOrder(int.Parse(HttpContext.Current.Session["CompanyId"].ToString()),Constants.IntNullValue, 1,Constants.IntNullValue,int.Parse(ddlLocation.Value.ToString()),Convert.ToDateTime(HttpContext.Current.Session["CurrentWorkDate"]), type);
            clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtSkus, "SKU_ID", "SKU_NAME", true);
            if (dtSkus.Rows.Count > 0)
            {
                ddlSkus.SelectedIndex = 0;
            }
            Session.Add("Dtsku_Price", dtSkus);
        }
    }
    #endregion

    #region Grid Operations
    protected void Grid_users_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserController _mUController = new UserController();
        mPopUpLocation.Show();
        try
        {
            GridViewRow gvr = Grid_users.Rows[e.NewEditIndex];
            hfMaster_ID.Value = gvr.Cells[1].Text;
            ddlLocation.Value = gvr.Cells[2].Text;
            ddlCustomer.Value = gvr.Cells[3].Text;
            ddlCustomer_SelectedIndexChanged(null, null);
            ddlCustomer.Enabled = false;
            ddlLocation.Enabled = false;
            txtRemarks.Text = gvr.Cells[4].Text.Replace("&nbsp;", "");
            txtExtraDiscount.Text = gvr.Cells[11].Text;
            if (gvr.Cells[10].Text == "2")
            {
                rdoDocType.SelectedIndex = 1;
            }
            else
            {
                rdoDocType.SelectedIndex = 0;
            }
            OrderEntryController oEController = new OrderEntryController();
            var details = oEController.Select_Invoice_Details(Convert.ToInt64(hfMaster_ID.Value));
            _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
            foreach (DataRow item in details.Rows)
            {
                DataRow dr = _purchaseSkus.NewRow();
                dr["SKU_ID"] = item["SKU_ID"].ToString();
                dr["SKU_Name"] = item["SKU_NAME"].ToString();
                dr["UOM_ID"] = item["UOM_ID"].ToString();
                dr["Quantity"] = item["QUANTITY"].ToString();
                dr["UOM_DESC"] = item["UOM_DESC"].ToString();
                dr["SALE_INVOICE_DETAIL_ID"] = item["SALE_INVOICE_DETAIL_ID"].ToString();
                dr["PRICE"] = item["PRICE"].ToString();
                dr["SALE_INVOICE_ID"] = hfMaster_ID.Value;
                dr["CAT_ID"] = 0;
                dr["I_D_ID"] = 0;
                dr["SalePercentAmount"] = 0;
                dr["AMOUNT"] = item["AMOUNT"].ToString();
                dr["DISCOUNT"] = item["DISCOUNT"].ToString();
                dr["DISCOUNT_PERCENT"] = item["DISCOUNT_PERCENT"].ToString();
                dr["IS_DELETED"] = false;
                _purchaseSkus.Rows.Add(dr);
            }
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
            btnSaveDocument.Text = "Update";
        }
        catch (Exception ex)
        {
            btnSave.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
            ex.Message.ToString();
        }
    }
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        mPopUpLocation.Show();
        _rowNo.Value = e.NewEditIndex.ToString();
        ddlSkus.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        ddlSkus_SelectedIndexChanged(null, null);
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        _privouseQty.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtPrice.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[9].Text;

        var discountPercent = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
        if (!string.IsNullOrEmpty(discountPercent) && Convert.ToDecimal(discountPercent) > 0)
        {
            drpDiscType.SelectedIndex = 0;
        }
        else
        {
            drpDiscType.SelectedIndex = 1;
        }

        if (drpDiscType.SelectedIndex == 0)
        {
            decimal discountAmount = decimal.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text);

            txtDiscount.Text = ((discountAmount * 100)
                / (decimal.Parse(txtAmount.Text) + discountAmount)).ToString();

            discVal.Value = discountAmount.ToString();
        }
        else
        {
            discVal.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
            txtDiscount.Text = discVal.Value;
        }

        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        }
        ddlSkus.Enabled = false;
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == ddlLocation.Value.ToString())
            {
                if (dr["MaxDayClose"].ToString().Length > 0)
                {
                    CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                    break;
                }
            }
        }
        DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(ddlLocation.Value),CurrentWorkDate);
        if (dsClosing.Tables[0].Rows.Count > 0)
        {
            decimal closing = Convert.ToDecimal(dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]) + Convert.ToDecimal(_privouseQty.Value);
            lblStock.Text = String.Format("{0:0.00}", closing);
        }
        txtQuantity.Focus();
        btnSave.Text = "Update";
    }
    protected void Grid_users_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var row = Grid_users.Rows[e.RowIndex];
        if (row != null)
        {
            DateTime CurrentWorkDate = Constants.DateNullValue;
            DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
            foreach (DataRow dr in dtLocationInfo.Rows)
            {
                if (dr["DISTRIBUTOR_ID"].ToString() == ddlLocation.Value.ToString())
                {
                    if (dr["MaxDayClose"].ToString().Length > 0)
                    {
                        CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                        break;
                    }
                }
            }

            OrderEntryController oEController = new OrderEntryController();
            var saleInvoiceId = Convert.ToInt64(row.Cells[1].Text);
            oEController.UpdateRollBackDocumentStock(saleInvoiceId,Convert.ToInt32(row.Cells[2].Text),CurrentWorkDate,2,Convert.ToInt32(Session["UserID"]), Constants.ShortNullValue);
            LoadGridData();
            LoadLookupGrid("");
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record deleted successfully.');", true);
        }
    }
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus.Rows.Count > 0)
        {
            _purchaseSkus.Rows.RemoveAt(e.RowIndex);
            LoadGird();
            mPopUpLocation.Show();
        }
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadLookupGrid("");
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        OrderEntryController oEController = new OrderEntryController();

        dt = oEController.Select_Invoice_Lookup(Constants.IntNullValue, Convert.ToInt32(Session["UserID"]), 
            Convert.ToDateTime(Session["CurrentWorkDate"]), 1);

        Session.Add("dtGridData", dt);
    }
    protected void LoadLookupGrid(string pType)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();
        if (ddlLocation.Items.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtGridData"];
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  OR LOGIN_ID LIKE '%" + txtSearch.Text + "%'  OR PASSWORD LIKE '%" + txtSearch.Text + "%'  OR role_name LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  OR LOGIN_ID LIKE '%" + txtSearch.Text + "%'  OR PASSWORD LIKE '%" + txtSearch.Text + "%'  OR role_name LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
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

    protected void Grid_users_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "1")
            {
                //e.Row.Visible = false;
            }
        }
    }
    #endregion

    #region Click OPerations

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        this.LoadLookupGrid("filter");
    }
    protected void btnSave_Document(object sender, EventArgs e)
    {
        try
        {
            mPopUpLocation.Show();
            var gridData = (DataTable)Session["PurchaseSKUS"];
            if (gridData == null || gridData.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Add items in Grid');", true);
                return;
            }
            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            if (Page.IsValid)
            {
                DateTime CurrentWorkDate = Constants.DateNullValue;
                DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
                foreach (DataRow dr in dtLocationInfo.Rows)
                {
                    if (dr["DISTRIBUTOR_ID"].ToString() == ddlLocation.Value.ToString())
                    {
                        if (dr["MaxDayClose"].ToString().Length > 0)
                        {
                            CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                            break;
                        }
                    }
                }
                bool flag = true;
                long savedRecordID = 0;
                OrderEntryController oEController = new OrderEntryController();
                if (btnSaveDocument.Text == "Save")
                {
                    savedRecordID = oEController.InsertSaleInvoice(decimal.Parse(_dc.chkNull_0(txtTotalAmount.Text)),Convert.ToInt32(ddlLocation.Value), CurrentWorkDate, Convert.ToInt64(ddlCustomer.Value.ToString()),Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)), rdoDocType.Value.ToString() == "1" ? 2 : 0,Convert.ToInt32(Session["UserId"]), txtRemarks.Text, decimal.Parse(_dc.chkNull_0(txtTotalDiscount.Text)), decimal.Parse(_dc.chkNull_0(txtExtraDiscount.Text)), gridData, IsFinanceSetting, dtConfig);                    
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);                    
                }
                else if (btnSaveDocument.Text == "Update")
                {
                    spUpdateSALE_INVOICE_MASTER master = new spUpdateSALE_INVOICE_MASTER();
                    master.AMOUNTDUE = decimal.Parse(_dc.chkNull_0(txtTotalAmount.Text));
                    master.CUSTOMER_ID = int.Parse(ddlCustomer.Value.ToString());
                    master.CUSTOMER_TYPE_ID = 3;
                    master.DISTRIBUTOR_ID = int.Parse(ddlLocation.Value.ToString());
                    master.IS_HOLD = false;
                    master.TYPE_ID = 2;
                    master.IS_ACTIVE = true;
                    master.PAIDIN = decimal.Parse(_dc.chkNull_0(txtTotalAmount.Text));
                    master.PAYMENT_MODE_ID = rdoDocType.Value.ToString() == "1" ? 2 : 0;
                    master.REMARKS = txtRemarks.Text;
                    master.SALE_INVOICE_ID = long.Parse(hfMaster_ID.Value);
                    master.USER_ID = int.Parse(Session["UserId"].ToString());
                    master.ITEM_DISCOUNT = decimal.Parse(_dc.chkNull_0(txtTotalDiscount.Text));
                    master.EXTRA_DISCOUNT = decimal.Parse(_dc.chkNull_0(txtExtraDiscount.Text));
                    var saledInvoiceId = oEController.UpdateSALE_INVOICE_MASTER(master);

                    var deleteSaleInvoiceDetail = oEController.DeleteSaleInvoiceDetail(master.SALE_INVOICE_ID);

                    savedRecordID = long.Parse(hfMaster_ID.Value);
                    DataTable newdt = new DataTable();
                    newdt = gridData.Clone();
                    foreach (DataRow dr in gridData.Rows)
                    {
                        var mSaleInvoiceDetail = new spInsertSALE_INVOICE_DETAIL();
                        mSaleInvoiceDetail.SALE_INVOICE_ID = saledInvoiceId;
                        mSaleInvoiceDetail.IS_VOID = false;
                        mSaleInvoiceDetail.PRICE = decimal.Parse(dr["PRICE"].ToString());
                        mSaleInvoiceDetail.QTY = decimal.Parse(dr["Quantity"].ToString());
                        mSaleInvoiceDetail.REMARKS = "H";
                        mSaleInvoiceDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleInvoiceDetail.intSaleMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mSaleInvoiceDetail.DealDetailQTY = decimal.Parse(dr["Quantity"].ToString());
                        mSaleInvoiceDetail.DISTRIBUTOR_ID = master.DISTRIBUTOR_ID;
                        mSaleInvoiceDetail.DISCOUNT = decimal.Parse(dr["DISCOUNT"].ToString());
                        mSaleInvoiceDetail.EXTRA_DISCOUNT = dr["DISCOUNT_PERCENT"].ToString();
                        mSaleInvoiceDetail.IS_FREE = false;
                        mSaleInvoiceDetail.TIME_STAMP2 = System.DateTime.Now;
                        var saleInvoiceDtailId = oEController.InsertSALE_INVOICE_DETAIL(mSaleInvoiceDetail);
                        dr["SALE_INVOICE_DETAIL_ID"] = saleInvoiceDtailId;
                        dr["SALE_INVOICE_ID"] = saledInvoiceId;
                        dr["CAT_ID"] = 0;
                        dr["I_D_ID"] = 0;
                        newdt.ImportRow(dr);
                    }

                    oEController.UpdateStockGLDetail(newdt, CurrentWorkDate, int.Parse(ddlLocation.Value.ToString()),IsFinanceSetting,
                        dtConfig, 3, 1, decimal.Parse(_dc.chkNull_0(txtTotalAmount.Text)), 0,master.ITEM_DISCOUNT + master.EXTRA_DISCOUNT, 
                        rdoDocType.Value.ToString() == "1" ? 2 : 0, 0, 0,0, decimal.Parse(_dc.chkNull_0(txtTotalAmount.Text)), 0,
                        long.Parse(hfMaster_ID.Value), int.Parse(Session["UserId"].ToString()),"", 
                        int.Parse(ddlCustomer.Value.ToString()), 0);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                    flag = true;
                    mPopUpLocation.Hide();
                }
                if (savedRecordID > 0)
                {
                    mPopUpLocation.Show();
                    _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
                    _purchaseSkus.Rows.Clear();
                    Session.Add("PurchaseSKUS", _purchaseSkus);
                    LoadGird();
                    LoadGridData();
                    LoadLookupGrid("");
                    clearMaster();
                    ClearAll();
                    btnSaveDocument.Text = "Save";
                    if (savedRecordID > 0)
                    {
                        ShowReportPopUp(savedRecordID, CurrentWorkDate);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            mPopUpLocation.Show();
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        clearMaster();
        ClearAll();
        btnSaveDocument.Text = "Save";
        CreatTable();
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        _purchaseSkus.Rows.Clear();
        Session.Add("PurchaseSKUS", _purchaseSkus);
        LoadGird();
        mPopUpLocation.Hide();

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        clearMaster();
        ClearAll();
        ddlSkus_SelectedIndexChanged(null, null);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            mPopUpLocation.Show();
            if (ddlCustomer.SelectedIndex == -1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Customer.');", true);
                ddlCustomer.Focus();
                return;
            }

            if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Quantity.');", true);
                txtQuantity.Focus();
                return;
            }

            if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) > decimal.Parse(_dc.chkNull_0(lblStock.Text)))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Quantity can not be greater than avaible stock.');", true);
                txtQuantity.Focus();
                return;
            }

            if (decimal.Parse(_dc.chkNull_0(txtPrice.Text)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter or select Price .');", true);
                txtPrice.Focus();
                return;
            }
            //if (rdoDocType.SelectedIndex == 1 && decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) > decimal.Parse(_dc.chkNull_0(txtQtyToDeliver.Text)))
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Quantity can not be greater than demanded Qty. Demanded Qty: "+ String.Format("{0:0.00}", txtQtyToDeliver.Text)+"');", true);
            //    txtQuantity.Focus();
            //    return;
            //}

            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length > 0)
            {
                _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
                decimal currentStock = CheckStockStatus(int.Parse(_dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
                if (Convert.ToBoolean(Session["VALIDATE_STOCK"]) == false)
                {
                    currentStock = -1;
                }
                decimal Price = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                
                if (btnSave.Text == "Add")
                {
                    if (CheckDublicateSku())
                    {
                        if (currentStock == -1)
                        {
                            DataRow dr = _purchaseSkus.NewRow();
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                            dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            dr["UOM_DESC"] = txtUOM.Text;
                            

                            var totalAmount = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            //var salePercent = decimal.Parse(_dc.chkNull_0(lblSalePer.Text));
                            var salePercent = 0;

                            var saleTaxAmount = totalAmount * (salePercent / 100);
                            dr["SalePercentAmount"] = saleTaxAmount;
                            dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                            dr["AMOUNT"] = (Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text)))
                                + saleTaxAmount - decimal.Parse(discVal.Value);
                            dr["DISCOUNT"] = _dc.chkNull_0(discVal.Value);
                            if (drpDiscType.SelectedIndex == 0)
                            dr["DISCOUNT_PERCENT"] = _dc.chkNull_0(txtDiscount.Text);
                            dr["SALE_INVOICE_DETAIL_ID"] = 0;
                            _purchaseSkus.Rows.Add(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + " Current closing Stock is " + currentStock + "');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + " Already Exists ');", true);
                        return;
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    if (currentStock == -1)
                    {
                        DataRow dr = _purchaseSkus.Rows[Convert.ToInt32(_rowNo.Value)];
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        dr["UOM_DESC"] = txtUOM.Text;

                        var totalAmount = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        //var salePercent = decimal.Parse(_dc.chkNull_0(lblSalePer.Text));
                        //var salePercent = 0;

                        //var saleTaxAmount = totalAmount * (salePercent / 100);
                        dr["SalePercentAmount"] = 0;
                        dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                        dr["AMOUNT"] = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) - decimal.Parse(discVal.Value);
                        dr["DISCOUNT"] = _dc.chkNull_0(discVal.Value);
                        if (drpDiscType.SelectedIndex == 0)
                            dr["DISCOUNT_PERCENT"] = _dc.chkNull_0(txtDiscount.Text);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + "Current closing Stock is " + currentStock.ToString() + "');", true);
                        return;
                    }
                }
                Session.Add("PurchaseSKUS", _purchaseSkus);
                ClearAll();
                LoadGird();
                ScriptManager.GetCurrent(Page).SetFocus(ddlSkus);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Item Select');", true);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CreatTable();
        LoadGird();
        ClearAll();
        clearMaster();
        txtRemarks.Text = "";
    }

    #endregion

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomers();
        LoadSkuDetail();
        ddlSkus_SelectedIndexChanged(null, null);
    }
    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        lblStock.Text = "0";
        txtPrice.Text = "";
        txtQuantity.Enabled = true;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        if (dtskuPrice.Rows.Count > 0)
        {
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length > 0)
            {
                txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
                if (foundRows[0]["IsInventoryWeight"].ToString() != "")
                {
                    if (Convert.ToBoolean(foundRows[0]["IsInventoryWeight"]))
                    {
                        hfInventoryType.Value = "1";
                    }
                    else
                    {
                        txtQuantity.Text = string.Empty;
                        hfInventoryType.Value = "0";
                    }
                }
                else
                {
                    txtQuantity.Text = string.Empty;
                }
                //txtPrice.Text = String.Format("{0:0.00}", foundRows[0]["PRICE"]);
                txtPrice.Text = "0.00";
                if (ddlSkus.Items.Count > 0 && ddlLocation.Items.Count > 0)
                {
                    DateTime CurrentWorkDate = Constants.DateNullValue;
                    DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
                    foreach (DataRow dr in dtLocationInfo.Rows)
                    {
                        if (dr["DISTRIBUTOR_ID"].ToString() == ddlLocation.Value.ToString())
                        {
                            if (dr["MaxDayClose"].ToString().Length > 0)
                            {
                                CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                                break;
                            }
                        }
                    }
                    DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(ddlLocation.Value),CurrentWorkDate);
                    if (dsClosing.Tables[0].Rows.Count > 0)
                    {
                        lblStock.Text = String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
                    }
                }
            }
        }
        txtQuantity.Focus();
    }    
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        LoadSkuDetail();
        //DataTable dtCustomer = (DataTable)Session["dtCustomer"];
        //foreach (DataRow dr in dtCustomer.Rows)
        //{
        //    if (dr["CUSTOMER_ID"].ToString() == ddlCustomer.SelectedItem.Value.ToString())
        //    {
        //        lblSalePer.Text = dr["SALES_PER"].ToString();
        //    }
        //}
    }
    private bool CheckDublicateSku()
    {
        try
        {
            _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

            DataRow[] foundRows = _purchaseSkus.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
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

    private decimal CheckStockStatus(int skuId)
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == ddlLocation.Value.ToString())
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

            var mController = new PhaysicalStockController();
            DataTable dt = mController.SelectSKUClosingStock2(int.Parse(ddlLocation.SelectedItem.Value.ToString()), skuId, "N/A", CurrentWorkDate, 15);
            if (dt.Rows.Count > 0)
            {
                if (decimal.Parse(dt.Rows[0][0].ToString()) + Convert.ToDecimal(_privouseQty.Value) >= decimal.Parse(txtQuantity.Text))
                {
                    return -1;
                }
                else
                {
                    return decimal.Parse(dt.Rows[0][0].ToString()) + Convert.ToDecimal(_privouseQty.Value);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            return 0;
        }
        return 0;
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


    private void ClearAll()
    {

        txtQuantity.Text = "";
        ddlSkus.Enabled = true;
        txtPrice.Text = "";
        txtAmount.Text = "0";
        btnSave.Text = "Add";
        _privouseQty.Value = "0";
        txtDiscount.Text = "0";
        discVal.Value = "0";
    }
    private void clearMaster()
    {
        ddlCustomer.SelectedIndex = 0;
        ddlCustomer_SelectedIndexChanged(null, null);
        txtRemarks.Text = "";
        txtExtraDiscount.Text = "0";
        ddlCustomer.Enabled = true;
        ddlLocation.Enabled = true;
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

    public void ShowReportPopUp(long p_franchise_Master_ID, DateTime CurrentWorkDate)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            RptSaleController saleController = new RptSaleController();
            CrpSaleInvoice CrpReport = new CrpSaleInvoice();
            DataTable dt = mController.SelectReportTitle(int.Parse(ddlLocation.SelectedItem.Value.ToString()));

            DataSet ds = saleController.SelectSaleInvoiceReport(Constants.IntNullValue,
                p_franchise_Master_ID.ToString(), int.Parse(ddlLocation.Value.ToString()), CurrentWorkDate,
                CurrentWorkDate, Constants.IntNullValue, 6, Constants.ByteNullValue, Constants.LongNullValue);

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
            CrpReport.SetParameterValue("ReportName", "SALE INVOICE");
            CrpReport.SetParameterValue("Username", Session["UserName"].ToString());
            CrpReport.SetParameterValue("ContactNumber", dt.Rows[0]["CONTACT_NUMBER"].ToString());
            CrpReport.SetParameterValue("Address", dt.Rows[0]["ADDRESS1"].ToString());
            CrpReport.SetParameterValue("Email", dt.Rows[0]["ADDRESS2"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdoDocType_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();

        if (rdoDocType.SelectedItem.Value.ToString() == "1")
        {
        }
        else
        {
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