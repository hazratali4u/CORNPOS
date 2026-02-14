using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using CORNBusinessLayer.Reports;

public partial class Forms_frmProductionPlan : System.Web.UI.Page
{

    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly ProdcutionPlanController ProductionCtl = new ProdcutionPlanController();
    readonly SkuController SKUCtl = new SkuController();
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
                Session.Remove("dtGridData");
                this.GetAppSettingDetail();
                LoadDistributor();
                LoadSKUFinished();
                CreateSKUDataTable();
                LoadUOM();
                GetFinishedDetail(2);
                LoadGridData();
                LoadGridMaster("");
                DataTable dtFinance = (DataTable)Session["dtAppSettingDetail"];
                if (dtFinance.Rows.Count > 0)
                {
                    bool IsFinanceIntegrate = Convert.ToInt32(dtFinance.Rows[0]["IsFinanceIntegrate"]) == 1 ? true : false;
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

        dt = ProductionCtl.SelectProdcutionPlanInfo(Constants.IntNullValue, Constants.LongNullValue, Convert.ToInt32(Session["UserID"]), CurrentWorkDate, CurrentWorkDate, 1);
        Session.Add("dtGridData", dt);
    }
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

    private void LoadSKUFinished()
    {
        DataTable Dtsku_Price = new DataTable();
        if(Session["IsLocationWiseItem"].ToString() == "1")
        {
            Dtsku_Price = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.IntNullValue, 35, int.Parse(Session["CompanyId"].ToString()), null);
        }
        else
        {
            Dtsku_Price = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 9, int.Parse(Session["CompanyId"].ToString()), null);
        }
        clsWebFormUtil.FillDxComboBoxList(this.ddlRecipeItem, Dtsku_Price, 0, 1, true);
        if (ddlRecipeItem.Items.Count > 0)
        {
            ddlRecipeItem.SelectedIndex = 0;
        }
    }

    private void GetFinishedDetail(int TypeID)
    {
        Clear();
        long lngRecipeProductionCode = Constants.LongNullValue;
        if(hfMasterId.Value != null)
        {
            if(hfMasterId.Value != "")
            {
                lngRecipeProductionCode = Convert.ToInt64(hfMasterId.Value);
            }
        }
        if (ddlRecipeItem.Items.Count > 0 && drpDistributor.Items.Count > 0)
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

            txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");

            txtClosing.Text = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlRecipeItem.Value), CurrentWorkDate));
            hfClosing.Value = txtClosing.Text;
            DataTable dtSKU = ProductionCtl.SelectProdcutionPlanInfo(Convert.ToInt32(ddlRecipeItem.Value), lngRecipeProductionCode, Convert.ToInt32(drpDistributor.Value), CurrentWorkDate, CurrentWorkDate, TypeID);
            if (dtSKU.Rows.Count > 0)
            {
                if (int.Parse(dtSKU.Rows[0]["intRecipeMUnitCode"].ToString()) > 0)
                {
                    drpSkuUnit.SelectedValue = dtSKU.Rows[0]["intRecipeMUnitCode"].ToString();
                }
                else
                {
                    drpSkuUnit.SelectedIndex = 0;
                }
                
                txtRecipeQty.Text = dtSKU.Rows[0]["Recipe_Qty"].ToString();
                hfFinishMasterId.Value = dtSKU.Rows[0]["FINISHED_GOOD_MASTER_ID"].ToString();
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
            GetFinishedDetail(2);
            mPopUpSection.Show();
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

    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        CreateSKUDataTable();
        GetFinishedDetail(2);
        txtSearch.Text = string.Empty;
        hfMasterId.Value = string.Empty;
        ddlRecipeItem.Enabled = true;
        LoadGridData();
        LoadGridMaster("filter");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {                
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        try
        {
            if (dtSKU.Rows.Count > 0)
            {
                if (Page.IsValid)
                {
                    string ConsumptionFromProductionPlan = Session["ConsumptionFromProductionPlan"].ToString();
                    bool IsFinanceIntegrate = (bool)HttpContext.Current.Session["IsFinanceIntegrate"];
                    DataTable dtCOAConfig = (DataTable)HttpContext.Current.Session["dtCOAConfig"];
                    long lngProductionPlanCode = 0;

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
                        lngProductionPlanCode = ProductionCtl.InsertProductionPlan(int.Parse(hfFinishMasterId.Value), int.Parse(drpDistributor.Value.ToString()), Convert.ToInt32(ddlRecipeItem.Value), decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)), CurrentWorkDate, Convert.ToInt32(Session["UserID"]),ConsumptionFromProductionPlan, dtSKU,IsFinanceIntegrate,dtCOAConfig);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully saved.')", true);
                    }
                    else
                    {
                        lngProductionPlanCode = long.Parse(hfMasterId.Value);
                        if (ConsumptionFromProductionPlan == "1")
                        {
                            if (ProductionCtl.UpdateProductionPlan(long.Parse(hfMasterId.Value), decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)), Convert.ToInt32(Session["UserID"]), dtSKU, int.Parse(drpDistributor.Value.ToString()), CurrentWorkDate, ConsumptionFromProductionPlan,IsFinanceIntegrate,dtCOAConfig))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully updated.')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                            }
                        }
                        else
                        {
                            if (ProductionCtl.UpdateProductionPlan(long.Parse(hfMasterId.Value), decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)), Convert.ToInt32(Session["UserID"]),dtSKU))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully updated.')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                            }
                        }
                    }
                    CreateSKUDataTable();
                    GetFinishedDetail(2);
                    txtSearch.Text = string.Empty;
                    hfMasterId.Value = string.Empty;
                    ddlRecipeItem.Enabled = true;
                    LoadGridData();
                    LoadGridMaster("filter");
                    if (lngProductionPlanCode > 0)
                    {
                        ShowReportPopUp(lngProductionPlanCode);
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
        txtRecipeQty.Text = "0";
        hfFinishMasterId.Value = "";
        btnSave.Text = "Save";
        ddlRecipeItem.Enabled = true;
    }

    public bool IsSKUEditable(int SKUID)
    {
        bool flag = true;
        DataTable dtSKU = SKUCtl.CheckSKU(SKUID);
        if (dtSKU != null)
        {
            if (dtSKU.Rows.Count > 0)
            {
                flag = false;
            }
        }
        return flag;
    }
    
    protected void GrdRecipe_RowEditing(object sender, GridViewEditEventArgs e)
    {   
        btnSave.Text = "Update";
        hfMasterId.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[0].Text;
        drpDistributor.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[5].Text;
        ddlRecipeItem.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
        ddlRecipeItem.Enabled = false;
        CreateSKUDataTable();
        GetFinishedDetail(3);
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
    
    protected void GrdRecipe_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtMaster = (DataTable)this.Session["dtMaster"];
            
            if(dtMaster.Rows.Count > 0)
            {
                var selectedRow = GrdRecipe.Rows[e.RowIndex];
                string ConsumptionFromProductionPlan = Session["ConsumptionFromProductionPlan"].ToString();
                if (ConsumptionFromProductionPlan == "1")
                {
                    if (ProductionCtl.DeleteProductionPlan(long.Parse(selectedRow.Cells[0].Text), 
                        Convert.ToInt32(Session["UserID"]), Convert.ToInt32(selectedRow.Cells[5].Text),
                        Convert.ToDateTime(selectedRow.Cells[3].Text)))
                    {
                        LoadGridData();
                        LoadGridMaster("filter");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully deleted.')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                    }
                }
                else
                {
                    if (ProductionCtl.DeleteProductionPlan(long.Parse(selectedRow.Cells[0].Text), Convert.ToInt32(Session["UserID"])))
                    {
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
            gvr.Cells[3].Text = (Convert.ToDecimal(dc.chkNull_0(gvr.Cells[6].Text)) * Convert.ToDecimal(dc.chkNull_0(txtRecipeQty.Text))).ToString();
        }
    }    

    protected void gvSKU_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow)
        {
            if(Convert.ToDecimal(e.Row.Cells[5].Text) < Convert.ToDecimal(e.Row.Cells[3].Text))
            {
                e.Row.BackColor = System.Drawing.Color.Red;
                e.Row.BorderColor = System.Drawing.Color.Red;
            }
        }
    }

    public void ShowReportPopUp(long lngProductionPlanCode)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            CrpProductionPlan CrpReport = new CrpProductionPlan();
            DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
            DataTable dtProduction = ProductionCtl.SelectProdcutionPlanInfo(Constants.IntNullValue, lngProductionPlanCode,Constants.IntNullValue,Constants.DateNullValue,Constants.DateNullValue,4);

            CrpReport.SetDataSource(dtProduction);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("ReportName", "Production Plan Document");
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

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        LoadSKUFinished();
        GetFinishedDetail(2);
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