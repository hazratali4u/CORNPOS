using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmSplitItem : System.Web.UI.Page
{

    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly SkuController SKUCtl = new SkuController();
    readonly SplitItemController SplitCtrl = new SplitItemController();
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
                LoadDistributor();
                LoadSKUFinished();
                LoadSKU();
                CreateSKUDataTable();
                LoadUOM();
                LoadGridData();
                LoadGridMaster("");
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
        DataTable dtskuPrice = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 23, int.Parse(Session["CompanyId"].ToString()), 2);

        clsWebFormUtil.FillDxComboBoxList(ddlSplitItem, dtskuPrice, "SKU_ID", "SKU_NAME", true);
        if (ddlSplitItem.Items.Count > 0)
        {
            ddlSplitItem.SelectedIndex = 0;
        }
        hfClosing.Value = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlSplitItem.Value)));
        txtClosing.Text = hfClosing.Value;
        Session.Add("dtItem", dtskuPrice);

        DataTable dtItem = (DataTable)Session["dtItem"];
        foreach (DataRow dr in dtItem.Rows)
        {
            if (dr["SKU_ID"].ToString() == ddlSplitItem.SelectedItem.Value.ToString())
            {
                drpSkuUnit.SelectedValue = dr["UOM_ID"].ToString();
                break;
            }
        }
    }

    private void GetFinishedDetail(int TypeID)
    {
        Clear();
        long lngSplitItemCode = Constants.LongNullValue;
        if(hfMasterId.Value != null)
        {
            if(hfMasterId.Value != "")
            {
                lngSplitItemCode = Convert.ToInt64(hfMasterId.Value);
            }
        }
        if (ddlSplitItem.Items.Count > 0 && drpDistributor.Items.Count > 0)
        {
            txtClosing.Text = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlSplitItem.Value)));
            hfClosing.Value = txtClosing.Text;
            DataTable dtSKU = SplitCtrl.SelectSplitItemInfo(lngSplitItemCode, Convert.ToInt32(drpDistributor.Value), Convert.ToDateTime(Session["CurrentWorkDate"]),TypeID);
            if (dtSKU.Rows.Count > 0)
            {
                if (int.Parse(dtSKU.Rows[0]["intProductionMUnitCode"].ToString()) > 0)
                {
                    drpSkuUnit.SelectedValue = dtSKU.Rows[0]["intProductionMUnitCode"].ToString();
                }
                else
                {
                    drpSkuUnit.SelectedIndex = 0;
                }
                if (int.Parse(dtSKU.Rows[0]["DistributorID"].ToString()) > 0)
                {
                    drpDistributor.Enabled = false;
                    drpDistributor.Value = dtSKU.Rows[0]["DistributorID"].ToString();
                }
                else
                {
                    drpDistributor.Enabled = true;
                    drpDistributor.SelectedIndex = 0;
                }
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
            DataTable Dtsku_Price = SKUCtl.SelectSkuInfo(Convert.ToInt32(drpDistributor.SelectedItem.Value), Constants.IntNullValue, Constants.IntNullValue, 6, int.Parse(Session["CompanyId"].ToString()), null);
            clsWebFormUtil.FillDxComboBoxList(this.ddlSKU, Dtsku_Price, 0, 2, true);
            Session.Add("Dtsku_Price", Dtsku_Price);
            if (Dtsku_Price.Rows.Count > 0)
            {
                ddlSKU.SelectedIndex = 0;
                DataRow[] foundRows2 = Dtsku_Price.Select("SKU_ID = '" + ddlSplitItem.Value.ToString() + "'");
                divConsumedQty.Visible = false;
                txtConsumedQty.Text = string.Empty;
                hfValidateStock.Value = "1";
                if (foundRows2.Length > 0)
                {
                    if (foundRows2[0]["ValidateStockOnSplitItem"].ToString().ToLower() == "false")
                    {
                        divConsumedQty.Visible = true;
                        hfValidateStock.Value = "0";
                    }
                }
            }
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
        dt = SplitCtrl.SelectSplitItemInfo(Constants.LongNullValue, Constants.IntNullValue, Convert.ToDateTime(Session["CurrentWorkDate"]), 1);
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
        dtSKU.Columns.Add("Selling", typeof(decimal));
        dtSKU.Columns.Add("Price", typeof(decimal));
        
        Session.Add("dtSKU", dtSKU);
    }

    protected void gvSKU_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            mPopUpSection.Show();
            RowId.Value = e.NewEditIndex.ToString();
            ddlSKU.Value = gvSKU.Rows[e.NewEditIndex].Cells[0].Text;
            txtPrice.Text = gvSKU.Rows[e.NewEditIndex].Cells[4].Text;
            txtQuantity.Text = gvSKU.Rows[e.NewEditIndex].Cells[5].Text;
            btnAdd.Text = "Update";
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void ddlSplitItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            mPopUpSection.Show();
            ddlSKU.SelectedIndex = 0;
            txtQuantity.Text = string.Empty;
            txtPrice.Text = string.Empty;
            btnAdd.Text = "Add";
            CreateSKUDataTable();
            hfClosing.Value = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlSplitItem.Value)));
            txtClosing.Text = hfClosing.Value;
            DataTable dtItem = (DataTable)Session["dtItem"];
            foreach(DataRow dr in dtItem.Rows)
            {
                if(dr["SKU_ID"].ToString() == ddlSplitItem.SelectedItem.Value.ToString())
                {
                    drpSkuUnit.SelectedValue = dr["UOM_ID"].ToString();
                    break;
                }
            }
            DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
            DataRow[] foundRows2 = Dtsku_Price.Select("SKU_ID = '" + ddlSplitItem.Value.ToString() + "'");
            divConsumedQty.Visible = false;
            txtConsumedQty.Text = string.Empty;
            hfValidateStock.Value = "1";
            if (foundRows2.Length > 0)
            {
                if (foundRows2[0]["ValidateStockOnSplitItem"].ToString().ToLower() == "false")
                {
                    divConsumedQty.Visible = true;
                    hfValidateStock.Value = "0";
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private decimal CheckStockStatus(int skuId)
    {

        var mController = new PhaysicalStockController();
        DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.Value.ToString()), skuId, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()), 12);
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
        if (hfValidateStock.Value == "0")
        {
            if (txtConsumedQty.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Enter consumed qty.')", true);
                return;
            }
        }
        decimal qty = 0;
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKU.SelectedItem.Value + "'");
        DataRow[] foundRows2 = Dtsku_Price.Select("SKU_ID = '" + ddlSplitItem.Value.ToString() + "'");
        bool ValidateStock = true;
        if (foundRows2.Length > 0)
        {
            if(foundRows2[0]["ValidateStockOnSplitItem"].ToString().ToLower() == "false")
            {
                ValidateStock = false;
            }
        }
        if (ValidateStock)
        {
            foreach (GridViewRow gvr in gvSKU.Rows)
            {
                qty += Convert.ToDecimal(gvr.Cells[5].Text);
            }

            if (qty + Convert.ToDecimal(txtQuantity.Text) > Convert.ToDecimal(txtClosing.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Quantity can not be greater than closing qty.')", true);
                return;
            }
        }
        try
        {
            if (decimal.Parse(dc.chkNull_0(txtQuantity.Text)) < 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Quantity must be greater than 0.')", true);
                return;
            }

            if (txtPrice.Text.Length==0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Price is required.')", true);
                return;
            }
        }
        catch (Exception)
        {
        }
        ddlSplitItem.Enabled = false;
        txtConsumedQty.Enabled = false;
        drpDistributor.Enabled = false;        
        if (foundRows.Length > 0)
        {
            if (txtQuantity.Text.Length > 0)
            {
                if (btnAdd.Text == "Add")
                {
                    DataRow dr = dtSKU.NewRow();
                    dr["SKU_ID"] = ddlSKU.SelectedItem.Value;
                    dr["QUANTITY"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                    dr["SKU_NAME"] = ddlSKU.SelectedItem.Text;
                    dr["UOM_DESC"] = foundRows[0]["UOM_DESC"];
                    dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                    dr["Selling"] = foundRows[0]["TRADE_PRICE"];
                    dr["Price"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                    dtSKU.Rows.Add(dr);
                    hfClosingDetail.Value = "0";
                    txtQuantity.Text = string.Empty;
                    txtPrice.Text = string.Empty;
                    Session.Add("dtSKU", dtSKU);
                }
                else
                {
                    DataRow dr = dtSKU.Rows[Convert.ToInt32(RowId.Value)];
                    dr["SKU_ID"] = ddlSKU.SelectedItem.Value;
                    dr["QUANTITY"] = decimal.Parse(txtQuantity.Text);
                    dr["SKU_NAME"] = ddlSKU.SelectedItem.Text;
                    dr["UOM_DESC"] = foundRows[0]["UOM_DESC"];
                    dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                    dr["Selling"] = foundRows[0]["TRADE_PRICE"];
                    dr["Price"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                    btnAdd.Text = "Add";
                }
                ScriptManager.GetCurrent(Page).SetFocus(ddlSKU);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Quantity is required');", true);
                return;
            }
        }
        LoadGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();        
        hfClosing.Value = Convert.ToString(CheckStockStatus(Convert.ToInt32(ddlSplitItem.Value)));
        txtClosing.Text = hfClosing.Value;                
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        decimal qty = 0;
        if (hfValidateStock.Value == "0")
        {
            qty = Convert.ToDecimal(dc.chkNull_0(txtConsumedQty.Text));
        }
        else
        {
            foreach (GridViewRow gvr in gvSKU.Rows)
            {
                qty += Convert.ToDecimal(gvr.Cells[5].Text);
            }
        }
        try
        {
            if (dtSKU.Rows.Count > 0)
            {
                if (Page.IsValid)
                {
                    if (btnSave.Text == "Save")
                    {
                        SplitCtrl.InsertSplitItem(int.Parse(drpDistributor.Value.ToString()), Convert.ToInt32(ddlSplitItem.Value), Convert.ToInt32(drpSkuUnit.SelectedItem.Value), qty,Convert.ToDateTime(Session["CurrentWorkDate"].ToString()), Convert.ToInt32(Session["UserID"]), dtSKU,false,null);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully saved.')", true);
                    }
                    else
                    {
                        if (SplitCtrl.UpdateSplitItem(int.Parse(hfMasterId.Value),Convert.ToInt32(drpDistributor.SelectedItem.Value), Convert.ToDateTime(Session["CurrentWorkDate"].ToString()), Convert.ToInt32(ddlSplitItem.Value),Convert.ToInt32(drpSkuUnit.SelectedItem.Value), qty, Convert.ToInt32(Session["UserID"]), dtSKU,false,null))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully updated.')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                        }
                    }
                    ddlSplitItem.Enabled = true;
                    txtConsumedQty.Enabled = true;
                    drpDistributor.Enabled = true;
                    CreateSKUDataTable();
                    LoadGrid();
                    txtSearch.Text = string.Empty;
                    hfMasterId.Value = string.Empty;
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
        btnAdd.Text = "Add";
        drpDistributor.Enabled = true;
        drpSkuUnit.Enabled = true;
    }

    #endregion

    private void Clear()
    {
        gvSKU.DataSource = null;
        gvSKU.DataBind();
        hfFinishMasterId.Value = "";
        btnSave.Text = "Save";
        ddlSplitItem.Enabled = true;
        txtConsumedQty.Enabled = true;
        txtConsumedQty.Text = string.Empty;
        drpDistributor.Enabled = true;
        drpDistributor.SelectedIndex = 0;
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
        drpDistributor.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;", "");
        ddlSplitItem.Value = GrdRecipe.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
        CreateSKUDataTable();
        GetFinishedDetail(2);
        ddlSplitItem_SelectedIndexChanged(null, null);
        if(hfValidateStock.Value=="0")
        {
            txtConsumedQty.Text = GrdRecipe.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        }
        ddlSplitItem.Enabled = false;
        txtConsumedQty.Enabled = true;        
        mPopUpSection.Show();
    }

    protected void GrdRecipe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        mPopUpSection.Show();
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        this.LoadGridMaster("filter");
    }

    protected void btnAdd2_Click(object sender, EventArgs e)
    {
        ddlSplitItem.Enabled = true;
        txtConsumedQty.Enabled = true;
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
                DataRow dr = dtMaster.Rows[e.RowIndex];

                if (SplitCtrl.DeleteSplitItem(long.Parse(dr["lngSplitItemCode"].ToString()),Convert.ToInt32(dr["DISTRIBUTOR_ID"]),Convert.ToDateTime(dr["DocumentDate"]), Convert.ToInt32(Session["UserID"])))
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
        catch (Exception)
        {

            throw;
        }
    }
}