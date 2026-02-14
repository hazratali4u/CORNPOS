using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmRecipeProduction : System.Web.UI.Page
{

    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
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
                LoadSKU();
                CreateSKUDataTable();
                LoadUOM();
                GetFinishedDetail(4);
                txtProductionDate.Attributes.Add("readonly", "readonly");
                txtRecipeQty.Attributes.Add("readonly", "readonly");
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

                if (CurrentWorkDate != null && CurrentWorkDate != Constants.DateNullValue)
                {
                    txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");
                    txtProductionDate.Text = CurrentWorkDate.ToString("dd/MM/yyyy");
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region Load
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
        if (Session["IsLocationWiseItem"].ToString() == "1")
        {
            Dtsku_Price = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.IntNullValue, 35, int.Parse(Session["CompanyId"].ToString()), null);
        }
        else
        {
            Dtsku_Price = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 9, int.Parse(Session["CompanyId"].ToString()), null);
        }
        clsWebFormUtil.FillDxComboBoxList(this.ddlSKUFinished, Dtsku_Price, 0, 1, true);
        if (ddlSKUFinished.Items.Count > 0)
        {
            ddlSKUFinished.SelectedIndex = 0;
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
        if (ddlSKUFinished.Items.Count > 0 && drpDistributor.Items.Count > 0)
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

            txtClosing.Text = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlSKUFinished.Value), CurrentWorkDate));
            hfClosing.Value = txtClosing.Text;
            DataTable dtSKU = SKUCtl.SelectRecipeInfo(Convert.ToInt32(ddlSKUFinished.Value), lngRecipeProductionCode, Convert.ToInt32(drpDistributor.Value), CurrentWorkDate, TypeID);
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
                txtActualQty.Text = dtSKU.Rows[0]["ActualProduction_Qty"].ToString();
                txtremarks.Text = dtSKU.Rows[0]["REMARKS"].ToString();
                hfFinishMasterId.Value = dtSKU.Rows[0]["FINISHED_GOOD_MASTER_ID"].ToString();
                btnSave.Text = dtSKU.Rows[0]["TYPE"].ToString();
                Session.Add("dtSKU", dtSKU);
                LoadGrid();
            }
        }
    }
    private void LoadSKU()
    {
        try
        {
            DataTable Dtsku_Price = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 6, int.Parse(Session["CompanyId"].ToString()), null);
            clsWebFormUtil.FillDropDownList(this.ddlSKU, Dtsku_Price, 0, 2, true);

        }
        catch (Exception)
        {

            throw;
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

        dt = SKUCtl.SelectRecipeInfo(Constants.IntNullValue, Constants.LongNullValue, Constants.IntNullValue, CurrentWorkDate, 1);
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
        dtSKU.Columns.Add("Price", typeof(decimal));
        Session.Add("dtSKU", dtSKU);
    }
    
    protected void ddlSKUFinished_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            mPopUpSection.Show();
            ddlSKU.SelectedIndex = 0;

            txtQuantity.Text = string.Empty;
            btnAdd.Text = "Add";

            CreateSKUDataTable();
            GetFinishedDetail(4);
            lblError.Text = "";
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();

        try
        {
            if (decimal.Parse(dc.chkNull_0(txtQuantity.Text)) < 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Quantity must be greater than 0.')", true);
            }
        }
        catch (Exception)
        {
        }

        lblError.Text = "";
        DataTable dtSKU = (DataTable)Session["dtSKU"];

        if (txtQuantity.Text.Length > 0)
        {
            if (btnAdd.Text == "Add")
            {
                //Do nothing
            }
            else
            {
                DataRow dr = dtSKU.Rows[Convert.ToInt32(RowId.Value)];
                dr["QUANTITY"] = decimal.Parse(txtQuantity.Text);
                ddlSKU.SelectedIndex = 0;
                hfClosingDetail.Value = "0";
                txtQuantity.Text = string.Empty;
                btnAdd.Text = "Add";
                RowId.Value = "0";
            }

            ScriptManager.GetCurrent(Page).SetFocus(ddlSKU);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Quantity is required');", true);
            return;
        }
        LoadGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        try
        {
            if(decimal.Parse(dc.chkNull_0(txtActualQty.Text)) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Actual Quantity must be greater than 0.')", true);
            }
        }
        catch (Exception)
        {
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

        bool checkstock = true;
        bool zeroConsumedQty = false;
        foreach (GridViewRow gvr in gvSKU.Rows)
        {
            TextBox txtQuantity = gvr.FindControl("txtActualQty") as TextBox;
            if (Convert.ToDecimal(DataControl.chkNull_Zero(txtQuantity.Text)) > Convert.ToDecimal(gvr.Cells[3].Text))
            {
                checkstock = false;
                break;
            }
            if (Convert.ToDecimal(DataControl.chkNull_Zero(txtQuantity.Text)) <= 0)
            {
                zeroConsumedQty = true;
            }
        }
        if (!checkstock)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Consumed Qty can not be greater than available stock. ');", true);
            return;
        }
        else if (zeroConsumedQty == true)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Consumed Qty should be greater than 0. ');", true);
            return;
        }

        lblError.Text = "";
        hfClosing.Value = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlSKUFinished.Value), CurrentWorkDate));
        txtClosing.Text = hfClosing.Value;
        
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        foreach(DataRow dr in dtSKU.Rows)
        {
            foreach(GridViewRow gvr in gvSKU.Rows)
            {
                if(dr["SKU_ID"].ToString() == gvr.Cells[0].Text)
                {
                    TextBox txtActual = (TextBox)gvr.FindControl("txtActualQty");
                    dr["ActulQty"] = dc.chkNull_0(txtActual.Text);
                    dr["OrignalQty"] = dc.chkNull_0(gvr.Cells[8].Text);
                }
            }
        }
        try
        {

            if (dtSKU.Rows.Count > 0)
            {
                if (Page.IsValid)
                {
                    string ConsumptionFromProductionPlan = Session["ConsumptionFromProductionPlan"].ToString();
                    bool IsFinanceIntegrate = (bool)HttpContext.Current.Session["IsFinanceIntegrate"];
                    DataTable dtCOAConfig = (DataTable)HttpContext.Current.Session["dtCOAConfig"];
                    if (btnSave.Text == "Save")
                    {   
                        SKUCtl.InsertRecipeProduction(int.Parse(hfFinishMasterId.Value),
                            int.Parse(drpDistributor.Value.ToString()), Convert.ToInt32(ddlSKUFinished.Value),
                            decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)), 
                            decimal.Parse(dc.chkNull_0(txtActualQty.Text)),
                            CurrentWorkDate,
                            int.Parse(drpSkuUnit.SelectedValue),
                            CurrentWorkDate,
                            Convert.ToInt32(Session["UserID"]), ConsumptionFromProductionPlan, dtSKU,
                            IsFinanceIntegrate, dtCOAConfig, Server.HtmlDecode(txtremarks.Text));

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully saved.')", true);
                    }
                    else
                    {
                        if (SKUCtl.UpdateRecipeProduction(int.Parse(hfMasterId.Value),
                            int.Parse(drpDistributor.Value.ToString()), decimal.Parse(dc.chkNull_0(txtRecipeQty.Text)),
                            int.Parse(ddlSKUFinished.Value.ToString()),
                            decimal.Parse(dc.chkNull_0(txtActualQty.Text)),
                            CurrentWorkDate,
                            int.Parse(drpSkuUnit.SelectedValue),
                            CurrentWorkDate,
                            Convert.ToInt32(Session["UserID"]),ConsumptionFromProductionPlan, dtSKU,
                            IsFinanceIntegrate, dtCOAConfig, Server.HtmlDecode(txtremarks.Text)))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully updated.')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                        }
                    }
                    CreateSKUDataTable();
                    GetFinishedDetail(4);
                    txtSearch.Text = string.Empty;
                    hfMasterId.Value = string.Empty;
                    ddlSKUFinished.Enabled = true;
                    LoadGridData();
                    LoadGridMaster("filter");
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
        txtActualQty.Text = "0";
        hfFinishMasterId.Value = "";
        btnSave.Text = "Save";
        ddlSKUFinished.Enabled = true;
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
        ddlSKUFinished.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;", "");
        ddlSKUFinished.Enabled = false;
        CreateSKUDataTable();
        GetFinishedDetail(3);
        mPopUpSection.Show();
    }

    protected void GrdRecipe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //mPopUpSection.Show();
        GrdRecipe.PageIndex = e.NewPageIndex;
        LoadGridMaster("");
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        this.LoadGridMaster("filter");
    }
    protected void btnAdd2_Click(object sender, EventArgs e)
    {
        ddlSKUFinished.Enabled = true;
    }

    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        Clear();
        hfMasterId.Value = string.Empty;
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
                    if (SKUCtl.DeleteRecipeProduction(long.Parse(selectedRow.Cells[0].Text), Convert.ToInt32(Session["UserID"])))
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
                    if (SKUCtl.DeleteRecipeProduction(long.Parse(selectedRow.Cells[0].Text),
                        Convert.ToInt32(selectedRow.Cells[4].Text),
                        Convert.ToDateTime(selectedRow.Cells[2].Text),
                        Convert.ToInt32(selectedRow.Cells[3].Text), Convert.ToInt32(Session["UserID"])))
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
            TextBox txtAQty = (TextBox)gvr.FindControl("txtActualQty");
            txtAQty.Text = (Convert.ToDecimal(dc.chkNull_0(gvr.Cells[8].Text)) * Convert.ToDecimal(dc.chkNull_0(txtActualQty.Text))).ToString();            
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        LoadSKUFinished();
        ddlSKU.SelectedIndex = 0;
        CreateSKUDataTable();
        GetFinishedDetail(4);
        mPopUpSection.Show();
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