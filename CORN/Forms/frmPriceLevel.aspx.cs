using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmPriceLevel : System.Web.UI.Page
{

    readonly SKUPriceLevelController SKUPrice = new SKUPriceLevelController();
    readonly DataControl _dc = new DataControl();

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
                LoadSkuDetail();
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
    
    private void LoadSkuDetail()
    {
        DataTable dtskuPrice = SKUPrice.GetItemPriceLevel(Constants.IntNullValue, 1, Constants.LongNullValue);
        GrdPurchase.DataSource = dtskuPrice;
        GrdPurchase.DataBind();
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = SKUPrice.GetItemPriceLevel(Constants.IntNullValue, 2, Constants.LongNullValue);
        Session.Add("dtGridData", dt);
    }
    private void LoadGridMaster(string pType)
    {
        DataTable dt = (DataTable)Session["dtGridData"];
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = " PriceLevelName LIKE '%" + txtSearch.Text + "%'";
            }
            grdPrice.DataSource = dt;
            grdPrice.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = " PriceLevelName LIKE '%" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            grdPrice.DataSource = dt;
            grdPrice.DataBind();
        }
        Session.Add("dtMaster", dt);
    }

    #endregion
    
    #region Click

    protected void btnSave_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        if (txtPriceLevel.Text.Trim().Length > 0)
        {
            DataTable dtItemPriceDetail = new DataTable();
            dtItemPriceDetail.Columns.Add("PRICE", typeof(decimal));
            dtItemPriceDetail.Columns.Add("SKU_ID", typeof(int));
            decimal Price = 0;
            foreach (GridViewRow gvr in GrdPurchase.Rows)
            {
                TextBox txtPrice = (TextBox)gvr.FindControl("txtPrice");                
                if (txtPrice.Text.Length > 0)
                {
                    try
                    {
                        Price = Convert.ToDecimal(txtPrice.Text);
                    }
                    catch (Exception)
                    {
                        Price = 0;
                    }
                    if (Price > 0)
                    { DataRow dr = dtItemPriceDetail.NewRow();
                        dr["SKU_ID"] = gvr.Cells[0].Text;
                        dr["PRICE"] = txtPrice.Text;
                        dtItemPriceDetail.Rows.Add(dr);
                    }
                }

            }
            if (btnSave.Text == "Save")
            {   
                if (SKUPrice.InsertItemPriceLevel(Convert.ToInt32(drpDistributor.SelectedItem.Value),
                    txtPriceLevel.Text.Trim(), Convert.ToInt32(Session["UserID"]), chkInPercent.Checked,
                    dtItemPriceDetail))
                {
                    txtPriceLevel.Text = string.Empty;
                    LoadSkuDetail();
                    LoadGridData();
                    LoadGridMaster("");
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured');", true);
                }
            }
            else
            {
                if (SKUPrice.UpdateItemPriceLevel(Convert.ToInt64(hfMasterId.Value),
                    Convert.ToInt32(drpDistributor.SelectedItem.Value),
                    txtPriceLevel.Text.Trim(), Convert.ToInt32(Session["UserID"]),1, chkInPercent.Checked, dtItemPriceDetail))
                {
                    txtPriceLevel.Text = string.Empty;
                    btnSave.Text = "Save";
                    LoadSkuDetail();                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully updated');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured');", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Enter Price Level Name.');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        hfMasterId.Value = string.Empty;
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        this.LoadGridMaster("filter");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GrdPurchase.DataSource = null;
        GrdPurchase.DataBind();
        LoadSkuDetail();
        mPopUpSection.Show();
    }

    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        LoadSkuDetail();
        Clear();
        hfMasterId.Value = string.Empty;
    }
    #endregion

    private void Clear()
    {
        hfFinishMasterId.Value = "";
        btnSave.Text = "Save";
        drpDistributor.Enabled = true;
        drpDistributor.SelectedIndex = 0;
    }

    protected void grdPrice_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnSave.Text = "Update";
        hfMasterId.Value = grdPrice.Rows[e.NewEditIndex].Cells[0].Text;
        txtPriceLevel.Text = grdPrice.Rows[e.NewEditIndex].Cells[1].Text;
        drpDistributor.Value = grdPrice.Rows[e.NewEditIndex].Cells[3].Text;
        chkInPercent.Checked = Convert.ToBoolean(grdPrice.Rows[e.NewEditIndex].Cells[4].Text);
        DataTable dtItemDetail = SKUPrice.GetItemPriceLevel(Constants.IntNullValue, 3, Convert.ToInt64(hfMasterId.Value));

        GrdPurchase.DataSource = dtItemDetail;
        GrdPurchase.DataBind();

        mPopUpSection.Show();
        chkInPercent_SelectedIndexChnaged(null, null);
    }

    protected void grdPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPrice.PageIndex = e.NewPageIndex;
        LoadGridMaster("");
    }    

    protected void grdPrice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtMaster = (DataTable)this.Session["dtMaster"];            
            if(dtMaster.Rows.Count > 0)
            {
                DataRow dr = dtMaster.Rows[e.RowIndex];
                if (SKUPrice.DeleteItemPriceLevel(Convert.ToInt64(dr["SKU_PRICES_LEVEL_ID"]), Convert.ToInt32(Session["UserID"]), 2))                    
                {
                    LoadGridData();
                    LoadGridMaster("");
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
    protected void chkInPercent_SelectedIndexChnaged(object sender, EventArgs e)
    {
        try
        {
            if (chkInPercent.Checked == false)
            {
                GrdPurchase.HeaderRow.Cells[3].Text = "Price";
            }
            else
            {
                GrdPurchase.HeaderRow.Cells[3].Text = "Price (%)";
            }

            mPopUpSection.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}