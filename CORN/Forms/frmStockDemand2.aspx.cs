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
public partial class Forms_frmStockDemand2 : System.Web.UI.Page
{
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptInventoryController _rptInvenController = new RptInventoryController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    readonly SkuController _mSkuController = new SkuController();
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
            LoadSection();
            LoadCategory();
            LoadSkuDetail();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("STOCK_DEMAND_ID", typeof(long));
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_HIE_NAME", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSku.Columns.Add("QUANTITY", typeof(decimal));
        _purchaseSku.Columns.Add("STOCK_DEMAND_DETAIL_ID", typeof(int));
        _purchaseSku.Columns.Add("DISTRIBUTOR_ID", typeof(int));
        _purchaseSku.Columns.Add("REMARKS", typeof(string));
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
            drpDistributor.Enabled = true;
            ClearAll();
            txtDocumentNo.Text = "";
            DisAbaleOption(false);
        }
        else
        {
            drpDistributor.Enabled = false;
            LoadSkuDetail();
            LoadDocumentDetail();
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();

        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
        GetDocumentNo();
    }

    private void LoadSection()
    {
        DataTable _mDt = _mSkuController.SelectProductSection(0, null, null);
        ddlSection.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlSection, _mDt, "SECTION_ID", "SECTION_NAME");
        if (_mDt.Rows.Count > 0)
        {
            ddlSection.SelectedIndex = 0;
        }
    }
    private void LoadCategory()
    {
        DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 18, Constants.IntNullValue);
        ddlCategory.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlCategory, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
        if (_mDt.Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;
        }
    }
    private void LoadSkuDetail()
    {
        GrdPurchase.Columns[5].Visible = false;
        SkuController SKUCtl = new SkuController();
        DataTable dtskuPrice = new DataTable();
        if (Session["IsLocationWiseItem"].ToString() == "1")
        {
            dtskuPrice = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.IntNullValue, 30, Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.Document_Opening);
        }
        else
        {
            dtskuPrice = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 22, Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.Document_Opening);
        }
        if (cbConsumption.Checked)
        {
            DataTable dtConsumption = SKUCtl.GetSKUConsumption(Convert.ToInt32(drpDistributor.SelectedItem.Value), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            GrdPurchase.Columns[5].Visible = true;
            foreach (DataRow dr in dtskuPrice.Rows)
            {
                DataRow[] foundRows = dtConsumption.Select("SKU_ID  = '" + dr["SKU_ID"].ToString() + "'");
                if (foundRows.Length > 0)
                {
                    dr["Consumption"] = foundRows[0]["Consumption"];
                }
            }
        }

        GrdPurchase.DataSource = dtskuPrice;
        GrdPurchase.DataBind();
        Session.Add("Dtsku_Price", dtskuPrice);
    }
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        {
            DataTable dt = mPurchase.selectStockDemandDetail(Convert.ToInt32(drpDistributor.Value), int.Parse(drpDocumentNo.Value.ToString()), Convert.ToDateTime(Session["CurrentWorkDate"]));
            if (dt.Rows.Count > 0)
            {
                drpDistributor.Value = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
                txtDocumentNo.Text = dt.Rows[0]["REMARKS"].ToString();

                DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

                foreach (DataRow gvr in dt.Rows)
                {
                    foreach (DataRow dr in Dtsku_Price.Rows)
                    {
                        if (gvr["SKU_ID"].ToString() == dr["SKU_ID"].ToString())
                        {
                            dr["Quantity"] = gvr["Quantity"];
                        }
                    }
                }
                GrdPurchase.DataSource = Dtsku_Price;
                GrdPurchase.DataBind();
            }
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
        GridView gv = GrdPurchase;
        DataTable dtStockDemand = new DataTable();
        dtStockDemand.Columns.Add("SKU_ID", typeof(int));
        dtStockDemand.Columns.Add("PRICE", typeof(decimal));
        dtStockDemand.Columns.Add("QUANTITY", typeof(decimal));
        dtStockDemand.Columns.Add("UOM_ID", typeof(int));
        dtStockDemand.Columns.Add("FINISHED_GOOD_ID", typeof(int));
        dtStockDemand.Columns.Add("FINISHED_GOOD_QTY", typeof(decimal));
        DataRow drStockDemand;

        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            foreach (DataRow dr in Dtsku_Price.Rows)
            {
                if (gvr.Cells[0].Text == dr["SKU_ID"].ToString())
                {
                    TextBox txtQuantity = (TextBox)gvr.FindControl("txtQuantity");
                    if (Convert.ToBoolean(dr["IS_Recipe"]))
                    {
                        DataTable dtFinishedGoodItems = _mSkuController.GetFinshedDetail(Convert.ToInt32(dr["SKU_ID"]), int.Parse(drpDistributor.SelectedItem.Value.ToString()), 6);
                        if (dtFinishedGoodItems.Rows.Count > 0)
                        {
                            foreach (DataRow drFinish in dtFinishedGoodItems.Rows)
                            {
                                drStockDemand = dtStockDemand.NewRow();
                                drStockDemand["SKU_ID"] = drFinish["SKU_ID"];
                                drStockDemand["PRICE"] = drFinish["PRICE"];
                                drStockDemand["QUANTITY"] = Convert.ToDecimal(drFinish["QUANTITY"]) * Convert.ToDecimal(_dc.chkNull_0(txtQuantity.Text));
                                drStockDemand["FINISHED_GOOD_QTY"] = dr["QUANTITY"];
                                drStockDemand["UOM_ID"] = drFinish["UOM_ID"];
                                drStockDemand["FINISHED_GOOD_ID"] = dr["SKU_ID"];
                                dtStockDemand.Rows.Add(drStockDemand);
                            }
                        }
                        else
                        {
                            drStockDemand = dtStockDemand.NewRow();
                            drStockDemand["SKU_ID"] = dr["SKU_ID"];
                            drStockDemand["PRICE"] = dr["PRICE"];
                            drStockDemand["QUANTITY"] = txtQuantity.Text;
                            drStockDemand["FINISHED_GOOD_QTY"] = txtQuantity.Text;
                            drStockDemand["UOM_ID"] = dr["UOM_ID"];
                            drStockDemand["FINISHED_GOOD_ID"] = dr["SKU_ID"];
                            dtStockDemand.Rows.Add(drStockDemand);
                        }
                    }
                    else
                    {
                        drStockDemand = dtStockDemand.NewRow();
                        drStockDemand["SKU_ID"] = dr["SKU_ID"];
                        drStockDemand["PRICE"] = dr["PRICE"];
                        drStockDemand["QUANTITY"] = txtQuantity.Text;
                        drStockDemand["FINISHED_GOOD_QTY"] = txtQuantity.Text;
                        drStockDemand["UOM_ID"] = dr["UOM_ID"];
                        drStockDemand["FINISHED_GOOD_ID"] = dr["SKU_ID"];
                        dtStockDemand.Rows.Add(drStockDemand);
                    }
                }
            }
        }
        var mDayClose = new DistributorController();
        int StockDemandId;
        DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.Value.ToString()));
        if (dt.Rows.Count > 0)
        {
            DateTime mWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());
            PurchaseController mController = new PurchaseController();
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                StockDemandId = mController.InsertStockDemand(int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate,
                    dtStockDemand, int.Parse(Session["UserId"].ToString()), txtDocumentNo.Text, false);
            }
            else
            {
                bool mResult = mController.updateStockDemand(int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate,
                    dtStockDemand, int.Parse(Session["UserId"].ToString()), txtDocumentNo.Text,
                    int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), false);
                StockDemandId = int.Parse(drpDocumentNo.SelectedItem.Value.ToString());
            }

            lblErrorMsg.Text = "Record Upated";
            Dtsku_Price.Rows.Clear();
            Session.Add("Dtsku_Price", Dtsku_Price);            ;
            LoadSkuDetail();
            GetDocumentNo();
            ClearAll();
            txtDocumentNo.Text = "";
            DisAbaleOption(false);
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
            PrintDemand(StockDemandId, gv);
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

    private void PrintDemand(int Demandid, GridView gv)
    {
        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds;
        ds = _rptInvenController.StockDemandPrint(Demandid);
        var crpReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();        
        if (cbConsumption.Checked)
        {
            foreach (GridViewRow gvr in gv.Rows)
            {
                foreach(DataRow drDS in ds.Tables["spSelectStockDemandForPrint"].Rows)
                {
                    if(gvr.Cells[0].Text == drDS["SKU_ID"].ToString())
                    {
                        drDS["Consumption"] = gvr.Cells[5].Text;
                    }
                }
            }
            crpReport = new CORNBusinessLayer.Reports.CrpStockDemandPrintConsumption();
        }
        else
        {
            crpReport = new CORNBusinessLayer.Reports.CrpStockDemandPrint();
        }
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

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

            foreach (GridViewRow gvr in GrdPurchase.Rows)
            {
                foreach (DataRow dr in Dtsku_Price.Rows)
                {
                    if (gvr.Cells[0].Text == dr["SKU_ID"].ToString())
                    {
                        TextBox txtQuantity = (TextBox)gvr.FindControl("txtQuantity");
                        dr["Quantity"] = txtQuantity.Text;
                    }
                }
            }

            if (ddlCategory.Value.ToString() == Constants.IntNullValue.ToString())
            {
                if (ddlSection.Value.ToString() == Constants.IntNullValue.ToString())
                {
                    GrdPurchase.DataSource = Dtsku_Price;
                    GrdPurchase.DataBind();
                }
                else
                {
                    DataTable dttableNew = Dtsku_Price.Clone();

                    foreach (DataRow drtableOld in Dtsku_Price.Rows)
                    {
                        if (drtableOld["SECTION_ID"].ToString() == ddlSection.Value.ToString())
                        {
                            dttableNew.ImportRow(drtableOld);
                        }
                    }
                    GrdPurchase.DataSource = dttableNew;
                    GrdPurchase.DataBind();
                }
            }
            else
            {
                DataTable dttableNew = Dtsku_Price.Clone();

                foreach (DataRow drtableOld in Dtsku_Price.Rows)
                {
                    if (drtableOld["SKU_HIE_ID"].ToString() == ddlCategory.Value.ToString())
                    {
                        dttableNew.ImportRow(drtableOld);
                    }
                }
                GrdPurchase.DataSource = dttableNew;
                GrdPurchase.DataBind();
            }
        }
        catch (Exception)
        {
        }
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

            foreach (GridViewRow gvr in GrdPurchase.Rows)
            {
                foreach (DataRow dr in Dtsku_Price.Rows)
                {
                    if (gvr.Cells[0].Text == dr["SKU_ID"].ToString())
                    {
                        TextBox txtQuantity = (TextBox)gvr.FindControl("txtQuantity");
                        dr["Quantity"] = txtQuantity.Text;
                    }
                }
            }

            if (ddlSection.Value.ToString() == Constants.IntNullValue.ToString())
            {
                if (ddlCategory.Value.ToString() == Constants.IntNullValue.ToString())
                {
                    GrdPurchase.DataSource = Dtsku_Price;
                    GrdPurchase.DataBind();
                }
                else
                {
                    DataTable dttableNew = Dtsku_Price.Clone();

                    foreach (DataRow drtableOld in Dtsku_Price.Rows)
                    {
                        if (drtableOld["SKU_HIE_ID"].ToString() == ddlCategory.Value.ToString())
                        {
                            dttableNew.ImportRow(drtableOld);
                        }
                    }
                    GrdPurchase.DataSource = dttableNew;
                    GrdPurchase.DataBind();
                }
            }
            else
            {
                DataTable dttableNew = Dtsku_Price.Clone();

                foreach (DataRow drtableOld in Dtsku_Price.Rows)
                {
                    if (drtableOld["SECTION_ID"].ToString() == ddlSection.Value.ToString())
                    {
                        dttableNew.ImportRow(drtableOld);
                    }
                }
                GrdPurchase.DataSource = dttableNew;
                GrdPurchase.DataBind();
            }
        }
        catch (Exception)
        {
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        mPOPImport.Show();
    }

    protected void btnClose_Import_ServerClick(object sender, EventArgs e)
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
            Response.AddHeader("content-disposition", "attachment;filename=StockDeaminDetail.xlsx");
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
            dtItemDetail.Columns.Add("QUANTITY", typeof(decimal));
            dtItemDetail.Columns.Add("UOM_ID", typeof(int));

            txtFile.PostedFile.SaveAs(pathFolder + filename);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(txtFile.PostedFile.InputStream);
            ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
            var startRow = workSheet.Dimension.Start.Row;
            var endRow = workSheet.Dimension.End.Row;
            int totalCols = 7;

            for (int row = startRow; row <= endRow; row++)
            {
                int SKUID = 0;
                decimal qty = 0;
                for (int col = startRow; col <= totalCols; col++)
                {
                    var cellValue = workSheet.Cells[row + 1, col].Text;
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        if (col == 1)
                            SKUID = Convert.ToInt32(cellValue.ToString());
                        if (col == 5)
                            qty = Convert.ToDecimal(cellValue.ToString());
                    }
                }
                DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + SKUID.ToString() + "'");
                if (foundRows.Length > 0)
                {
                    if (qty > 0)
                    {
                        DataRow dr = dtItemDetail.NewRow();
                        dr["SKU_ID"] = SKUID;
                        dr["PRICE"] = foundRows[0]["PRICE"];
                        dr["QUANTITY"] = qty;
                        dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dtItemDetail.Rows.Add(dr);
                    }
                }
            }
            var mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.Value.ToString()));
            if (dt.Rows.Count > 0 && dtItemDetail.Rows.Count > 0)
            {
                PurchaseController mController = new PurchaseController();
                DateTime mWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());
                int StockDemandId;
                StockDemandId = mController.InsertStockDemand(int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate,
                    dtItemDetail, int.Parse(Session["UserId"].ToString()), txtDocumentNo.Text, false);
                if (StockDemandId > 0)
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
        dtItems.Columns.Add("CLOSING_STOCK", typeof(decimal));
        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            DataRow dr = dtItems.NewRow();
            dr["SKU_ID"] = gvr.Cells[0].Text;
            dr["SKU_NAME"] = gvr.Cells[2].Text;
            dr["UOM"] = gvr.Cells[3].Text;
            dr["CLOSING_STOCK"] = gvr.Cells[4].Text;
            dtItems.Rows.Add(dr);
        }
        int rowIndex = 1;
        ws.Cells[1, 1].Value = "Item ID";
        ws.Cells[1, 2].Value = "Item Name";
        ws.Cells[1, 3].Value = "UOM";
        ws.Cells[1, 4].Value = "Closing Stock";
        ws.Cells[1, 5].Value = "Quantity";
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

    protected void cbConsumption_CheckedChanged(object sender, EventArgs e)
    {
        dvFromDate.Visible = false;
        dvToDate.Visible = false;
        dvToImage.Visible = false;
        dvFromImage.Visible = false;
        btnLoadConsumption.Visible = false;
        if (cbConsumption.Checked)
        {
            dvFromDate.Visible = true;
            dvToDate.Visible = true;
            dvToImage.Visible = true;
            dvFromImage.Visible = true;
            btnLoadConsumption.Visible = true;
        }
        else
        {
            GrdPurchase.Columns[5].Visible = false;
        }
    }

    protected void btnLoadConsumption_Click(object sender, EventArgs e)
    {
        LoadSkuDetail();
    }
}