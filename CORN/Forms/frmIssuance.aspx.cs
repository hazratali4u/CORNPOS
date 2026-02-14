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

public partial class Forms_frmIssuance : System.Web.UI.Page
{
    readonly SkuController _pController = new SkuController();
    readonly PurchaseController _mPurchaseCtrl = new PurchaseController();
    readonly DataControl _dc = new DataControl();

    DataTable _purchaseSkus;
    
    /// <summary>
    /// p_PrincipalId as section Id in GL Master
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            this.GetAppSettingDetail();
            DrpDocumentType.Focus();
            LoadDistributor();
            LoadSection();
            LoadSkuDetail();
            ddlSkus_SelectedIndexChanged(null, null);
            CreatTable();
            GetDocumentNo();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
        }
    }

    private void CreatTable()
    {
        _purchaseSkus = new DataTable();
        _purchaseSkus.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        _purchaseSkus.Columns.Add("SKU_ID", typeof(int));
        _purchaseSkus.Columns.Add("SKU_Code", typeof(string));
        _purchaseSkus.Columns.Add("SKU_Name", typeof(string));
        _purchaseSkus.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSkus.Columns.Add("UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("FREE_SKU", typeof(decimal));
        _purchaseSkus.Columns.Add("PS_QUANTITY", typeof(decimal));
        _purchaseSkus.Columns.Add("S_UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("PRICE", typeof(decimal));
        _purchaseSkus.Columns.Add("AMOUNT", typeof(decimal));

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

    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        DataTable dt = _mPurchaseCtrl.SelectPurchaseDocumentNo(Convert.ToInt32(DrpDocumentType.Value), Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
        drpDocumentNo.Items.Add(new DevExpress.Web.ListEditItem("New", Constants.LongNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDocumentNo, dt, 0, 0);
        drpDocumentNo.SelectedIndex = 0;
    }

    private void LoadSection()
    {
        SkuController _mSkuController = new SkuController();
        DataTable _mDt = _mSkuController.SelectProductSection(Constants.IntNullValue, null, null);
        clsWebFormUtil.FillDxComboBoxList(drpSection, _mDt, "SECTION_ID", "SECTION_NAME", true);
        if (_mDt.Rows.Count > 0) {
            drpSection.SelectedIndex = 0;
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
    }

    private void LoadDocumentDetail()
    {
        DataTable dt = _mPurchaseCtrl.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedItem.Value.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            drpSection.Value = dt.Rows[0]["PRINCIPAL_ID"].ToString();
            drpDistributor.Value = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
            txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            _purchaseSkus = _mPurchaseCtrl.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
        }
    }

    private void LoadGird()
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        if (_purchaseSkus != null)
        {
            GrdPurchase.DataSource = _purchaseSkus;

            decimal totalValue = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["FREE_SKU"].ToString()));
            txtTotalQuantity.Text = String.Format(CultureInfo.InvariantCulture, "{0:N}", totalValue);
            GrdPurchase.DataBind();
        }
    }

    private void LoadSkuDetail()
    {
        hfInventoryType.Value = "0";
        if (drpSection.Items.Count > 0)
        {
            ddlSkus.Items.Clear();
            DataTable dtskuPrice = new DataTable();
            if (Session["IsLocationWiseItem"].ToString() == "1")
            {
                dtskuPrice = _pController.GetSKUInfo(Convert.ToInt32(drpDistributor.Value),Convert.ToDateTime(Session["CurrentWorkDate"]),6);
            }
            else
            {
                dtskuPrice = _pController.GetSKUInfo(Convert.ToInt32(drpDistributor.Value), Convert.ToDateTime(Session["CurrentWorkDate"]), 5);
            }
                  
            if (dtskuPrice.Rows.Count > 0)
            {
                clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_DETAIL", true);
                ddlSkus.SelectedIndex = 0;
                txtUOM.Text = dtskuPrice.Rows[0]["UOM_DESC"].ToString();
                if (dtskuPrice.Rows[0]["IsInventoryWeight"].ToString() != "")
                {
                    if (Convert.ToBoolean(dtskuPrice.Rows[0]["IsInventoryWeight"]))
                    {
                        hfInventoryType.Value = "1";
                    }
                }
            }
            else
            {
                ddlSkus.DataSource = new DataTable();
                ddlSkus.DataBind();
            }
            Session.Add("Dtsku_Price", dtskuPrice);
        }
        ddlSkus_SelectedIndexChanged(null,null);
    }

    #endregion

    #region IndexChnage

    protected void DrpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {

        GetDocumentNo();
        DrpDocumentType.Focus();
    }
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
            ClearAll();
            drpSection.Enabled = true;
            drpDistributor.Enabled = true;
            DrpDocumentType.Enabled = true;
        }
        else
        {
            txtBuiltyNo.Text = "";

            drpSection.Enabled = false;
            drpDistributor.Enabled = false;
            DrpDocumentType.Enabled = false;
            LoadDocumentDetail();
            LoadSkuDetail();
        }
        drpDocumentNo.Focus();
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
        drpDistributor.Focus();
    }

    #endregion

    #region Grid Operations

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _rowNo.Value = e.NewEditIndex.ToString();
        ddlSkus.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        if (drpDocumentNo.Value.ToString() == Constants.LongNullValue.ToString())
        {
            _privouseQty.Value = "0";            
        }
        else
        {
            _privouseQty.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        }
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        }
        ddlSkus.Enabled = false;
        decimal closing = CheckStockStatus2(Convert.ToInt32(ddlSkus.Value)) + Convert.ToDecimal(_privouseQty.Value);
        lblStock.InnerHtml = "Stock: " + String.Format("{0:0.00}", closing);
        txtQuantity.Focus();
        btnSave.Text = "Update";
    }

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
            if (_purchaseSkus.Rows.Count > 0)
            {
                _purchaseSkus.Rows.RemoveAt(e.RowIndex);
                Session.Add("PurchaseSKUS", _purchaseSkus);
                LoadGird();
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion

    private bool CheckDuplicateSku(string SKUID)
    {
        try
        {
            _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

            DataRow[] foundRows = _purchaseSkus.Select("SKU_ID  = '" + SKUID + "'");
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

    #region Click OPerations

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) > 0 || drpDocumentNo.Value.ToString() != Constants.LongNullValue.ToString())
            {
                DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
                DataRow[] foundRows;
                string wQty = "";
                string ERPCode = "";
                decimal Qty = 0;
                if(cbScan.Checked)
                {
                    if (txtSKUCode.Text.Length > 0)
                    {
                        try
                        {
                            ERPCode = txtSKUCode.Text.Substring(0, 7);
                            wQty = txtSKUCode.Text.Substring(7, 6);
                            Convert.ToInt32(wQty);
                        }
                        catch (Exception ex)
                        {
                            wQty = "";
                        }
                        foundRows = dtskuPrice.Select("ERPCode = '" + ERPCode + "'");
                        if (foundRows.Length > 0 && wQty.Length > 0)
                        {
                            Qty = Convert.ToDecimal(wQty.Substring(0, 2) + "." + wQty.Substring(2, 4));
                        }
                        else
                        {
                            foundRows = dtskuPrice.Select("ERPCode = '" + txtSKUCode.Text + "'");
                            Qty = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    foundRows = dtskuPrice.Select("SKU_ID = '" + ddlSkus.SelectedItem.Value + "'");
                    Qty = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                }
                if (foundRows.Length > 0)
                {
                    _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
                    decimal currentStock = CheckStockStatus(int.Parse(_dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
                    decimal LastPrice = 0;
                    DataTable dtLastPurchasePrice = _pController.GetSKULastPurchasePrice(int.Parse(_dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())), Convert.ToInt32(drpDistributor.Value), bool.Parse(_dc.chkNull_0(foundRows[0]["IS_Recipe"].ToString())));
                    if(dtLastPurchasePrice.Rows.Count > 0)
                    {
                        LastPrice = Convert.ToDecimal(dtLastPurchasePrice.Rows[0]["LastPurchasePrice"]);
                    }
                    if (Convert.ToBoolean(Session["VALIDATE_STOCK"]) == false)
                    {
                        currentStock = -1;
                    }
                    if (btnSave.Text == "Add")
                    {
                        if (CheckDuplicateSku(foundRows[0]["SKU_ID"].ToString()))
                        {
                            if (currentStock == -1)
                            {
                                DataRow dr = _purchaseSkus.NewRow();
                                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                                dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                                dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                                dr["UOM_DESC"] = txtUOM.Text;
                                dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                                dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                                dr["FREE_SKU"] = Qty;
                                if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                                {
                                    dr["PS_QUANTITY"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), Qty, Constants.DecimalNullValue, "");
                                }
                                else
                                {
                                    dr["PS_QUANTITY"] = Qty;
                                }
                                dr["PRICE"] = LastPrice;
                                dr["AMOUNT"] = LastPrice * Qty;

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
                            if (cbScan.Checked)
                            {
                                foreach (DataRow dr in _purchaseSkus.Rows)
                                {
                                    if (dr["SKU_ID"].ToString() == foundRows[0]["SKU_ID"].ToString())
                                    {
                                        dr["FREE_SKU"] = Convert.ToDecimal(dr["FREE_SKU"]) + Qty;
                                        if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                                        {
                                            dr["PS_QUANTITY"] = Convert.ToDecimal(dr["PS_QUANTITY"]) + DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), Qty, Constants.DecimalNullValue, "");
                                        }
                                        else
                                        {
                                            dr["PS_QUANTITY"] = Convert.ToDecimal(dr["PS_QUANTITY"]) + Qty;
                                        }
                                        dr["PRICE"] = LastPrice;
                                        dr["AMOUNT"] = Convert.ToDecimal(dr["AMOUNT"]) + LastPrice * Qty;

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + " Already Exists ');", true);
                                return;
                            }
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
                            dr["UOM_DESC"] = txtUOM.Text;
                            dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                            dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                            dr["FREE_SKU"] = Qty;
                            if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                            {
                                dr["PS_QUANTITY"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), Qty, Constants.DecimalNullValue, "");
                            }
                            else
                            {
                                dr["PS_QUANTITY"] = Qty;
                            }
                            dr["PRICE"] = LastPrice;
                            dr["AMOUNT"] = LastPrice * Qty;
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
                    DisAbaleOption(true);
                    if (cbScan.Checked)
                    {
                        ScriptManager.GetCurrent(Page).SetFocus(txtSKUCode);
                    }
                    else
                    {
                        ScriptManager.GetCurrent(Page).SetFocus(ddlSkus);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Item Select');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Quantity');", true);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        //if (IsDayClosed())
        //{
        //    UserController UserCtl = new UserController();

        //    UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
        //    Session.Clear();
        //    System.Web.Security.FormsAuthentication.SignOut();
        //    Response.Redirect("../Login.aspx");
        //}
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        if (_purchaseSkus.Rows.Count > 0)
        {
            var mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, Convert.ToInt32(drpDistributor.Value));
            if (dt.Rows.Count > 0)
            {
                if (CalculatePurchase(DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString())))
                {
                    ShowReport(Convert.ToInt64(hfId.Value));
                    _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
                    _purchaseSkus.Rows.Clear();
                    Session.Add("PurchaseSKUS", _purchaseSkus);

                    LoadGird();
                    GetDocumentNo();
                    drpDistributor.Enabled = true;
                    drpSection.Enabled = true;
                    DrpDocumentType.Enabled = true;
                    LoadSkuDetail();
                    ClearAll();
                    txtBuiltyNo.Text = "";

                    txtTotalQuantity.Text = "";
                    DisAbaleOption(false);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' At least one Item must enter');", true);

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisAbaleOption(false);
        CreatTable();
        LoadGird();
        ClearAll();
        txtBuiltyNo.Text = "";
    }

    #endregion
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

    private bool CalculatePurchase(DateTime mWorkDate)
    {
        var dtPurchaseDetail = (DataTable)Session["PurchaseSKUS"];

        bool mResult = false;
        DataTable dtConfig = GetCOAConfiguration();
        bool IsFinanceSetting = GetFinanceConfig();
        if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
        {
            long id = _mPurchaseCtrl.InsertIssuance(int.Parse(drpDistributor.SelectedItem.Value.ToString()), "", int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
           , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpSection.SelectedItem.Value.ToString()), 0, false, dtPurchaseDetail, 0
           , txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpSection.SelectedItem.Value.ToString()), dtConfig, IsFinanceSetting);

            if (id > 0)
            {
                mResult = true;
                hfId.Value = Convert.ToString(id);
            }
        }
        else
        {
            mResult = _mPurchaseCtrl.UpdateIssuance(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), "", int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
           , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpSection.SelectedItem.Value.ToString()), 0, false, dtPurchaseDetail, 0
           , txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpSection.SelectedItem.Value.ToString()), dtConfig, IsFinanceSetting);

            hfId.Value = drpDocumentNo.SelectedItem.Value.ToString();
        }
        return mResult;
    }
    private decimal CheckStockStatus(int skuId)
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
            if (DrpDocumentType.SelectedItem.Value.ToString() == "14")
            {
                return -1;
            }
            else
            {
                var mController = new PhaysicalStockController();
                DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.Value.ToString()), skuId, "N/A", CurrentWorkDate, 15);
                if (dt.Rows.Count > 0)
                {
                    if (decimal.Parse(dt.Rows[0][0].ToString()) + Convert.ToDecimal(_privouseQty.Value) >= decimal.Parse(_dc.chkNull_0(txtQuantity.Text)))
                    {
                        return -1;
                    }
                    else
                    {
                        return decimal.Parse(dt.Rows[0][0].ToString()) + Convert.ToDecimal(_privouseQty.Value);
                    }
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
    private decimal CheckStockStatus2(int skuId)
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
            var mController = new PhaysicalStockController();
            DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.Value.ToString()), skuId, "N/A", CurrentWorkDate, 15);
            if (dt.Rows.Count > 0)
            {
                return decimal.Parse(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            return 0;
        }        
    }
    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            DrpDocumentType.Enabled = false;
            drpSection.Enabled = false;
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;
        }
        else
        {
            DrpDocumentType.Enabled = true;
            drpSection.Enabled = true;
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    private void ClearAll()
    {

        if (cbScan.Checked)
        {
            txtQuantity.Text = "1";
        }
        else
        {
            txtQuantity.Text = "";
        }
        txtSKUCode.Text = "";
        ddlSkus.Enabled = true;
        btnSave.Text = "Add";
        _privouseQty.Value = "0";
    }

    private void ShowReport(long Id)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            RptInventoryController RptInventoryCtl = new RptInventoryController();
            CrpIssueDocument CrpReport = new CrpIssueDocument();
            DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.Value.ToString()));
            DataSet ds = RptInventoryCtl.SelectIssueDocument(int.Parse(drpDistributor.Value.ToString()),int.Parse(drpSection.Value.ToString()), Constants.DateNullValue, Constants.DateNullValue, int.Parse(DrpDocumentType.Value.ToString()), Id);
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            if (DrpDocumentType.SelectedItem.Value.ToString() == "19")
            {
                CrpReport.SetParameterValue("DocumentType", "Issue To");
                CrpReport.SetParameterValue("ReportName", "Stock Issuance Document");
                CrpReport.SetParameterValue("IssuedBy", "Issue By");
                CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
            }
            else
            {
                CrpReport.SetParameterValue("DocumentType", "Return From");
                CrpReport.SetParameterValue("ReportName", "Stock Return Document");
                CrpReport.SetParameterValue("IssuedBy", "Return By");
                CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
            }
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
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

    protected void GrdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[1].Text = Convert.ToDecimal(txtTotalQuantity.Text).ToString("N");
            e.Row.Cells[0].Font.Bold = true;
            e.Row.Cells[1].Font.Bold = true;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
        }
    }
    
    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
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
            }
            lblStock.InnerHtml = "Stock: " + String.Format("{0:0.00}", CheckStockStatus2(Convert.ToInt32(ddlSkus.SelectedItem.Value)));
        }
        txtQuantity.Focus();
    }

    public void ShowIssuancePopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CORNBusinessLayer.Reports.CrpIssuanceDocument CrpReport = new CORNBusinessLayer.Reports.CrpIssuanceDocument();
        DataSet ds = null;
        if (type != 0)
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 5);
        }
        else
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(Constants.IntNullValue, 6);
        }

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        if(DrpDocumentType.SelectedItem.Value.ToString() == "19")
        {
            CrpReport.SetParameterValue("DocumentType", "Issue To");
            CrpReport.SetParameterValue("ReportName", "Stock Issuance Document");
            CrpReport.SetParameterValue("IssuedBy", "Issue By");
        }
        else
        {
            CrpReport.SetParameterValue("DocumentType", "Return From");
            CrpReport.SetParameterValue("ReportName", "Stock Return Document");
            CrpReport.SetParameterValue("IssuedBy", "Return By");
        }
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
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

    protected void cbScan_CheckedChanged(object sender, EventArgs e)
    {
        if (cbScan.Checked)
        {
            ddlSkus.Visible = false;
            txtSKUCode.Visible = true;
            txtQuantity.Text = "1";
            txtSKUCode.Focus();
        }
        else
        {
            ddlSkus.Visible = true;
            txtSKUCode.Visible = false;
            ddlSkus.Focus();
        }
    }
}