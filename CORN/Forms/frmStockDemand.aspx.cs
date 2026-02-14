using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.IO.Ports;

/// <summary>
/// From To Adjust Stock
/// </summary>
public partial class Forms_frmStockDemand : System.Web.UI.Page
{
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptInventoryController _rptInvenController = new RptInventoryController();
    readonly SkuController SKUCtl = new SkuController();
    readonly DataControl _dc = new DataControl();
    DataTable _purchaseSku;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            CreatTable();
            GetDocumentNo();
            LoadSkuDetail();
        }
    }

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("STOCK_DEMAND_ID", typeof(long));
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_Code", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSku.Columns.Add("PRICE", typeof(decimal));
        _purchaseSku.Columns.Add("QUANTITY", typeof(decimal));
        _purchaseSku.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("UOM_ID", typeof(int));
        _purchaseSku.Columns.Add("STOCK_DEMAND_DETAIL_ID", typeof(int));
        _purchaseSku.Columns.Add("DISTRIBUTOR_ID", typeof(int));
        _purchaseSku.Columns.Add("REMARKS", typeof(string));
        _purchaseSku.Columns.Add("S_Quantity", typeof(decimal));
        _purchaseSku.Columns.Add("S_UOM_ID", typeof(decimal));
        _purchaseSku.Columns.Add("FINISHED_GOOD_ID", typeof(int));
        _purchaseSku.Columns.Add("IS_Recipe", typeof(bool));
        Session.Add("PurchaseSKU", _purchaseSku);
    }

    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        // DateTime MWorkDate = System.DateTime.Now;

        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(20, int.Parse(drpDistributor.Value.ToString()), Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
        drpDocumentNo.Items.Add(new DevExpress.Web.ListEditItem("New", Constants.LongNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDocumentNo, dt, 0, 0, false);

        drpDocumentNo.SelectedIndex = 0;
    }

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.Value.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            LoadGird();
            drpDistributor.Enabled = true;
            ClearAll();
            txtDocumentNo.Text = "";
            chkFranchiseDemand.Checked = false;
            DisAbaleOption(false);
        }
        else
        {
            drpDistributor.Enabled = false;
            LoadDocumentDetail();
            LoadSkuDetail();
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));        
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", true);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
        GetDocumentNo();
    }

    private void LoadSkuDetail()
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

        hfInventoryType.Value = "0";
        SkuController SKUCtl = new SkuController();
        ddlSkus.Items.Clear();
        DataTable dtskuPrice = new DataTable();
        if (Session["IsLocationWiseItem"].ToString() == "1")
        {
            dtskuPrice = SKUCtl.GetSKUInfo(int.Parse(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate,11);
        }
        else
        {
            dtskuPrice = SKUCtl.GetSKUInfo(int.Parse(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate, 10);
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
        if (ddlSkus.Items.Count > 0 && drpDistributor.Items.Count > 0)
        {
            DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(drpDistributor.Value),CurrentWorkDate);
            if (dsClosing.Tables[0].Rows.Count > 0)
            {
                lblStock.Text = "Stock: " + String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
            }
        }
    }

    private void LoadGird()
    {
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        GrdPurchase.DataSource = _purchaseSku;
        GrdPurchase.DataBind();
    }

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _rowNo.Value = e.NewEditIndex.ToString();
        ddlSkus.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        // txtPrice.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        }
        ddlSkus.Enabled = false;
        txtQuantity.Focus();
        btnSave.Text = "Update";

    }

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        if (_purchaseSku.Rows.Count > 0)
        {
            _purchaseSku.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
        }
    }
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        {
            DataTable dt = mPurchase.selectStockDemandDetail(Convert.ToInt32(drpDistributor.Value), int.Parse(drpDocumentNo.Value.ToString()),Convert.ToDateTime(Session["CurrentWorkDate"]));
            if (dt.Rows.Count > 0)
            {
                drpDistributor.Value = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
                txtDocumentNo.Text = dt.Rows[0]["REMARKS"].ToString();
                chkFranchiseDemand.Checked = Convert.ToBoolean(dt.Rows[0]["IsFranchiseDemand"].ToString());
            }
            _purchaseSku = dt;
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
        }
    }

    private bool CheckDublicateSku()
    {
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        DataRow[] foundRows = _purchaseSku.Select("SKU_ID  = '" + ddlSkus.Value + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds Document Detail To Document Detail Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) > 0 || drpDocumentNo.Value.ToString() != Constants.LongNullValue.ToString())
        {
            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.Value + "'");
            if (foundRows.Length > 0)
            {
                _purchaseSku = (DataTable)Session["PurchaseSKU"];

                if (btnSave.Text == "Add")
                {
                    if (CheckDublicateSku())
                    {
                        DataRow dr = _purchaseSku.NewRow();
                        dr["FINISHED_GOOD_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                        dr["QUANTITY"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        dr["UOM_DESC"] = txtUOM.Text;
                        dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                        dr["IS_Recipe"] = foundRows[0]["IS_Recipe"];
                        if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                        {
                            dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                        }
                        else
                        {
                            dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }
                        _purchaseSku.Rows.Add(dr);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem + " Already Exists ');", true);

                        return;
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    DataRow dr = _purchaseSku.Rows[Convert.ToInt32(_rowNo.Value)];
                    dr["FINISHED_GOOD_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                    dr["UOM_DESC"] = txtUOM.Text;
                    dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                    dr["QUANTITY"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                    dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                    dr["IS_Recipe"] = foundRows[0]["IS_Recipe"];
                    if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                    {
                        dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                    }
                    else
                    {
                        dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                    }
                }
                Session.Add("PurchaseSKU", _purchaseSku);
                ClearAll();
                LoadGird();
                DisAbaleOption(true);
                ScriptManager.GetCurrent(Page).SetFocus(ddlSkus);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong item please check in list');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Quantity');", true);
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
    /// <summary>
    /// Saves Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        var mDayClose = new DistributorController();
        int StockDemandId;
        DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.Value.ToString()));
        if (dt.Rows.Count > 0)
        {
            DateTime mWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());
            PurchaseController mController = new PurchaseController();
            SkuController skuCtrl = new SkuController();
            DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKU"];
            DataTable dtConfig = GetCOAConfiguration();
            DataTable dtStockDemand = new DataTable();
            dtStockDemand.Columns.Add("SKU_ID", typeof(int));
            dtStockDemand.Columns.Add("PRICE", typeof(decimal));
            dtStockDemand.Columns.Add("QUANTITY", typeof(decimal));
            dtStockDemand.Columns.Add("UOM_ID", typeof(int));
            dtStockDemand.Columns.Add("FINISHED_GOOD_ID", typeof(int));
            dtStockDemand.Columns.Add("FINISHED_GOOD_QTY", typeof(decimal));
            DataRow drStockDemand;
            foreach (DataRow dr in dtPurchaseDetail.Rows)
            {
                if (dr["IS_Recipe"].ToString().ToLower() == "true")
                {
                    DataTable dtFinishedGoodItems = skuCtrl.GetFinshedDetail(Convert.ToInt32(dr["FINISHED_GOOD_ID"]), int.Parse(drpDistributor.SelectedItem.Value.ToString()), 6);
                    if (dtFinishedGoodItems.Rows.Count > 0)
                    {
                        foreach (DataRow drFinish in dtFinishedGoodItems.Rows)
                        {
                            drStockDemand = dtStockDemand.NewRow();
                            drStockDemand["SKU_ID"] = drFinish["SKU_ID"];
                            drStockDemand["PRICE"] = drFinish["PRICE"];
                            drStockDemand["QUANTITY"] = Convert.ToDecimal(drFinish["QUANTITY"]) * Convert.ToDecimal(dr["QUANTITY"]);
                            drStockDemand["FINISHED_GOOD_QTY"] = dr["QUANTITY"];
                            drStockDemand["UOM_ID"] = drFinish["UOM_ID"];
                            drStockDemand["FINISHED_GOOD_ID"] = dr["FINISHED_GOOD_ID"];
                            dtStockDemand.Rows.Add(drStockDemand);
                        }
                    }
                    else
                    {
                        drStockDemand = dtStockDemand.NewRow();
                        drStockDemand["SKU_ID"] = dr["SKU_ID"];
                        drStockDemand["PRICE"] = dr["PRICE"];
                        drStockDemand["QUANTITY"] = dr["QUANTITY"];
                        drStockDemand["FINISHED_GOOD_QTY"] = dr["QUANTITY"];
                        drStockDemand["UOM_ID"] = dr["UOM_ID"];
                        drStockDemand["FINISHED_GOOD_ID"] = dr["FINISHED_GOOD_ID"];
                        dtStockDemand.Rows.Add(drStockDemand);
                    }
                }
                else
                {
                    drStockDemand = dtStockDemand.NewRow();
                    drStockDemand["SKU_ID"] = dr["SKU_ID"];
                    drStockDemand["PRICE"] = dr["PRICE"];
                    drStockDemand["QUANTITY"] = dr["QUANTITY"];
                    drStockDemand["FINISHED_GOOD_QTY"] = dr["QUANTITY"];
                    drStockDemand["UOM_ID"] = dr["UOM_ID"];
                    drStockDemand["FINISHED_GOOD_ID"] = dr["FINISHED_GOOD_ID"];
                    dtStockDemand.Rows.Add(drStockDemand);
                }
            }
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                StockDemandId = mController.InsertStockDemand(int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate,
                    dtStockDemand, int.Parse(Session["UserId"].ToString()), txtDocumentNo.Text, chkFranchiseDemand.Checked);
            }
            else
            {
                bool mResult = mController.updateStockDemand(int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate,
                    dtStockDemand, int.Parse(Session["UserId"].ToString()), txtDocumentNo.Text,
                    int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), chkFranchiseDemand.Checked);
                StockDemandId = int.Parse(drpDocumentNo.SelectedItem.Value.ToString());
            }

            lblErrorMsg.Text = "Record Upated";
            _purchaseSku = (DataTable)Session["PurchaseSKU"];
            _purchaseSku.Rows.Clear();
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
            GetDocumentNo();
            ClearAll();
            txtDocumentNo.Text = "";
            chkFranchiseDemand.Checked = false;
            DisAbaleOption(false);
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
            PrintDemand(StockDemandId);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CreatTable();
        LoadGird();
        drpDistributor.Enabled = true;
        ClearAll();
        txtDocumentNo.Text = "";
        chkFranchiseDemand.Checked = false;
        DisAbaleOption(false);
    }

    /// <summary>
    /// Enables/Disables Controls
    /// </summary>
    /// <param name="IsDisable">bool</param>
    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;
        }
        else
        {
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Clears Form Controls
    /// </summary>
    private void ClearAll()
    {

        txtQuantity.Text = "1";
        //  txtPrice.Text = string.Empty;
        ddlSkus.Enabled = true;
        btnSave.Text = "Add";
        lblErrorMsg.Text = "";

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

    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblStock.Text = "Stock: 0";
        txtQuantity.Enabled = true;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");

        if (ddlSkus.Items.Count > 0 && drpDistributor.Items.Count > 0)
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

            DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(drpDistributor.Value),CurrentWorkDate);
            if (dsClosing.Tables[0].Rows.Count > 0)
            {
                lblStock.Text = "Stock: " + String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
            }
        }

        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
            if (foundRows[0]["IsInventoryWeight"].ToString() != "")
            {
                if (Convert.ToBoolean(foundRows[0]["IsInventoryWeight"]))
                {
                    //txtQuantity.Enabled = false;
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
        txtQuantity.Focus();
    }

    private void PrintDemand(int Demandid)
    {
        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;
        ds = _rptInvenController.GetDemandFinishGood(Demandid);
        var crpReport = new CORNBusinessLayer.Reports.CrpStockDemandPrint();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }    
}