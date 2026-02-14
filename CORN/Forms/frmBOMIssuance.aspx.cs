using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using CORNBusinessLayer.Reports;

public partial class Forms_frmBOMIssuance : System.Web.UI.Page
{

    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly BOMIssuanceController BOMCtl = new BOMIssuanceController();
    readonly ProdcutionPlanController ProdCtrl = new ProdcutionPlanController();
    readonly DataControl dc = new DataControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        try
        {
            if (!IsPostBack)
            {
                GetAppSettingDetail();
                Session.Remove("dtGridData");
                LoadDistributor();
                LoadUOM();
                LoadSKUFinished(6,Constants.LongNullValue);
                CreateSKUDataTable();                
                GetFinishedDetail(3);
                LoadGridData();
                LoadGridMaster("");
                DataTable dtFinance = (DataTable)Session["dtAppSettingDetail"];
                if (dtFinance.Rows.Count > 0)
                {
                    bool IsFinanceIntegrate = false;
                    if(dtFinance.Rows[0]["IsFinanceIntegrate"].ToString() == "1")
                    {
                        IsFinanceIntegrate = true;
                    }
                    DataTable dtCOAConfig = GetCOAConfiguration();
                    HttpContext.Current.Session.Add("dtCOAConfig", dtCOAConfig);
                    HttpContext.Current.Session.Add("IsFinanceIntegrate", IsFinanceIntegrate);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region Load

    private void LoadDistributor()
    {
        var dController = new DistributorController();
        DataTable dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);
        if(drpDistributor.Items.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }

        Session.Add("dtLocationInfo", dt);
    }

    private void LoadUOM()
    {
        GeoHierarchyController DptTpe = new GeoHierarchyController();
        drpSkuUnit.Items.Clear();
        DataTable dt = DptTpe.GetUOM(0, Constants.IntNullValue, Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(drpSkuUnit, dt, 0, 1, true);
    }

    private void LoadSKUFinished(int TypeID,long lngBOMIssuanceCode)
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

        DataTable dtProdItem = ProdCtrl.SelectProdcutionPlanInfo(Constants.IntNullValue, lngBOMIssuanceCode, Convert.ToInt32(drpDistributor.Value), CurrentWorkDate, CurrentWorkDate, TypeID);
        clsWebFormUtil.FillDxComboBoxList(this.ddlRecipeItem, dtProdItem, "lngProductionPlanCode", "SKU_NAME", true);
        if (ddlRecipeItem.Items.Count > 0)
        {
            ddlRecipeItem.SelectedIndex = 0;
            drpSkuUnit.SelectedValue = dtProdItem.Rows[0]["UOM"].ToString();
        }        
        Session.Add("dtProdItem", dtProdItem);
    }

    private void GetFinishedDetail(int TypeID)
    {
        Clear();
        if (ddlRecipeItem.Items.Count > 0 && drpDistributor.Items.Count > 0)
        {
            DataTable dtProdItem = (DataTable)Session["dtProdItem"];
            int ProdItem = 0;
            foreach (DataRow dr in dtProdItem.Rows)
            {
                if (dr["lngProductionPlanCode"].ToString() == ddlRecipeItem.Value.ToString())
                {
                    ProdItem = Convert.ToInt32(dr["FINISHED_SKU_ID"]);
                }
            }

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

            txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");

            txtClosing.Text = Convert.ToString(CheckStockStatus(ProdItem, CurrentWorkDate));
            hfClosing.Value = txtClosing.Text;
            DataTable dtSKU = ProdCtrl.SelectProdcutionPlanInfo(Convert.ToInt32(ddlRecipeItem.Value), Convert.ToInt64(ddlRecipeItem.Value), Convert.ToInt32(drpDistributor.Value), CurrentWorkDate, CurrentWorkDate, TypeID);
            if (dtSKU.Rows.Count > 0)
            {
                txtRecipeQty.Text = dtSKU.Rows[0]["Recipe_Qty"].ToString();
                Session.Add("dtSKU", dtSKU);
                LoadGrid();
            }
        }
    }
    private void GetBOMIssuanceDetail()
    {
        Clear();        
        long lngBOMIssuanceCode = Constants.LongNullValue;
        if (hfMasterId.Value != null)
        {
            if (hfMasterId.Value != "")
            {
                lngBOMIssuanceCode = Convert.ToInt64(hfMasterId.Value);
            }
        }
        if (drpDistributor.Items.Count > 0)
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

            txtClosing.Text = Convert.ToString(CheckStockStatus(int.Parse(ddlRecipeItem.Value.ToString()), CurrentWorkDate));
            hfClosing.Value = txtClosing.Text;

            DataTable dtSKU = BOMCtl.SelectBOMIssuanceInfo(Constants.IntNullValue, lngBOMIssuanceCode,
                Constants.IntNullValue, CurrentWorkDate, CurrentWorkDate, 3);

            if (dtSKU.Rows.Count > 0)
            {
                if (int.Parse(dtSKU.Rows[0]["DISTRIBUTOR_ID"].ToString()) > 0)
                {
                    drpDistributor.Enabled = false;
                    drpDistributor.Value = dtSKU.Rows[0]["DISTRIBUTOR_ID"].ToString();
                }
                else
                {
                    drpDistributor.Enabled = true;
                    drpDistributor.SelectedIndex = 0;
                }
                LoadSKUFinished(5,lngBOMIssuanceCode);
                //ddlRecipeItem.SelectedItem.Value = dtSKU.Rows[0]["FINISHED_SKU_ID"];
                txtIssueQty.Text = dtSKU.Rows[0]["Recipe_Qty"].ToString();
                btnSave.Text = dtSKU.Rows[0]["TYPE"].ToString();
                Session.Add("dtSKU", dtSKU);
                LoadGrid();
            }
        }
    }
    private void LoadGrid()
    {
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        gvSKU.DataSource = dtSKU;
        gvSKU.DataBind();
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
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

        dt = BOMCtl.SelectBOMIssuanceInfo(Constants.IntNullValue, Constants.LongNullValue, Convert.ToInt32(Session["UserID"]), CurrentWorkDate, CurrentWorkDate, 1);
        Session.Add("dtGridData", dt);
    }
    private void LoadGridMaster(string pType)
    {
        DataTable dt = (DataTable)Session["dtGridData"];
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = " SKU_NAME LIKE '%" + txtSearch.Text + "%'";
            }
            GrdRecipe.DataSource = dt;
            GrdRecipe.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = " SKU_NAME LIKE '%" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            GrdRecipe.DataSource = dt;
            GrdRecipe.DataBind();
        }
        Session.Add("dtMaster", dt);
    }

    private DataTable GetCOAConfiguration()
    {
        try
        {
            COAMappingController _cController = new COAMappingController();
            DataTable dtCOAConfig = _cController.SelectCOAConfiguration(5, Constants.ShortNullValue, Constants.LongNullValue, "Level 4");

            if (dtCOAConfig.Rows.Count > 0)
            {
                return dtCOAConfig;
            }
            return null;
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Plz Configure Financial Integration Settings');", true);

            return null;
        }
    }

    #endregion

    private void CreateSKUDataTable()
    {
        DataTable dtSKU = new DataTable();
        dtSKU.Columns.Add("SKU_ID", typeof(int));
        dtSKU.Columns.Add("QUANTITY", typeof(decimal));
        dtSKU.Columns.Add("SKU_NAME", typeof(string));
        dtSKU.Columns.Add("UOM_ID", typeof(int));
        dtSKU.Columns.Add("UOM_DESC", typeof(string));
        dtSKU.Columns.Add("FINISHED_GOOD_DETAIL_ID", typeof(int));
        dtSKU.Columns.Add("CLOSING_STOCK", typeof(decimal));
        dtSKU.Columns.Add("ORG_QUANTITY", typeof(decimal));
        dtSKU.Columns.Add("ActulQty", typeof(decimal));
        dtSKU.Columns.Add("OrignalQty", typeof(decimal));
        Session.Add("dtSKU", dtSKU);
    }

    protected void gvSKU_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            mPopUpSection.Show();
            RowId.Value = e.NewEditIndex.ToString();
            hfClosingDetail.Value = gvSKU.Rows[e.NewEditIndex].Cells[6].Text;
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void ddlRecipeItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            mPopUpSection.Show();
            CreateSKUDataTable();
            GetFinishedDetail(3);
            DataTable dtProdItem = (DataTable)Session["dtProdItem"];
            foreach (DataRow dr in dtProdItem.Rows)
            {
                if (dr["lngProductionPlanCode"].ToString() == ddlRecipeItem.Value.ToString())
                {
                    drpSkuUnit.SelectedValue = dr["UOM"].ToString();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private decimal CheckStockStatus(int skuId, DateTime workDate)
    {

        var mController = new PhaysicalStockController();
        DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.Value.ToString()), skuId, "N/A", workDate, 12);
        if (dt.Rows.Count > 0)
        {
            return decimal.Parse(dt.Rows[0][0].ToString());
        }
        return 0;
    }

    #region Click

    protected void btnSave_Click(object sender, EventArgs e)
    {        
        decimal FinishedSKUPrice = 0;        

        DataTable dtSKU = (DataTable)Session["dtSKU"];
        bool stock = true;
        foreach(DataRow dr in dtSKU.Rows)
        {
            foreach(GridViewRow gvr in gvSKU.Rows)
            {
                if(dr["SKU_ID"].ToString() == gvr.Cells[0].Text)
                {
                    dr["Quantity"] = dc.chkNull_0(gvr.Cells[4].Text);
                    FinishedSKUPrice += Convert.ToDecimal(dr["Price"]) * Convert.ToDecimal(dc.chkNull_0(gvr.Cells[4].Text));
                    if(Convert.ToDecimal(dc.chkNull_0(gvr.Cells[4].Text)) > Convert.ToDecimal(dc.chkNull_0(gvr.Cells[6].Text)))
                    {
                        stock = false;
                        break;
                    }
                }
            }
        }

        if(!stock)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Stock is not avaialbe for some item(s).')", true);
            return;
        }

        DataTable dtProdItem = (DataTable)Session["dtProdItem"];        
        try
        {
            if (dtSKU.Rows.Count > 0)
            {
                if (Page.IsValid)
                {
                    long lngBOMIssuanceCode = Constants.LongNullValue;
                    bool IsFinanceIntegrate = (bool)HttpContext.Current.Session["IsFinanceIntegrate"];
                    DataTable dtCOAConfig = (DataTable)HttpContext.Current.Session["dtCOAConfig"];

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

                    if (btnSave.Text == "Save")
                    {
                        int FinishedSKUID = 0;
                        foreach (DataRow dr in dtProdItem.Rows)
                        {
                            if (dr["lngProductionPlanCode"].ToString() == ddlRecipeItem.Value.ToString())
                            {
                                FinishedSKUID = Convert.ToInt32(dr["FINISHED_SKU_ID"]);
                                break;
                            }
                        }

                        lngBOMIssuanceCode = BOMCtl.InsertBOMIssuance(Convert.ToInt64(ddlRecipeItem.Value), int.Parse(drpDistributor.Value.ToString()), FinishedSKUID, decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)), CurrentWorkDate, Convert.ToInt32(Session["UserID"]), "", FinishedSKUPrice, dtSKU, IsFinanceIntegrate, dtCOAConfig);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully saved.')", true);
                    }
                    else
                    {
                        lngBOMIssuanceCode = Convert.ToInt64(hfMasterId.Value);
                        if (BOMCtl.UpdateBOMIssuance(long.Parse(hfMasterId.Value), decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)), Convert.ToInt32(Session["UserID"]), dtSKU, int.Parse(drpDistributor.Value.ToString()), CurrentWorkDate, FinishedSKUPrice, IsFinanceIntegrate, dtCOAConfig))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully updated.')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                        }
                    }
                    CreateSKUDataTable();
                    LoadSKUFinished(6,Constants.LongNullValue);
                    GetFinishedDetail(2);
                    txtSearch.Text = string.Empty;
                    hfMasterId.Value = string.Empty;
                    ddlRecipeItem.Enabled = true;
                    LoadGridData();
                    LoadGridMaster("filter");
                    if (lngBOMIssuanceCode > 0)
                    {
                        ShowReportPopUp(lngBOMIssuanceCode);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('No Raw Material Item found.')", true);
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured')", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        this.CreateSKUDataTable();
        hfMasterId.Value = string.Empty;
    }

    #endregion

    private void Clear()
    {
        gvSKU.DataSource = null;
        gvSKU.DataBind();
        txtIssueQty.Text = "0";
        btnSave.Text = "Save";
        ddlRecipeItem.Enabled = true;
        drpDistributor.Enabled = true;
    }    
    protected void GrdRecipe_RowEditing(object sender, GridViewEditEventArgs e)
    {   
        btnSave.Text = "Update";
        hfMasterId.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[0].Text;
        ddlRecipeItem.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
        ddlRecipeItem.Enabled = false;
        CreateSKUDataTable();
        GetBOMIssuanceDetail();
        mPopUpSection.Show();
    }

    protected void GrdRecipe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdRecipe.PageIndex = e.NewPageIndex;
        LoadGridMaster("");
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        this.LoadGridMaster("filter");
    }

    protected void btnAdd2_Click(object sender, EventArgs e)
    {
        ddlRecipeItem.Enabled = true;
    }

    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        Clear();
        CreateSKUDataTable();
        LoadSKUFinished(6, Constants.LongNullValue);
        GetFinishedDetail(2);
        txtSearch.Text = string.Empty;
        hfMasterId.Value = string.Empty;
        ddlRecipeItem.Enabled = true;
        LoadGridData();
        LoadGridMaster("filter");
    }

    protected void GrdRecipe_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtMaster = (DataTable)this.Session["dtMaster"];

            if (dtMaster.Rows.Count > 0)
            {
                var selectedRow = GrdRecipe.Rows[e.RowIndex];
                if (BOMCtl.DeleteBOMIssuance(long.Parse(selectedRow.Cells[0].Text), 
                    Convert.ToInt32(Session["UserID"]), Convert.ToInt32(selectedRow.Cells[5].Text),
                    Convert.ToDateTime(selectedRow.Cells[3].Text)))
                {
                    CreateSKUDataTable();
                    LoadSKUFinished(6, Constants.LongNullValue);
                    GetFinishedDetail(3);
                    txtSearch.Text = string.Empty;
                    hfMasterId.Value = string.Empty;
                    ddlRecipeItem.Enabled = true;
                    LoadGridData();
                    LoadGridMaster("filter");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully deleted.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnApplye_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        foreach (GridViewRow gvr in gvSKU.Rows)
        {            
            gvr.Cells[4].Text = (Convert.ToDecimal(dc.chkNull_0(gvr.Cells[7].Text)) * Convert.ToDecimal(dc.chkNull_0(txtIssueQty.Text))).ToString();
        }
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        GetFinishedDetail(3);
    }

    public void ShowReportPopUp(long lngBOMIssuanceCode)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            CrpBOMIssuance CrpReport = new CrpBOMIssuance();
            DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
            DataTable dtProduction = BOMCtl.SelectBOMIssuanceInfo(Constants.IntNullValue, lngBOMIssuanceCode, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, 4);

            CrpReport.SetDataSource(dtProduction);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("ReportName", "BOM Issuance Document");
            CrpReport.SetParameterValue("user", Session["UserName"].ToString());

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