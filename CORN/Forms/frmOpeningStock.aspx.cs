using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.IO.Ports;
using OfficeOpenXml;
using System.IO;

/// <summary>
/// From To Adjust Stock
/// </summary>
public partial class Forms_frmOpeningStock : System.Web.UI.Page
{
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
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
            this.GetAppSettingDetail();
            CreatTable();
            GetDocumentNo();
            LoadDistributor();
            LoadSkuDetail();
        }
    }

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_Code", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSku.Columns.Add("PRICE", typeof(decimal));
        _purchaseSku.Columns.Add("Quantity", typeof(decimal));
        _purchaseSku.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("UOM_ID", typeof(int));
        _purchaseSku.Columns.Add("S_UOM_ID", typeof(int));
        _purchaseSku.Columns.Add("S_Quantity", typeof(decimal));
        _purchaseSku.Columns.Add("Remarks", typeof(string));
        _purchaseSku.Columns.Add("Expiry_Date", typeof(string));
        Session.Add("PurchaseSKU", _purchaseSku);
    }
    
    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        // DateTime MWorkDate = System.DateTime.Now;

        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(int.Parse(DrpDocumentType.Value.ToString()), Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
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

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);

        if(dt.Rows.Count>0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
    }

    private void LoadSkuDetail()
    {
        hfInventoryType.Value = "0";
        SkuController SKUCtl = new SkuController();
        DataTable dtskuPrice = new DataTable();
        if (Session["IsLocationWiseItem"].ToString() == "1")
        {
            dtskuPrice = SKUCtl.SelectSkuInfo(Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.IntNullValue, Constants.IntNullValue, 34, int.Parse(Session["CompanyId"].ToString()), Convert.ToInt32(DrpDocumentType.Value));
        }
        else
        {
            dtskuPrice = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 13, int.Parse(Session["CompanyId"].ToString()), Convert.ToInt32(DrpDocumentType.Value));
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
        txtPrice.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
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
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(int.Parse(DrpDocumentType.Value.ToString()), Constants.IntNullValue, long.Parse(drpDocumentNo.Value.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.Value = dt.Rows[0]["SOLD_TO"].ToString();
            //drpPrincipal.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
            txtDocumentNo.Text = dt.Rows[0][2].ToString();
            _purchaseSku = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
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
            if (decimal.Parse(_dc.chkNull_0(txtPrice.Text)) > 0 || drpDocumentNo.Value.ToString() != Constants.LongNullValue.ToString())
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
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                            dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                            dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) * decimal.Parse(_dc.chkNull_0(txtPrice.Text)); 
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
                            dr["Remarks"] = "";
                            dr["Expiry_Date"] = DateTime.Now;
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
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dr["UOM_DESC"] = txtUOM.Text;
                        dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                        dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) * decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                        dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                        if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                        {
                            dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                        }
                        else
                        {
                            dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }
                        dr["Remarks"] = "";
                        dr["Expiry_Date"] = DateTime.Now;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Price');", true);
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
    /// <summary>
    /// Saves Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
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

        var mDayClose = new DistributorController();
        DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.Value.ToString()));
        if (dt.Rows.Count > 0)
        {            
            DateTime mWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());

            PurchaseController mController = new PurchaseController();
            DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKU"];
            bool flag = false;
            string ItemName = string.Empty;
            foreach (DataRow dr in dtPurchaseDetail.Rows)
            {
                if(mController.OpeningStockExist(Convert.ToInt32(drpDistributor.SelectedItem.Value),Convert.ToInt32(dr["SKU_ID"])))
                {
                    ItemName = dr["SKU_Name"].ToString();
                    flag = true;
                    break;
                }
            }
            if(flag)
            {
                lblErrorMsg.Text = "Opening Stock already exists for Item " + ItemName;
                return;
            }
                decimal mTotalAmount = dtPurchaseDetail.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["AMOUNT"].ToString()));
            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
                      , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, mTotalAmount,
                      false, dtPurchaseDetail, 0, null, int.Parse(Session["UserId"].ToString()), 0, dtConfig,
                      IsFinanceSetting, Constants.LongNullValue);
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocument(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
                   , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0
                   , mTotalAmount, false, dtPurchaseDetail, 0, null, int.Parse(Session["UserId"].ToString()),
                   0, dtConfig, IsFinanceSetting, Constants.LongNullValue);
            }

            lblErrorMsg.Text = "Record Upated";
            _purchaseSku = (DataTable)Session["PurchaseSKU"];
            _purchaseSku.Rows.Clear();
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
            GetDocumentNo();
            ClearAll();
            txtDocumentNo.Text = "";
            DisAbaleOption(false);
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
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
        txtPrice.Text = string.Empty;
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
        txtQuantity.Enabled = true;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
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

    protected void btnImport_Click(object sender, EventArgs e)
    {
        mPOPImport.Show();
    }
    protected void btnImportClose_Click(object sender, EventArgs e)
    {
        mPOPImport.Hide();
    }

    protected void btnExportOpeningTemplate_Click(object sender, EventArgs e)
    {
        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        using (ExcelPackage p = new ExcelPackage())
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add("ItemList");

            var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
            var headerFont = headerCells.Style.Font;
            headerFont.Bold = true;

            GenerateItemPreFilled(ws, p);

            Byte[] fileBytes = p.GetAsByteArray();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=OpeningItemDetail.xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }

    protected void btnImportOpening_Click(object sender, EventArgs e)
    {
        if (txtFile.PostedFile.ContentLength == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select a file and then upload');", true);
            return;
        }
        else if (txtFile.PostedFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Only excel file are supported');", true);
            return;
        }

        string pathFolder = AppDomain.CurrentDomain.BaseDirectory + "ImportFiles";
        if (!Directory.Exists(pathFolder))
        {
            Directory.CreateDirectory(pathFolder);
        }

        string path = System.IO.Path.GetFullPath(txtFile.PostedFile.FileName);
        string filename = path.Substring(path.LastIndexOf('\\'), path.Length - path.LastIndexOf('\\'));
        if (File.Exists(pathFolder + filename))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('File already Exist in folder. Save file with other name');", true);
            return;
        }
        else
        {
            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataTable dtItemDetail = new DataTable();
            dtItemDetail.Columns.Add("SKU_ID", typeof(int));
            dtItemDetail.Columns.Add("PRICE", typeof(decimal));
            dtItemDetail.Columns.Add("Quantity", typeof(decimal));
            dtItemDetail.Columns.Add("AMOUNT", typeof(decimal));
            dtItemDetail.Columns.Add("UOM_ID", typeof(int));
            dtItemDetail.Columns.Add("S_UOM_ID", typeof(int));
            dtItemDetail.Columns.Add("S_Quantity", typeof(decimal));
            dtItemDetail.Columns.Add("Remarks", typeof(string));
            dtItemDetail.Columns.Add("Expiry_Date", typeof(string));
            dtItemDetail.Columns.Add("SKU_Name", typeof(string));

            txtFile.PostedFile.SaveAs(pathFolder + filename);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(txtFile.PostedFile.InputStream);
            ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
            var startRow = workSheet.Dimension.Start.Row;
            var endRow = workSheet.Dimension.End.Row;
            int totalCols = workSheet.Dimension.End.Column;

            for (int row = startRow; row <= endRow; row++)
            {
                int SKUID = 0;
                decimal qty = 0;
                decimal price = 0;
                for (int col = startRow; col <= totalCols; col++)
                {
                    var cellValue = workSheet.Cells[row + 1, col].Text;
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        if (col == 1)
                            SKUID = Convert.ToInt32(cellValue.ToString());
                        if (col == 4)
                            qty = Convert.ToDecimal(cellValue.ToString());
                        if (col == 5)
                            price = Convert.ToDecimal(cellValue.ToString());
                    }
                }
                DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + SKUID.ToString() + "'");
                if (foundRows.Length > 0)
                {
                    if (qty > 0 && price > 0)
                    {
                        DataRow dr = dtItemDetail.NewRow();
                        dr["SKU_ID"] = SKUID;
                        dr["PRICE"] = price;
                        dr["Quantity"] = qty;
                        dr["AMOUNT"] = price * qty;
                        dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                        if (decimal.Parse(foundRows[0]["UOM_ID"].ToString()) != decimal.Parse(foundRows[0]["S_UOM_ID"].ToString()))
                        {
                            dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(foundRows[0]["DEFAULT_QTY"].ToString()), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(foundRows[0]["PS_FACTOR"].ToString()), qty, Constants.DecimalNullValue, "");
                        }
                        else
                        {
                            dr["S_Quantity"] = qty;
                        }
                        dr["Remarks"] = "";
                        dr["Expiry_Date"] = DateTime.Now;
                        dr["SKU_Name"] = foundRows[0]["SKU_Name"].ToString();
                        dtItemDetail.Rows.Add(dr);
                    }
                }
            }
            var mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.Value.ToString()));
            if (dt.Rows.Count > 0 && dtItemDetail.Rows.Count > 0)
            {
                DateTime mWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());
                PurchaseController mController = new PurchaseController();
                decimal mTotalAmount = dtItemDetail.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["AMOUNT"].ToString()));

                bool flag = false;
                string ItemName = string.Empty;
                foreach (DataRow dr in dtItemDetail.Rows)
                {
                    if (mController.OpeningStockExist(Convert.ToInt32(drpDistributor.SelectedItem.Value), Convert.ToInt32(dr["SKU_ID"])))
                    {
                        ItemName = dr["SKU_Name"].ToString();
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    lblErrorMsg.Text = "Opening Stock already exists for Item " + ItemName;
                    return;
                }

                DataTable dtConfig = GetCOAConfiguration();
                bool IsFinanceSetting = GetFinanceConfig();

                bool mResult = mController.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
                      , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, mTotalAmount,
                      false, dtItemDetail, 0, null, int.Parse(Session["UserId"].ToString()), 0, dtConfig,
                      IsFinanceSetting, Constants.LongNullValue);
                if(mResult)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
                }
            }            
        }
        File.Delete(pathFolder + filename);
    }

    public void GenerateItemPreFilled(ExcelWorksheet ws, ExcelPackage p)
    {
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataTable dtItems = new DataTable();
        dtItems.Columns.Add("SKU_ID", typeof(int));
        dtItems.Columns.Add("SKU_NAME", typeof(string));
        dtItems.Columns.Add("UOM", typeof(string));
        foreach (DataRow drItem in dtskuPrice.Rows)
        {
            DataRow dr = dtItems.NewRow();
            dr["SKU_ID"] = drItem["SKU_ID"];
            dr["SKU_NAME"] = drItem["SKU_NAME"];
            dr["UOM"] = drItem["UOM_DESC"];
            dtItems.Rows.Add(dr);
        }

        int rowIndex = 1;
        ws.Cells[1, 1].Value = "Item ID";
        ws.Cells[1, 2].Value = "Item Name";
        ws.Cells[1, 3].Value = "UOM";
        ws.Cells[1, 4].Value = "Quantity";
        ws.Cells[1, 5].Value = "Price";
        foreach (DataRow DataTableRow in dtItems.Rows)
        {
            int colIndex = 1;
            rowIndex++;
            foreach (DataColumn DataTableColumn in dtItems.Columns)
            {
                var cell = ws.Cells[rowIndex, colIndex];
                cell.Value = DataTableRow[DataTableColumn.ColumnName];
                colIndex++;
            }
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