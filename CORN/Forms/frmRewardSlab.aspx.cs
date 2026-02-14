using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class Forms_frmRewardSlab : System.Web.UI.Page
{   
    COAMappingController _cController = new COAMappingController();
    readonly SkuController mSKUController = new SkuController();
    readonly SkuHierarchyController sController = new SkuHierarchyController();
    LoyaltyController LoyaltyCtl = new LoyaltyController();
    DataControl dc = new DataControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            this.LoadLoyaltyCard();
            this.LoadItem();
            LoadGridData();
            LoadLoyaltySalb("");
        }
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = LoyaltyCtl.GetLoyaltyCardSlab(Constants.LongNullValue, Constants.IntNullValue, 1);
        Session.Add("dtGridData", dt);
    }
    private void LoadLoyaltySalb(string pType)
    {
        try
        {
            DataTable dt = (DataTable)Session["dtGridData"];
            gvLoyaltySlab.DataSource = null;
            gvLoyaltySlab.DataBind();
            dt = LoyaltyCtl.GetLoyaltyCardSlab(Constants.LongNullValue,Constants.IntNullValue,1);
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "decPointsFrom = " + txtSearch.Text + "  OR decPointsTo =" + txtSearch.Text + "  OR decCash =" + txtSearch.Text + "  OR decDiscount =" + txtSearch.Text + " OR IsActive LIKE '%" + txtSearch.Text + "%' OR decPointsDeduction =" + txtSearch.Text + "";
                }
                gvLoyaltySlab.DataSource = dt;
                gvLoyaltySlab.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "decPointsFrom = " + txtSearch.Text + "  OR decPointsTo =" + txtSearch.Text + "  OR decCash =" + txtSearch.Text + "  OR decDiscount =" + txtSearch.Text + " OR IsActive LIKE '%" + txtSearch.Text + "%' OR decPointsDeduction =" + txtSearch.Text + "";
                }
                else
                {
                    dt.DefaultView.RowFilter = null;
                }
                if (dt.Rows.Count > 0)
                {
                    gvLoyaltySlab.PageIndex = 0;
                }
                gvLoyaltySlab.DataSource = dt;
                gvLoyaltySlab.DataBind();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }

    private void LoadItem()
    {
        try
        {
            DataTable dt = mSKUController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue,Constants.IntNullValue, 16,int.Parse(Session["CompanyId"].ToString()), null);
            clsWebFormUtil.FillDxComboBoxList(ddlItem, dt, "SKU_ID", "SKU_NAME", false);
            if (ddlItem.Items.Count > 0)
            {
                ddlItem.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void LoadDistributor()
    {
        try
        {
            DistributorController DController = new DistributorController();
            DataTable dt = DController.SelectDistributorInfo(-1, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2, true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            throw;
        }
    }

    private void LoadLoyaltyCard()
    {
        try
        {
            DataTable dt = LoyaltyCtl.SelectLoyaltyCard(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 3);
            clsWebFormUtil.FillDxComboBoxList(ddlLoyaltyCard, dt, "ID", "CARD_NAME", false);
            if (ddlLoyaltyCard.Items.Count > 0)
            {
                ddlLoyaltyCard.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            throw;
        }
    }

    #region Grid Operations   

    protected void gvLoyaltySlab_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLoyaltySlab.PageIndex = e.NewPageIndex;
        LoadLoyaltySalb("");
    }
    protected void gvLoyaltySlab_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ClearControls();
            mPopUpLocation.Show();
            GridViewRow gvr = gvLoyaltySlab.Rows[e.NewEditIndex];
            hfSlabID.Value = gvr.Cells[2].Text;
            rblType.SelectedValue = gvr.Cells[3].Text.ToString();
            rblType.Enabled = false;
            hfType.Value = rblType.SelectedValue;
            DataTable dtLocation = LoyaltyCtl.GetLoyaltyCardSlab(Convert.ToInt64(gvr.Cells[2].Text), Constants.IntNullValue, 2);
            foreach(DataRow dr in dtLocation.Rows)
            {
                foreach(ListItem li in ChbDistributorList.Items)
                {
                    if(li.Value == dr["intLocationID"].ToString())
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
            ddlLoyaltyCard.Value = gvr.Cells[12].Text.ToString();
            if (gvr.Cells[3].Text.ToString() == "1")
            {
                ddlItem.Value = Convert.ToDecimal(gvr.Cells[4].Text).ToString();
                txtPointsFrom.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[5].Text));
                txtPointsTo.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[6].Text));
                txtCash.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[7].Text));
                txtDiscount.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[8].Text));
                txtFreeUnit.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[9].Text));
                txtPointDeduction.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[10].Text));
            }
            else
            {
                txtPointsFrom2.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[5].Text));
                txtPointsTo2.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[6].Text));
                txtMultipleOf.Text = String.Format("{0:0}", Convert.ToDecimal(gvr.Cells[7].Text));
                txtDiscount2.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[8].Text));
                txtPointDeduction2.Text = String.Format("{0:0.00}", Convert.ToDecimal(gvr.Cells[10].Text));                
            }
            btnSave.Text = "Update";
        }

        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    #endregion

    #region Click

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        ClearControls();
        mPopUpLocation.Show();
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in gvLoyaltySlab.Rows)
            {
                var chRelized2 = (CheckBox)dr2.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized2.Checked)
                {
                    check = true;
                    break;
                }

            }
            if (!check)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select record first');", true);
                return;
            }

            bool flag = false;

            foreach (GridViewRow dr in gvLoyaltySlab.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[11].Text) == "Active")
                    {
                        _UserCtrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[2].Text), Convert.ToInt32(Session["UserID"]), 10);
                        flag = true;
                    }
                    else
                    {
                        _UserCtrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[2].Text), Convert.ToInt32(Session["UserID"]), 10);
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
            }
            LoadGridData();
            LoadLoyaltySalb("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        btnSave.Text = "Save";
        ClearControls();
       
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadLoyaltySalb("filter");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool flag = false;
            DataTable dtLocation = new DataTable();
            dtLocation.Columns.Add("intLocationID", typeof(int));
            int Count = 0;
            foreach(ListItem li in ChbDistributorList.Items)
            {
                if(li.Selected)
                {
                    Count++;
                    DataRow dr = dtLocation.NewRow();
                    dr["intLocationID"] = li.Value;
                    dtLocation.Rows.Add(dr);
                }
            }
            if(Count ==0)
            {
                mPopUpLocation.Show();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('No location selected!');", true);
                return;
            }
            
            if (btnSave.Text == "Save")
            {
                if (rblType.SelectedItem.Value == "1")
                {
                    if (!(Convert.ToDecimal(dc.chkNull_0(txtPointsFrom.Text)) > 0 && Convert.ToDecimal(dc.chkNull_0(txtPointsTo.Text)) > 0 && Convert.ToDecimal(dc.chkNull_0(txtPointDeduction.Text)) > 0))
                    {
                        mPopUpLocation.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Points From, Points To and Points Deduction must be greater than zero!');", true);
                        return;
                    }
                    if (!(Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)) > 0 || Convert.ToDecimal(dc.chkNull_0(txtCash.Text)) > 0))
                    {
                        mPopUpLocation.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Discount or Cash must be greater than zero!');", true);
                        return;
                    }
                    flag = LoyaltyCtl.InsertLoyaltyCardSalab(Constants.RewardsCard, Convert.ToInt32(rblType.SelectedItem.Value), Convert.ToInt32(ddlItem.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtPointsFrom.Text)), Convert.ToDecimal(dc.chkNull_0(txtPointsTo.Text)), 0, Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)), Convert.ToDecimal(dc.chkNull_0(txtFreeUnit.Text)), Convert.ToDecimal(dc.chkNull_0(txtPointDeduction.Text)), Convert.ToDateTime(Session["CurrentWorkDate"]), Convert.ToInt32(Session["UserID"]),Convert.ToInt32(dc.chkNull_0(txtMultipleOf.Text)), Convert.ToInt32(ddlLoyaltyCard.Value), dtLocation);
                }
                else
                {
                    if (!(Convert.ToDecimal(dc.chkNull_0(txtPointsFrom2.Text)) > 0 && Convert.ToDecimal(dc.chkNull_0(txtPointsTo2.Text)) > 0))
                    {
                        mPopUpLocation.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Points From, Points To and Points Deduction must be greater than zero!');", true);
                        return;
                    }
                    if (!(Convert.ToDecimal(dc.chkNull_0(txtDiscount2.Text)) > 0 || Convert.ToDecimal(dc.chkNull_0(txtMultipleOf.Text)) > 0))
                    {
                        mPopUpLocation.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Discount or Multiple Of must be greater than zero!');", true);
                        return;
                    }
                    flag = LoyaltyCtl.InsertLoyaltyCardSalab(Constants.RewardsCard, Convert.ToInt32(rblType.SelectedItem.Value), Constants.IntNullValue, Convert.ToDecimal(dc.chkNull_0(txtPointsFrom2.Text)), Convert.ToDecimal(dc.chkNull_0(txtPointsTo2.Text)), 0, Convert.ToDecimal(dc.chkNull_0(txtDiscount2.Text)), Constants.DecimalNullValue, Convert.ToDecimal(dc.chkNull_0(txtPointDeduction2.Text)), Convert.ToDateTime(Session["CurrentWorkDate"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(dc.chkNull_0(txtMultipleOf.Text)), Convert.ToInt32(ddlLoyaltyCard.Value), dtLocation);
                }
                if (flag)
                {
                    LoadGridData();
                    LoadLoyaltySalb("");
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Saved Successfully!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Some error occured!')", true);
                }
                mPopUpLocation.Show();
            }
            else
            {
                if (rblType.SelectedItem.Value == "1")
                {
                    flag = LoyaltyCtl.UpdateLoyaltyCardSalab(Convert.ToInt64(hfSlabID.Value), Constants.RewardsCard, Convert.ToInt32(rblType.SelectedItem.Value), Convert.ToInt32(ddlItem.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtPointsFrom.Text)), Convert.ToDecimal(dc.chkNull_0(txtPointsTo.Text)), 0, Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)), Convert.ToDecimal(dc.chkNull_0(txtFreeUnit.Text)), Convert.ToDecimal(dc.chkNull_0(txtPointDeduction.Text)),Convert.ToInt32(Session["UserID"]),true,false,Convert.ToInt32(dc.chkNull_0(txtMultipleOf.Text)),Convert.ToInt32(ddlLoyaltyCard.Value), dtLocation);
                }
                else
                {
                    flag = LoyaltyCtl.UpdateLoyaltyCardSalab(Convert.ToInt64(hfSlabID.Value), Constants.RewardsCard, Convert.ToInt32(rblType.SelectedItem.Value), Constants.IntNullValue, Convert.ToDecimal(dc.chkNull_0(txtPointsFrom2.Text)), Convert.ToDecimal(dc.chkNull_0(txtPointsTo2.Text)), 0, Convert.ToDecimal(dc.chkNull_0(txtDiscount2.Text)), Constants.DecimalNullValue, Convert.ToDecimal(dc.chkNull_0(txtPointDeduction2.Text)), Convert.ToInt32(Session["UserID"]),true,false, Convert.ToInt32(dc.chkNull_0(txtMultipleOf.Text)), Convert.ToInt32(ddlLoyaltyCard.Value), dtLocation);
                }
                if (flag)
                {
                    LoadGridData();
                    LoadLoyaltySalb("");
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Saved Successfully!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Some error occured!')", true);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Error Occured: \n" + ex + "');", true);
        }
    }

    #endregion

    private void ClearControls()
    {
        txtCash.Text = string.Empty;
        txtMultipleOf.Text = string.Empty;
        txtDiscount.Text = string.Empty;
        txtDiscount2.Text = string.Empty;
        txtFreeUnit.Text = string.Empty;
        txtPointDeduction.Text = string.Empty;
        txtPointDeduction2.Text = string.Empty;
        txtPointsFrom.Text = string.Empty;
        txtPointsFrom2.Text = string.Empty;
        txtPointsTo.Text = string.Empty;
        txtPointsTo2.Text = string.Empty;
        foreach(ListItem li in ChbDistributorList.Items)
        {
            li.Selected = false;
        }
        ChbSelectAll.Checked = false;
    }
}