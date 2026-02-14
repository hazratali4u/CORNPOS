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

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmFranchiseSaleInvoice : System.Web.UI.Page
{
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
    readonly PurchaseController _mPurchaseCtrl = new PurchaseController();
    readonly SkuController SKUCtl = new SkuController();
    readonly DataControl _dc = new DataControl();
    readonly CustomerDataController _cc = new CustomerDataController();
    readonly FranchiseSaleInvoiceController _franchiseController = new FranchiseSaleInvoiceController();

    DataTable _purchaseSkus;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            GetAppSettingDetail();
            Session.Remove("dtGridData");
            ddlLocation.Focus();
            LoadLocations();
            CreatTable();
            LoadGridData();
            LoadLookupGrid("");
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            mPopUpLocation.Hide();
            LoadPriceLevel();
            GetDocumentNo();
        }
    }

    private void CreatTable()
    {
        _purchaseSkus = new DataTable();
        _purchaseSkus.Columns.Add("FRANCHISE_DETAIL_ID", typeof(long));
        _purchaseSkus.Columns.Add("SKU_ID", typeof(int));
        _purchaseSkus.Columns.Add("SKU_Code", typeof(string));
        _purchaseSkus.Columns.Add("SKU_Name", typeof(string));
        _purchaseSkus.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSkus.Columns.Add("PRICE", typeof(decimal));
        _purchaseSkus.Columns.Add("PRICE_LEVEL", typeof(decimal));
        _purchaseSkus.Columns.Add("Quantity", typeof(decimal));
        _purchaseSkus.Columns.Add("SalePercentAmount", typeof(decimal));
        _purchaseSkus.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("S_UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("S_Quantity", typeof(decimal));
        _purchaseSkus.Columns.Add("IS_DELETED", typeof(bool));
        _purchaseSkus.Columns.Add("PRICE_LEVEL_AMOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("PRICE_LEVEL_FACTOR", typeof(decimal));
        _purchaseSkus.Columns.Add("SKU_PRICES_LEVEL_ID", typeof(long));

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
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus != null)
        {
            GrdPurchase.DataSource = _purchaseSkus;
            GrdPurchase.DataBind();
            decimal totalValue = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["Quantity"].ToString()));
            decimal totalAmount = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["AMOUNT"].ToString()));
            txtTotalQuantity.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalValue);
            txtTotalAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalAmount);
        }
        if(_purchaseSkus.Rows.Count == 0)
        {
            ddlCustomer.Enabled = true;
        }
    }
    private void LoadLocations()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 3);
        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);

        LoadCustomers();
        LoadSkuDetail();
    }

    private void LoadCustomers()
    {
        ddlCustomer.Items.Clear();
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 4);
        clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlCustomer.SelectedIndex = 0;
            lblSalePer.Text = dt.Rows[0]["SALES_PER"].ToString();
        }
        Session.Add("dtCustomer", dt);
    }
    private void LoadSkuDetail()
    {
        hfInventoryType.Value = "0";
        SKUPriceDetailController SKUCtl = new SKUPriceDetailController();
        if (ddlLocation.SelectedIndex > -1)
        {
            DataTable dtskuPrice = new DataTable();
            if (Session["IsLocationWiseItem"].ToString() == "1")
            {
                dtskuPrice = _pController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(ddlLocation.Value), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 8, Convert.ToDateTime(Session["CurrentWorkDate"]));
            }
            else
            {
                dtskuPrice = SKUCtl.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(ddlLocation.Value), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 12, Convert.ToDateTime(Session["CurrentWorkDate"]));
            }
            clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_NAME", true);
            if (dtskuPrice.Rows.Count > 0)
            {
                txtUOM.Text = dtskuPrice.Rows[0]["UOM_DESC"].ToString();
                ddlSkus.SelectedIndex = 0;
                if (dtskuPrice.Rows[0]["IsInventoryWeight"].ToString() != "")
                {
                    if (Convert.ToBoolean(dtskuPrice.Rows[0]["IsInventoryWeight"]))
                    {
                        hfInventoryType.Value = "1";
                    }
                }
            }
            Session.Add("Dtsku_Price", dtskuPrice);
            Session.Add("dtskuPrice2", dtskuPrice);
        }
    }
    private void LoadPriceLevel()
    {
        SKUPriceLevelController SKUPrice = new SKUPriceLevelController();
        DataTable dtPrice = new DataTable();
        ddlPriceLevel.DataSource = dtPrice;
        ddlPriceLevel.DataBind();
        ddlPriceLevel.Items.Add(new DevExpress.Web.ListEditItem("Select Price Level", Constants.LongNullValue.ToString()));
        if (ddlSkus.SelectedIndex != -1)
        {
            dtPrice = SKUPrice.GetItemPriceLevel(int.Parse(ddlSkus.SelectedItem.Value.ToString()), 2, Convert.ToInt32(ddlCustomer.Value));
        }
        clsWebFormUtil.FillDxComboBoxList(ddlPriceLevel, dtPrice, "SKU_PRICES_LEVEL_ID", "PriceLevelName");

        ddlPriceLevel.SelectedIndex = 0;
        lblPrice.Text = "Avg. Price";
        txtPrice.Attributes.Remove("readonly");

        if (dtPrice.Rows.Count > 0)
        {
            ddlPriceLevel.SelectedIndex = 1;

            var dt = LoadPriceLevelDetail(long.Parse(ddlPriceLevel.SelectedItem.Value.ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow[] isPriceWise = dt.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "' AND IsPercentWise = 'False'");

                if (isPriceWise != null && isPriceWise.Length > 0)
                {
                    txtPrice.Text = isPriceWise[0]["PRICE"].ToString();
                    lblPrice.Text = "Price";
                    txtPrice.Attributes.Add("readonly", "true");
                }
            }
        }
    }
    private DataTable LoadPriceLevelDetail(long PriceLevelID)
    {
        SKUPriceLevelController SKUPrice = new SKUPriceLevelController();
        DataTable dtPrice = SKUPrice.GetItemPriceLevel(int.Parse(ddlSkus.SelectedItem.Value.ToString()), 4, PriceLevelID);
        return dtPrice;
    }
    private void LoadLastPurchasePrice()
    {
        SKUPriceDetailController SKUPrice = new SKUPriceDetailController();
        lblLastPrice.Text = "Last Price: " + String.Format("{0:0.00}", 0);
        if (ddlSkus.SelectedIndex != -1)
        {
            DataTable dt = SKUPrice.GetLastPurchasePrice(
                int.Parse(ddlSkus.SelectedItem.Value.ToString()), int.Parse(ddlLocation.SelectedItem.Value.ToString()));
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["PurchasePrice"].ToString()) &&
                    decimal.Parse(dt.Rows[0]["PurchasePrice"].ToString()) > 0)
                {
                    lblLastPrice.Text = "Last Price (Purchase): " + String.Format("{0:0.00}", dt.Rows[0]["PurchasePrice"]);
                }
                else if (!string.IsNullOrEmpty(dt.Rows[0]["ProductionInPrice"].ToString()) &&
                    decimal.Parse(dt.Rows[0]["ProductionInPrice"].ToString()) > 0)
                {
                    lblLastPrice.Text = "Last Price (Production In): " + String.Format("{0:0.00}", dt.Rows[0]["ProductionInPrice"]);
                }
                else if (!string.IsNullOrEmpty(dt.Rows[0]["SplitItemPrice"].ToString()) &&
                    decimal.Parse(dt.Rows[0]["SplitItemPrice"].ToString()) > 0)
                {
                    lblLastPrice.Text = "Last Price (Split Item): " + String.Format("{0:0.00}", dt.Rows[0]["SplitItemPrice"]);
                }
                else if (!string.IsNullOrEmpty(dt.Rows[0]["TransferInPrice"].ToString()) &&
                    decimal.Parse(dt.Rows[0]["TransferInPrice"].ToString()) > 0)
                {
                    lblLastPrice.Text = "Last Price (Transfer In): " + String.Format("{0:0.00}", dt.Rows[0]["TransferInPrice"]);
                }
            }
         }
    }
    protected void ddlPriceLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        lblPrice.Text = "Avg. Price";
        txtPrice.Attributes.Remove("readonly");

        var dt = LoadPriceLevelDetail(long.Parse(ddlPriceLevel.SelectedItem.Value.ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow[] isPriceWise = dt.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "' AND IsPercentWise = 'False'");

            if (isPriceWise != null && isPriceWise.Length > 0)
            {
                txtPrice.Text = isPriceWise[0]["PRICE"].ToString();
                lblPrice.Text = "Price";
                txtPrice.Attributes.Add("readonly", "true");
            }
        }
        else
        {
            lblPrice.Text = "Avg. Price";
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
            LoadCustomers();
            ddlCustomer.Value = gvr.Cells[3].Text;
            ddlCustomer_SelectedIndexChanged(null, null);
            txtRefNo.Text = gvr.Cells[7].Text.Replace("&nbsp;", "");
            txtRemarks.Text = gvr.Cells[4].Text.Replace("&nbsp;", "");

            if (rdoDocType.SelectedItem.Value.ToString() =="1")
            {
                drpDocumentNo.Value = Server.HtmlDecode(gvr.Cells[10].Text);
                drpDocumentNo_SelectedIndexChanged(null, null);
            }

            txtRemarks.Text = gvr.Cells[4].Text.Replace("&nbsp;", "");
            var details = _franchiseController.SelectFranchise_Invoice_Details(Convert.ToInt64(hfMaster_ID.Value));
            _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
            foreach (DataRow item in details.Rows)
            {
                DataRow dr = _purchaseSkus.NewRow();
                dr["SKU_ID"] = item["SKU_ID"].ToString();
                dr["SKU_Name"] = item["SKU_NAME"].ToString();
                dr["UOM_ID"] = item["UOM_ID"].ToString();
                dr["Quantity"] = item["QUANTITY"].ToString();
                dr["UOM_DESC"] = item["UOM_DESC"].ToString();
                dr["FRANCHISE_DETAIL_ID"] = item["FRANCHISE_DETAIL_ID"].ToString();
                dr["PRICE"] = item["PRICE"].ToString();
                dr["PRICE_LEVEL"] = item["PRICE_LEVEL"].ToString();
                dr["PRICE_LEVEL_FACTOR"] = item["PRICE_LEVEL_FACTOR"].ToString();
                dr["SKU_PRICES_LEVEL_ID"] = !string.IsNullOrEmpty(item["SKU_PRICES_LEVEL_ID"].ToString()) ?
                    item["SKU_PRICES_LEVEL_ID"].ToString() : Constants.LongNullValue.ToString();

                dr["PRICE_LEVEL_AMOUNT"] = Convert.ToDecimal(item["QUANTITY"].ToString())
                    * (Convert.ToDecimal(item["PRICE"]) *
                    (Convert.ToDecimal(item["PRICE_LEVEL_FACTOR"]) / 100));

                dr["SalePercentAmount"] = item["SalePercentAmount"].ToString();
                dr["AMOUNT"] = item["AMOUNT"].ToString();
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
        txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
        ddlPriceLevel.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[13].Text;
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

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus.Rows.Count > 0)
        {
            _purchaseSkus.Rows.RemoveAt(e.RowIndex);
            DataRow dr = _purchaseSkus.NewRow();
            LoadGird();
            mPopUpLocation.Show();
        }
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = _franchiseController.SelectFranchise_Invoice_Lookup(Constants.IntNullValue, Convert.ToInt32(Session["UserID"]), Convert.ToDateTime(Session["CurrentWorkDate"]), 1);
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

            if (rdoDocType.SelectedIndex == 1 && drpDocumentNo.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Demand No');", true);
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
                var stockDemandId = Constants.IntNullValue;
                if (rdoDocType.SelectedIndex == 1)
                    stockDemandId = int.Parse(drpDocumentNo.Value.ToString());

                if (btnSaveDocument.Text == "Save")
                {
                    savedRecordID = FranchiseSaleInvoiceController.Add_FranchiseSaleInvoice(Convert.ToInt32(ddlLocation.SelectedItem.Value), 
                        Convert.ToInt64(ddlCustomer.SelectedItem.Value), txtRefNo.Text, txtRemarks.Text,
                        Convert.ToDecimal(txtTotalAmount.Text), Convert.ToInt32(Session["UserID"]),
                        CurrentWorkDate, false, false, false, Convert.ToDecimal(lblSalePer.Text),
                        (DataTable)Session["PurchaseSKUS"], IsFinanceSetting, dtConfig, stockDemandId);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                    flag = true;
                }
                else if (btnSaveDocument.Text == "Update")
                {
                    savedRecordID = FranchiseSaleInvoiceController.Update_FranchiseSaleInvoice(Convert.ToInt64(hfMaster_ID.Value),
                        Convert.ToInt32(ddlLocation.SelectedItem.Value), Convert.ToInt64(ddlCustomer.SelectedItem.Value),
                        txtRefNo.Text, txtRemarks.Text, Convert.ToDecimal(txtTotalAmount.Text),
                        Convert.ToInt32(Session["UserID"]), CurrentWorkDate, false, false, false, 
                        Convert.ToDecimal(lblSalePer.Text), (DataTable)Session["PurchaseSKUS"],
                        IsFinanceSetting, dtConfig, stockDemandId);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                    flag = true;
                    mPopUpLocation.Hide();
                }
                if (flag)
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
                    ddlCustomer.Enabled = true;
                    btnSaveDocument.Text = "Save";
                    drpDocumentNo_SelectedIndexChanged(null, null);
                    if (savedRecordID > 0)
                    {
                        ShowReportPopUp(savedRecordID);
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

            if (decimal.Parse(_dc.chkNull_0(txtPrice.Text)) <= 0 && ddlPriceLevel.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter or select Price .');", true);
                txtPrice.Focus();
                return;
            }
            if (rdoDocType.SelectedIndex == 1 && decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) > decimal.Parse(_dc.chkNull_0(txtQtyToDeliver.Text)))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Quantity can not be greater than demanded Qty. Demanded Qty: "+ String.Format("{0:0.00}", txtQtyToDeliver.Text)+"');", true);
                txtQuantity.Focus();
                return;
            }
            if (rdoDocType.SelectedIndex == 1 && drpDocumentNo.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Demand No');", true);
                return;
            }

            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataTable dtskuPrice2 = (DataTable)Session["dtskuPrice2"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            DataRow[] foundRows2 = dtskuPrice2.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length > 0)
            {
                _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
                decimal currentStock = CheckStockStatus(int.Parse(_dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
                if (Convert.ToBoolean(Session["VALIDATE_STOCK"]) == false)
                {
                    currentStock = -1;
                }
                decimal Price = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                decimal priceFactorAmount = 0;
                decimal priceFactor = 0;
                if (ddlPriceLevel.SelectedItem.Value.ToString() != Constants.LongNullValue.ToString())
                {
                    DataTable dtPriceLevelDetail = LoadPriceLevelDetail(Convert.ToInt64(ddlPriceLevel.SelectedItem.Value));
                    foreach (DataRow dr in dtPriceLevelDetail.Rows)
                    {
                        if (ddlSkus.SelectedItem.Value.ToString() == dr["SKU_ID"].ToString() && dr["IsPercentWise"].ToString() == "True")
                        {
                            priceFactor = decimal.Parse(_dc.chkNull_0(dr["PRICE"].ToString()));

                            priceFactorAmount = decimal.Parse(_dc.chkNull_0(txtQuantity.Text))
                                * (Price * (decimal.Parse(_dc.chkNull_0(dr["PRICE"].ToString())) / 100));

                            Price = Price + (Price * (decimal.Parse(_dc.chkNull_0(dr["PRICE"].ToString())) / 100));
                            break;
                        }
                    }
                }
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
                            dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                            dr["UOM_DESC"] = txtUOM.Text;
                            if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                            {
                                dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                            }
                            else
                            {
                                dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            }

                            var totalAmount = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            var salePercent = decimal.Parse(_dc.chkNull_0(lblSalePer.Text));

                            var saleTaxAmount = totalAmount * (salePercent / 100);
                            dr["SalePercentAmount"] = saleTaxAmount;
                            dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                            dr["PRICE_LEVEL"] = Price;
                            dr["PRICE_LEVEL_AMOUNT"] = priceFactorAmount;
                            dr["PRICE_LEVEL_FACTOR"] = priceFactor;
                            dr["SKU_PRICES_LEVEL_ID"] = ddlPriceLevel.SelectedItem.Value;
                            dr["AMOUNT"] = (Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text))) + saleTaxAmount;
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
                        dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];

                        if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                        {
                            dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                        }
                        else
                        {
                            dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }

                        var totalAmount = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        var salePercent = decimal.Parse(_dc.chkNull_0(lblSalePer.Text));

                        var saleTaxAmount = totalAmount * (salePercent / 100);
                        dr["SalePercentAmount"] = saleTaxAmount;
                        dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                        dr["PRICE_LEVEL"] = Price;
                        dr["PRICE_LEVEL_AMOUNT"] = priceFactorAmount;
                        dr["PRICE_LEVEL_FACTOR"] = priceFactor;
                        dr["SKU_PRICES_LEVEL_ID"] = ddlPriceLevel.SelectedItem.Value;
                        dr["AMOUNT"] = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) + saleTaxAmount;
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
                ddlCustomer.Enabled = false;
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
        txtRefNo.Text = "";
        txtRemarks.Text = "";
        ddlCustomer.Enabled = true;
    }

    #endregion

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
        LoadCustomers();
        ddlSkus_SelectedIndexChanged(null, null);
    }
    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        lblStock.Text = "0";
        txtPrice.Text = "";
        txtQuantity.Enabled = true;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        if (dtskuPrice != null)
        {
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
                    LoadLastPurchasePrice();
                    txtPrice.Text = String.Format("{0:0.00}", foundRows[0]["DISTRIBUTOR_PRICE"]);
                    //lblLastPrice.Text = "Last Pur. Price:" + String.Format("{0:0.00}", foundRows[0]["LastPurPrice"]);
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
                        DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(ddlLocation.Value), CurrentWorkDate);
                        if (dsClosing.Tables[0].Rows.Count > 0)
                        {
                            lblStock.Text = String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
                        }
                    }
                    LoadPriceLevel();
                }
            }
        }
        if (Session["DemandedSKUs"] != null && rdoDocType.SelectedIndex == 1)
        {
            var demandSku = (DataTable)Session["DemandedSKUs"];
            DataRow[] demandQty = demandSku.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (demandQty != null && demandQty.Length > 0)
            {
                txtQuantity.Text = (Convert.ToDecimal(demandQty[0]["QUANTITY"].ToString()) - Convert.ToDecimal(demandQty[0]["DeliverQty"].ToString())).ToString();
                deliveredQty.Text = "Delivered Qty: " + String.Format("{0:0.00}", demandQty[0]["DeliverQty"].ToString());
                txtQtyToDeliver.Text = txtQuantity.Text;
            }
        }
        txtQuantity.Focus();
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadLookupGrid("");
    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        DataTable dtCustomer = (DataTable)Session["dtCustomer"];
        foreach (DataRow dr in dtCustomer.Rows)
        {
            if (dr["CUSTOMER_ID"].ToString() == ddlCustomer.SelectedItem.Value.ToString())
            {
                lblSalePer.Text = dr["SALES_PER"].ToString();
            }
        }
        LoadPriceLevel();
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
    }
    private void clearMaster()
    {
        ddlCustomer.SelectedIndex = 0;
        ddlCustomer_SelectedIndexChanged(null, null);
        txtRefNo.Text = "";
        txtRemarks.Text = "";
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

    public void ShowReportPopUp(long p_franchise_Master_ID)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            CrpSaleInvoice CrpReport = new CrpSaleInvoice();
            DataTable dt = mController.SelectReportTitle(int.Parse(ddlLocation.SelectedItem.Value.ToString()));
            DataSet ds = _franchiseController.SelectFranchiseReport(p_franchise_Master_ID);

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
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
            docRow.Visible = true;

            if (drpDocumentNo.Items.Count > 0)
            {
                LoadItemsFromDemand();
                ddlSkus_SelectedIndexChanged(null, null);
            }
            ddlCustomer.Enabled = false;
            qtyRow.Visible = true;
        }
        else
        {
            docRow.Visible = false;
            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_NAME", true);

            if (dtskuPrice.Rows.Count > 0)
                ddlSkus.SelectedIndex = 0;

            ddlSkus_SelectedIndexChanged(null, null);
            ddlCustomer.Enabled = true;
            qtyRow.Visible = false;
        }
    }
    private void LoadItemsFromDemand()
    {
        ddlSkus.Items.Clear();

        SKUPriceDetailController SKUCtl = new SKUPriceDetailController();

        if (ddlLocation.Items.Count > 0 && drpDocumentNo.Items.Count > 0)
        {
            DataTable dtskuPrice = SKUCtl.SelectDataPrice(Constants.IntNullValue, int.Parse(drpDocumentNo.SelectedItem.Value.ToString()),
                Constants.IntNullValue, Constants.IntNullValue,
                Convert.ToInt32(ddlLocation.Value), int.Parse(Session["UserId"].ToString()),
                Constants.IntNullValue, 10, Convert.ToDateTime(Session["CurrentWorkDate"]));

            clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_NAME", true);

            Session["DemandedSKUs"] = dtskuPrice;

            if (dtskuPrice.Rows.Count > 0)
            {
                ddlSkus.SelectedIndex = 0;
                ddlCustomer.Value = dtskuPrice.Rows[0]["DISTRIBUTOR_ID"].ToString();
            }
        }
    }
    private void GetDocumentNo()
    {
        try
        {
            drpDocumentNo.Items.Clear();
            PurchaseController mPurchase = new PurchaseController();
            DataTable dt = mPurchase.SelectPurchaseDocumentNo(25, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
            clsWebFormUtil.FillDxComboBoxList(drpDocumentNo, dt, "STOCK_DEMAND_ID", "DISTRIBUTOR_NAME", true);
            if (dt.Rows.Count > 0)
            {
                drpDocumentNo.SelectedIndex = 0;
            }
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Error:   " + EX.Message.ToString() + " ');", true);
        }
    }
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();

        if (rdoDocType.SelectedItem.Value.ToString() == "1")
        {
            LoadItemsFromDemand();
        }
        ddlSkus_SelectedIndexChanged(null, null);
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