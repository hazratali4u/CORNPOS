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
using DevExpress.Web;
using System.Drawing;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmPurchaseOrder : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly SkuController SKUCtl = new SkuController();
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
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
            LoadLocations();
            LoadPrincipal();
            LoadSkuDetail();
            ddlLocation.Focus();
            LoadGridData();
            LoadLookupGrid("");
            contentBox.Visible = false;
            lookupBox.Visible = true;
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            ddlSkus_SelectedIndexChanged(null, null);
            CreatTable();

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

            if (CurrentWorkDate != null && CurrentWorkDate != Constants.DateNullValue)
            {
                txtDeliveryDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
                txtExpiryDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
            }
        }
    }

    #region Load
    private void LoadSkuDetail()
    {
        hfInventoryType.Value = "0";
        if (ddlLocation.Items.Count > 0)
        {
            ddlSkus.Items.Clear();
            DataTable dtskuPrice = new DataTable();
            if (Session["IsLocationWiseItem"].ToString() == "1")
            {
                dtskuPrice = SKUCtl.GetSKUInfo(Convert.ToInt32(ddlLocation.Value), Constants.DateNullValue, 4);
            }
            else
            {
                dtskuPrice = SKUCtl.GetSKUInfo(Convert.ToInt32(ddlLocation.Value), Constants.DateNullValue, 3);
            }
            clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_NAME", true);
            if (dtskuPrice.Rows.Count > 0)
            {
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
        }
    }
    private void LoadLocations()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);
        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);
    }
    private void LoadPrincipal()
    {
        DataTable mDt = _pController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 4, Constants.DateNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddlSupplier, mDt, 0, 1, true);

        if (mDt.Rows.Count > 0)
        {
            ddlSupplier.SelectedIndex = 0;
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
        _purchaseSkus.Columns.Add("PRICE", typeof(decimal));
        _purchaseSkus.Columns.Add("Quantity", typeof(decimal));
        _purchaseSkus.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("S_UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("S_Quantity", typeof(decimal));
        _purchaseSkus.Columns.Add("DiscountPercentage", typeof(decimal));
        _purchaseSkus.Columns.Add("TaxPercentage", typeof(decimal));

        Session.Add("PurchaseSKUS", _purchaseSkus);

    }
    #endregion

    //#region Grid Operations
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        var purchaseController = new PurchaseController();
        var purchaseDetail = purchaseController.getPurchaseByPurchaseOrderMasterID(Convert.ToInt64(Grid_users.Rows[e.NewEditIndex].Cells[1].Text));

        if (purchaseDetail != null && purchaseDetail.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "alert('Record cannot be modified as some Qty is received in GRN');", true);
        }
        else
        {
            _rowNo.Value = e.NewEditIndex.ToString();
            PurchaseOrderID.Value = Grid_users.Rows[e.NewEditIndex].Cells[1].Text;
            ddlLocation.Value = Grid_users.Rows[e.NewEditIndex].Cells[2].Text;
            ddlSupplier.Value = Grid_users.Rows[e.NewEditIndex].Cells[4].Text;
            ddlPaymentMode.Value = Grid_users.Rows[e.NewEditIndex].Cells[6].Text;
            txtDeliveryDate.Text = Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[7].Text).ToString("dd-MMM-yyyy");
            txtExpiryDate.Text = Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[8].Text).ToString("dd-MMM-yyyy");
            txtRemarks.Text = Grid_users.Rows[e.NewEditIndex].Cells[10].Text;
            btnSave_Document.Text = "Update";

            GetPurchaseOrderDetailForUpdate(Convert.ToInt64(PurchaseOrderID.Value));
            Document_Date.Value = Grid_users.Rows[e.NewEditIndex].Cells[15].Text;
            //txtGstAmount.Text = Grid_users.Rows[e.NewEditIndex].Cells[11].Text;
            //txtDiscount.Text = Grid_users.Rows[e.NewEditIndex].Cells[12].Text;
            //txtNetAmount.Text = Grid_users.Rows[e.NewEditIndex].Cells[13].Text;

            contentBox.Visible = true;
            lookupBox.Visible = false;
            contentBox.Visible = true;
            lookupBox.Visible = false;
            searchBox.Visible = false;
            searchBtn.Visible = false;
            btnForceClose.Visible = false;
            btnCancel.Visible = true;
            btnSave.Visible = true;
            btnAdd.Visible = false;
        }
    }

    public void GetPurchaseOrderDetailForUpdate(long purchaseOrderMaster_ID)
    {
        CreatTable();

        var purchaseController = new PurchaseController();
        var purchaseDetail = purchaseController.GetPurchaseOrderDetailForUpdate(purchaseOrderMaster_ID);


        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        foreach (DataRow item in purchaseDetail.Rows)
        {
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + item["SKU_ID"].ToString() + "'");
            if (foundRows.Length > 0)
            {
                DataRow dr = _purchaseSkus.NewRow();
                dr["SKU_ID"] = Convert.ToInt32(item["SKU_ID"].ToString());
                dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                dr["UOM_ID"] = Convert.ToInt32(item["UOM_ID"].ToString());
                dr["Quantity"] = decimal.Parse(item["QUANTITY"].ToString());
                dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                dr["UOM_DESC"] = item["UOM_DESC"].ToString();
                dr["DiscountPercentage"] = decimal.Parse(item["Discount_Percentage"].ToString());
                dr["TaxPercentage"] = decimal.Parse(item["Tax_Percentage"].ToString());
                if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                {
                    dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                }
                else
                {
                    dr["S_Quantity"] = decimal.Parse(item["QUANTITY"].ToString());
                }

                dr["PRICE"] = decimal.Parse(item["PRICE"].ToString());
                dr["AMOUNT"] = decimal.Parse(item["AMOUNT"].ToString());

                _purchaseSkus.Rows.Add(dr);
            }
        }
        LoadGird();
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        var purchaseController = new PurchaseController();
        dt = purchaseController.SelectPuchaseOrder(Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(Session["UserID"]),2);
        Session.Add("dtGridData", dt);
    }

    protected void LoadLookupGrid(string Type)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();

        DataTable dt = (DataTable)Session["dtGridData"];

        if (dt != null && dt.Rows.Count > 0)
        {
            if (Type == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR Convert(ID, System.String) LIKE '" + txtSearch.Text + "'";
                }
                else
                {
                    dt.DefaultView.RowFilter = "ID > 0";
                }
            }
        }
        Grid_users.DataSource = dt;
        Grid_users.DataBind();

    }

    protected void Grid_users_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          // ((LinkButton)e.Row.FindControl("btnEdit")).OnClientClick = "return HideUnhideFields("+e+ "',' " + chkIsTemporaryClosed+ ");";
        }
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadLookupGrid("IndexChanged");
    }
    //#endregion

    //#region Click OPerations

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadLookupGrid("");
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                long purchase_Order_Master_ID = 0;

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);

                if (GridViewPurchase.Rows.Count > 0)
                {
                    DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
                    var purchaseController = new PurchaseController();
                    if (btnSave_Document.Text == "Save")
                    {

                        purchase_Order_Master_ID = purchaseController.InsertPurchaseOrder(Convert.ToInt32(_dc.chkNull_0(ddlSupplier.Value.ToString())),
                            Convert.ToInt32(_dc.chkNull_0(ddlLocation.Value.ToString())),
                            Convert.ToInt32(_dc.chkNull_0(ddlPaymentMode.Value.ToString())),
                            Convert.ToDateTime(txtDeliveryDate.Text), Convert.ToDateTime(txtExpiryDate.Text)
                            , 0, txtRemarks.Text,
                            Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)),
                            Convert.ToDecimal(_dc.chkNull_0(txtGstAmount.Text)),
                            Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text)),
                            Convert.ToDecimal(_dc.chkNull_0(txtNetAmount.Text)),
                            Convert.ToInt32(Session["UserID"]),
                            currentWorkDate,
                            GridViewPurchase
                            );
                    }
                    else if (btnSave_Document.Text == "Update")
                    {
                        if (Document_Date.Value == "")
                            Document_Date.Value = currentWorkDate.ToString();

                        purchase_Order_Master_ID = purchaseController.UpdatePurchaseOrder(Convert.ToInt32(_dc.chkNull_0(ddlSupplier.Value.ToString())),
                             Convert.ToInt32(_dc.chkNull_0(ddlLocation.Value.ToString())),
                             Convert.ToInt32(_dc.chkNull_0(ddlPaymentMode.Value.ToString())),
                             Convert.ToDateTime(txtDeliveryDate.Text), Convert.ToDateTime(txtExpiryDate.Text)
                             , 0, txtRemarks.Text,
                             Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)),
                             Convert.ToDecimal(_dc.chkNull_0(txtGstAmount.Text)),
                             Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text)),
                             Convert.ToDecimal(_dc.chkNull_0(txtNetAmount.Text)),
                             Convert.ToInt32(Session["UserID"]),
                             Convert.ToDateTime(Document_Date.Value),
                             Convert.ToInt64(PurchaseOrderID.Value),
                             GridViewPurchase
                             );
                    }
                    if (purchase_Order_Master_ID > 0)
                    {
                        ClearAll();
                        CreatTable();
                        txtRemarks.Text = "";
                        LoadGird();
                        LoadGridData();
                        LoadLookupGrid("");
                        btnSave_Document.Text = "Save";
                        ShowReportPopUp(purchase_Order_Master_ID);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Please add Item Details');", true);
                }
            }
            else
            {
                //LoadApprovalBy();
            }
        }

        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        btnSave_Document.Text = "Save";
        contentBox.Visible = false;
        lookupBox.Visible = true;
        searchBox.Visible = true;
        searchBtn.Visible = true;
        btnForceClose.Visible = true;
        btnCancel.Visible = false;
        btnSave.Visible = false;
        btnAdd.Visible = true;
        ClearAll();
        CreatTable();
        txtRemarks.Text = "";
        LoadGird();

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        contentBox.Visible = true;
        lookupBox.Visible = false;
        searchBox.Visible = false;
        searchBtn.Visible = false;
        btnForceClose.Visible = false;
        btnCancel.Visible = true;
        btnSave.Visible = true;
        btnAdd.Visible = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //LoadGird();
        ClearAll();
    }

    //#endregion
    #region Grid Operations

    protected void GridViewPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _rowNo2.Value = e.NewEditIndex.ToString();
        ddlSkus.Value = GridViewPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtQuantity.Text = GridViewPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        _privouseQty.Value = GridViewPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtPrice.Text = GridViewPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtAmount.Text = GridViewPurchase.Rows[e.NewEditIndex].Cells[8].Text;
        txtTaxPercent.Text = GridViewPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        txtDiscountPercent.Text = GridViewPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        }
        ddlSkus.Enabled = false;
        txtQuantity.Focus();
        btnSave.Text = "Update";
        lblStock.Text = "Closing Stock: 0";
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
                lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
            }
        }
    }

    protected void GridViewPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus.Rows.Count > 0)
        {
            _purchaseSkus.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
        }
    }
    private void LoadGird()
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        decimal totalQty = 0;
        decimal totalGrossAmount = 0;
        decimal totalDiscountAmount = 0;
        decimal totalTaxAmount = 0;
        decimal grandTotal = 0;

        if (_purchaseSkus != null)
        {
            GridViewPurchase.DataSource = _purchaseSkus;
            GridViewPurchase.DataBind();

            foreach (DataRow item in _purchaseSkus.Rows)
            {
                var qty = item["Quantity"].ToString();
                var price = item["PRICE"].ToString();
                var discountPercent = item["DiscountPercentage"].ToString();
                var taxPercent = item["TaxPercentage"].ToString();

                if (qty == null || qty == "")
                {
                    qty = "0";
                }
                if (price == null || price == "")
                {
                    price = "0";
                }
                if (discountPercent == null || discountPercent == "")
                {
                    discountPercent = "0";
                }
                if (taxPercent == null || taxPercent == "")
                {
                    taxPercent = "0";
                }

                totalQty = Convert.ToDecimal(totalQty) + Convert.ToDecimal(qty);
                var grossAmount = Convert.ToDecimal(qty) * Convert.ToDecimal(price);
                totalGrossAmount = totalGrossAmount + grossAmount;
                var discountAmount = grossAmount * (Convert.ToDecimal(discountPercent) / 100);
                totalDiscountAmount = totalDiscountAmount + discountAmount;
                var amountAfterDiscount = grossAmount - discountAmount;



                var taxAmount = amountAfterDiscount * (Convert.ToDecimal(taxPercent) / 100);
                totalTaxAmount = totalTaxAmount + taxAmount;
                var amountAfterTax = amountAfterDiscount + taxAmount;
                grandTotal = grandTotal + amountAfterTax;
            }
        }

        txtGstAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalTaxAmount);
        txtDiscount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalDiscountAmount);
        txtTotalQuantity.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalQty);
        txtTotalAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalGrossAmount);
        txtNetAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", grandTotal);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Quantity.');", true);
                txtQuantity.Focus();
                return;
            }
            if (decimal.Parse(_dc.chkNull_0(txtPrice.Text)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Price.');", true);
                txtPrice.Focus();
                return;
            }

            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length > 0)
            {
                _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

                if (btnSave.Text == "Add")
                {
                    if (CheckDublicateSku())
                    {
                        DataRow dr = _purchaseSkus.NewRow();
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                        dr["UOM_DESC"] = txtUOM.Text;
                        dr["DiscountPercentage"] = decimal.Parse(_dc.chkNull_0(txtDiscountPercent.Text));
                        dr["TaxPercentage"] = decimal.Parse(_dc.chkNull_0(txtTaxPercent.Text));
                        if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                        {
                            dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                        }
                        else
                        {
                            dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }

                        dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                        dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtAmount.Text));

                        _purchaseSkus.Rows.Add(dr);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + " Already Exists ');", true);
                        return;
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    DataRow dr = _purchaseSkus.Rows[Convert.ToInt32(_rowNo2.Value)];
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                    dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                    dr["UOM_DESC"] = txtUOM.Text;
                    dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                    dr["DiscountPercentage"] = decimal.Parse(_dc.chkNull_0(txtDiscountPercent.Text));
                    dr["TaxPercentage"] = decimal.Parse(_dc.chkNull_0(txtTaxPercent.Text));

                    if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                    {
                        dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                    }
                    else
                    {
                        dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                    }

                    dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                    dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtAmount.Text));

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


    #endregion

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
    private void ClearAll()
    {

        txtQuantity.Text = "";
        ddlSkus.Enabled = true;
        txtPrice.Text = "";
        txtTaxPercent.Text = "";
        txtDiscountPercent.Text = "";

        txtAmount.Text = "0";
        btnSave.Text = "Add";
        _privouseQty.Value = "0";
    }
    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPrice.Text = "";
        txtQuantity.Enabled = true;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        lblLastPrice.Text = "Last Price: 0";
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
        }
        txtQuantity.Focus();
        lblStock.Text = "Closing Stock: 0";
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
                lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
            }
            if(dsClosing.Tables[0].Rows.Count > 0)
            {
                txtPrice.Text = String.Format("{0:0.00}", dsClosing.Tables[1].Rows[0]["PRICE"]);
                lblLastPrice.Text = "Last Price: " + String.Format("{0:0.00}", dsClosing.Tables[1].Rows[0]["PRICE"]);
            }
        }
    }
    protected void ddlLocation_SelectedIndex_Changed(object sender, EventArgs e)
    {
        LoadSkuDetail();
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
        if (CurrentWorkDate != null && CurrentWorkDate != Constants.DateNullValue)
        {
            txtDeliveryDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
            txtExpiryDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
        }
        lblStock.Text = "Closing Stock: 0";
        lblLastPrice.Text = "Last Price: 0";
        if (ddlSkus.Items.Count > 0 && ddlLocation.Items.Count > 0)
        {
            DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(ddlLocation.Value),CurrentWorkDate);
            if (dsClosing.Tables[0].Rows.Count > 0)
            {
                lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
            }
            if (dsClosing.Tables[0].Rows.Count > 0)
            {
                txtPrice.Text = String.Format("{0:0.00}", dsClosing.Tables[1].Rows[0]["PRICE"]);
                lblLastPrice.Text = "Last Price: " + String.Format("{0:0.00}", dsClosing.Tables[1].Rows[0]["PRICE"]);
            }
        }
    }
    protected void btnForceClose_Click(object sender, EventArgs e)
    {
        var purchaseController = new PurchaseController();

        try
        {
            int checkedRowsCount = Grid_users.Rows.Cast<GridViewRow>()
                            .Count(r => ((CheckBox)r.FindControl("ChbIsAssigned")).Checked);

            if (checkedRowsCount == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alert", "alert('Please select any record to Force Close.');", true);
            }
            else
            {
                foreach (GridViewRow item in Grid_users.Rows)
                {
                    CheckBox chkbox = (CheckBox)item.FindControl("ChbIsAssigned");

                    if (chkbox.Checked == true)
                    {
                        var purchaseOrderMasterID = Convert.ToInt64(item.Cells[1].Text.ToString());
                        purchaseController.ForceClosePurchaseOrder(purchaseOrderMasterID);
                    }
                }
                LoadGridData();
                LoadLookupGrid("");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alert", "alert('Selected record marked as closed');", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void ShowReportPopUp(long p_Purchase_Order_ID)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            var purchaseController = new PurchaseController();
            CrpPurchaseOrder CrpReport = new CrpPurchaseOrder();
            DataTable dt = mController.SelectReportTitle(int.Parse(ddlLocation.SelectedItem.Value.ToString()));
            DataSet ds = purchaseController.SelectPurchaseOrderReport(p_Purchase_Order_ID);

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("Location", ddlLocation.SelectedItem.Text);
            CrpReport.SetParameterValue("ReportName", "PURCHASE ORDER");
            CrpReport.SetParameterValue("Username", Session["UserName"].ToString());

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
}